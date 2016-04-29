using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using RestSharp;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Text.RegularExpressions;
using sql_w = GetDataFromJIRAStructure.SQL_Worker;
using prop = GetDataFromJIRATempo.Properties;
using GetDataFromJIRATempo;
using System.Threading;

namespace GetDataFromJIRAStructure
{
    class JIRA_Worker
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IList<Attribs.JIRA_Project> _listAllProjects = new List<Attribs.JIRA_Project>();
        private List<Attribs.JIRA_Tempo_Attribs> _listAllTimesheets = new List<Attribs.JIRA_Tempo_Attribs>();

        //Custom fields mapping objects
        IList<GetDataFromJIRATempo.Attribs.JIRA_mapping> _workTypeMapping = new List<GetDataFromJIRATempo.Attribs.JIRA_mapping>();
        IList<GetDataFromJIRATempo.Attribs.JIRA_mapping> _asMapping = new List<GetDataFromJIRATempo.Attribs.JIRA_mapping>();
      
        public JIRA_Worker()
        {
            try
            {
                DropCreateExecutes(); //Drop and creates tempo table
                sql_w.Execute(string.Format(sql_w._getProjects), Constants._jiraProdConnectionString, ref _listAllProjects);//Fill projects list
                //Fill custom fields mapping lists
                MappingAttribs(ref _workTypeMapping, Constants._workTypeSource);
                MappingAttribs(ref _asMapping, Constants._asSource);

                DateTime startDate = Program.currentTime.AddDays(-Constants._deltaTime);
                DateTime finDate;
                double threadCount = Math.Ceiling((double)Constants._deltaTime / (double)Constants._threadLimit);

                Dictionary<Data, Thread> dictThreads = new Dictionary<Data, Thread>();
                for (int t = 1; t <= threadCount; t++)
                {
                    if (startDate.AddDays(t * Constants._threadLimit) <= Program.currentTime)
                    {
                        finDate = startDate.AddDays(t * Constants._threadLimit);
                    }
                    else finDate = Program.currentTime;
                    if (startDate < finDate)
                    {
                        Data data = new Data(startDate, finDate);
                        Thread thread = new Thread(GetDataFromThread);
                        dictThreads.Add(data.Clone() as Data, thread);
                        startDate = startDate.AddDays(Constants._threadLimit + 1);
                    }
                }
                foreach (var thread in dictThreads)
                {
                    thread.Value.Start(thread.Key);
                }

                foreach (var thread in dictThreads)
                {
                    do
                    {
                        Thread.Sleep(100);
                    } while (thread.Value.IsAlive);
                }

                ExportData();
                UpdateInsertTempo();
                DropTables();
            }
            catch (Exception ex)
            {
                throw new TempoException(ex.ToString());
            }
        }

        private void UpdateInsertTempo()
        {
            sql_w.Execute(sql_w._update_TempoLabor);
            sql_w.Execute(sql_w._insert_TempoLabor);
        }

        private void GetDataFromThread (object dataObj)
        {
            try
            {
                foreach (var project in _listAllProjects)
                {
                    if (project.Name_project_PKEY == "SUPPORTDPT")
                    {
                        Console.WriteLine();
                    }
                    Debug.WriteLine(project.Name_project);
                    string data = GetData(string.Format(Constants._jiraProdBaseURL + Constants._tempoRest, (dataObj as Data).startDate.ToString("yyyy-MM-dd"), (dataObj as Data).finDate.ToString("yyyy-MM-dd"), project.Name_project_PKEY), WebRequestMethods.Http.Get);
                    if ((!string.IsNullOrEmpty(data)) && !data.Equals("[]"))
                    {
                        lock (_listAllTimesheets)
                        {
                            _listAllTimesheets = _listAllTimesheets.Concat(GetObjTempo(data)).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TempoException(ex.ToString());
            }
        }

        private void DropCreateExecutes()
        {
            try
            {
                DropTables();
                //Create a tempo tables, sequences and triggers
                foreach (sql_w.TempoTables str_table in Enum.GetValues(typeof(sql_w.TempoTables)))
                {
                    switch (str_table)
                    {
                        case sql_w.TempoTables.TempoLabor:
                            sql_w.Execute(sql_w._createTable_TempoLabor); break;
                        case sql_w.TempoTables.TempoLaborTmp:
                            sql_w.Execute(sql_w._createTable_TempoLaborTmp); break;
                        default: break;
                    }

                    if ((str_table == sql_w.TempoTables.TempoLabor) || (str_table == sql_w.TempoTables.TempoLaborTmp))
                    {
                        sql_w.Execute(string.Format(sql_w._createSequence, str_table)); //Create sequence
                        sql_w.Execute(string.Format(sql_w._createTrigger, str_table)); //Create trigger
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TempoException(ex.ToString());
            }
        }

        private void DropTables()
        {
            //Clean a TMP Tempo table
            Stack<string> tbls = new Stack<string>();
            foreach (sql_w.TempoTables str_table in Enum.GetValues(typeof(sql_w.TempoTables)))
            {
                tbls.Push(str_table.ToString());
            }

            foreach (string table in tbls)
            {
                if ((table == sql_w.TempoTables.TempoLaborTmp.ToString()))
                {
                    sql_w.Execute(string.Format(sql_w._dropTrigger, table)); //Drop trigger
                    sql_w.Execute(string.Format(sql_w._dropSequence, table)); //Drop sequence
                    sql_w.Execute(string.Format(sql_w._dropTable, table)); //Drop table
                }
            }
        }

        private void MappingAttribs(ref IList<GetDataFromJIRATempo.Attribs.JIRA_mapping> mapObj, string source)
        {
            try
            {
                string startJsonPattern = @"^\w+\s\(\s";
                Regex reg = new Regex(startJsonPattern);
                string mapJson = GetData(string.Format(Constants._jiraProdBaseURL + source), WebRequestMethods.Http.Get);
                var substrStart = reg.Match(mapJson).Index + reg.Match(mapJson).Value.Length;
                var substrFin = reg.Match(mapJson).Index;
                Debug.WriteLine(mapJson.Length);
                mapJson = mapJson.Substring(substrStart, mapJson.Length - substrStart - 1);

                if (!string.IsNullOrEmpty(mapJson))
                {
                    JObject jObj = JObject.Parse(mapJson);
                    IList<JToken> result = jObj["values"].Children().ToList();
                    foreach (JToken token in result)
                    {
                        var item = JsonConvert.DeserializeObject<GetDataFromJIRATempo.Attribs.JIRA_mapping>(token.ToString());
                        mapObj.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TempoException(ex.ToString());
            }
        }

        private List<Attribs.JIRA_Tempo_Attribs> GetObjTempo(string jsonText)
        {
            try
            {
                List<Attribs.JIRA_Tempo_Attribs> atrList = new List<Attribs.JIRA_Tempo_Attribs>();
                JArray jArr = JArray.Parse(jsonText);
                foreach (var jObj in jArr)
                {
                    Attribs.JIRA_Tempo_Attribs currentItem = JsonConvert.DeserializeObject<Attribs.JIRA_Tempo_Attribs>(jObj.ToString());

                    foreach (var worklog_attr in currentItem.Worklog_attribs)
                    {
                        if (Enum.IsDefined(typeof(Attribs.JIRA_customfield_attribs), worklog_attr.Key))
                        switch ((Attribs.JIRA_customfield_attribs)Enum.Parse(typeof(Attribs.JIRA_customfield_attribs), worklog_attr.Key))
                        {
                            case Attribs.JIRA_customfield_attribs._АС_:
                                {
                                    foreach (var _as in _asMapping)
                                    {
                                        if (worklog_attr.Value == _as.Key)
                                        {
                                            currentItem.AS_num = _as.Key;
                                            currentItem.AS_val = _as.Value.Replace(_as.Key, "").Trim();
                                            break;
                                        }
                                    }
                                    break;
                                }
                            case Attribs.JIRA_customfield_attribs._Типработ_:
                                {
                                    foreach (var _pType in _workTypeMapping)
                                    {
                                        if (worklog_attr.Value == _pType.Key)
                                        {
                                            currentItem.WorkType_val = _pType.Value;
                                            break;
                                        }
                                    }
                                    break;
                                }
                            case Attribs.JIRA_customfield_attribs._Account_:
                                {
                                    currentItem.Account = worklog_attr.Value;
                                } break;
                            default: break;
                        }
                    }
                    atrList.Add(currentItem);
                }
                return atrList;
            }
            catch (Exception ex)
            {
                throw new TempoException(ex.ToString());
            }
        }

        private void ExportData()
        {
            try
            {
                //Export tempo
                foreach (var item in _listAllTimesheets)
                {
                    sql_w.Execute(string.Format(sql_w._insert_TempoLaborTmp, item.ID_identity, item.ID_timesheet, item.ID_issue, (item.Issue_name = item.Issue_name ?? "").Replace("'", "''''"), (item.Summary = item.Summary ?? "").Replace("'", "''''"), item.Timespent, item.Workdate.ToString("d"), item.Full_name, item.User_name, item.Issue_type, item.ID_project, (item.Description = item.Description ?? "").Replace("'", "''''"), (item.WorkType_val = item.WorkType_val??"").Replace("'", "''''"), (item.AS_num = item.AS_num??"").Replace("'", "''''"), (item.AS_val = item.AS_val??"").Replace("'", "''''"), (item.Account = item.Account??"").Replace("'","''''")));
                }
                log.Info(@"Update/Insert " + _listAllTimesheets.Count + " rows");
            }
            catch (Exception ex)
            {
                throw new TempoException(ex.ToString());
            }
        }
        
        private string GetData(string uri, string method)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
                request.Accept = "application/json";
                request.Method = method;
                request.Headers.Add("Authorization", "Basic " + GetEncodedCredentials());
                //request.UseDefaultCredentials = false;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                string result = string.Empty;

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    //Debug.WriteLine(result);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new TempoException(ex.ToString());
            }
        }

        private string GetEncodedCredentials()
        {

            try
            {
                string mergedCredentials = string.Format("{0}:{1}", Constants._jiraProdUsername, Constants._jiraProdPassword);
                byte[] byteCredentials = UTF8Encoding.UTF8.GetBytes(mergedCredentials);
                return Convert.ToBase64String(byteCredentials);
            }
            catch (Exception ex)
            {
                throw new TempoException(ex.ToString());
            }
        }

        //Get data using RESTSHARP EXAMPLE
        public string GetDataTest(string baseuri)
        {
            try
            {
                var client = new RestClient(baseuri);
                var request = new RestRequest("rest/structure/1.0/structure", Method.GET);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Authorization", "Basic " + GetEncodedCredentials());
                IRestResponse response = client.Execute(request);
                var content = response.Content;
                return content;
            }
            catch (Exception ex)
            {
                throw new TempoException(ex.ToString());
            }
        }
    }

    public class Data:ICloneable
    {
        public DateTime startDate { get; set; }
        public DateTime finDate { get; set; }
        public Data(DateTime startDate, DateTime finDate)
        {
            this.startDate = startDate;
            this.finDate = finDate;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}
