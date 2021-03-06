﻿using System;
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

namespace GetDataFromJIRAStructure
{
    class JIRA_Worker
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IList<Attribs.JIRA_Structure_Attribs> ListAllStructures = new List<Attribs.JIRA_Structure_Attribs>();
        private IList<Attribs.JIRA_Structure_Root> ListAllStructuresRoot = new List<Attribs.JIRA_Structure_Root>();
        private IList<Attribs.JIRA_Structure_Root_Forest> ListAllStructuresRootForest = new List<Attribs.JIRA_Structure_Root_Forest>();
        private IList<Attribs.JIRA_Structure_Child> ListAllStructuresChild = new List<Attribs.JIRA_Structure_Child>();
        private IList<Attribs.JIRA_Structure_Child_Forest> ListAllStructuresChildForest = new List<Attribs.JIRA_Structure_Child_Forest>();

        public JIRA_Worker()
        {
            try
            {
                DropCreateExecutes(); //Drop and creates temp table
                string allStructuresJson = GetData(Constants._jiraProdBaseURL + Constants._structRest + Constants._getStructList, WebRequestMethods.Http.Get);
                if (!string.IsNullOrEmpty(allStructuresJson))
                {
                    ListAllStructures = GetObjStructures(allStructuresJson); //All structures
                }
                if ((ListAllStructures != null) && (ListAllStructures.Count() > 0))
                {
                    foreach (var item in ListAllStructures)
                    {
                        string currentStructureJson = GetData(string.Format(Constants._jiraProdBaseURL + Constants._structRest + Constants._getStructForest, item.ID), WebRequestMethods.Http.Get);
                        if (!string.IsNullOrEmpty(currentStructureJson))
                        {
                            List<string> forestRoot = GetForestRoot(currentStructureJson);

                            if ((forestRoot!=null)&&(forestRoot.Count > 0))
                            {

                                foreach (var id in forestRoot)
                                {
                                    //Add data to ListAllStructuresRoot result list
                                    ListAllStructuresRoot.Add(new Attribs.JIRA_Structure_Root { ID = int.Parse(id.Remove(id.Length - 2)) });
                                    //Add data to ListAllStructuresRootForest result list
                                    ListAllStructuresRootForest.Add(new Attribs.JIRA_Structure_Root_Forest { ID = 0, ID_struct = item.ID, ID_root = int.Parse(id.Remove(id.Length - 2)) });

                                    Console.WriteLine("\t1 level: " + item.ID + "\t" + int.Parse(id.Remove(id.Length - 2)));

                                    string forestChildJson = GetData(string.Format(Constants._jiraProdBaseURL + Constants._structRest + Constants._getStructForestIssue, item.ID, id.Remove(id.Length-2)), WebRequestMethods.Http.Get);
                                    if (!string.IsNullOrEmpty(forestChildJson))
                                    {
                                        IEnumerable<string> forestChild = GetForestChild(forestChildJson).Where(s => s.Substring(s.Length - 1) != "0");
                                        if ((forestChild != null) && (forestChild.Count() > 0))
                                        {
                                            foreach (var id_child in forestChild)
                                            {
                                                //Add data to ListAllStructuresChild result list
                                                ListAllStructuresChild.Add(new Attribs.JIRA_Structure_Child { ID = int.Parse(id_child.Remove(id_child.Length - 2)) });
                                                //Add data to ListAllStructuresChildForest result list
                                                ListAllStructuresChildForest.Add(new Attribs.JIRA_Structure_Child_Forest { ID = 0, ID_struct = item.ID, ID_root = int.Parse(id.Remove(id.Length - 2)), ID_child = int.Parse(id_child.Remove(id_child.Length - 2)) });

                                                Console.WriteLine("\t\t2 level: " + int.Parse(id.Remove(id.Length - 2)) + "\t" + int.Parse(id_child.Remove(id_child.Length - 2)));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    ExportData();
                }
                log.Info("Экспортировано: \n" + ListAllStructures.Count + " - Structure\n");
                log.Info(ListAllStructuresRootForest.Count + " - First level\n");
                log.Info(ListAllStructuresChildForest.Count + " - Second level\n");
            }
            catch (Exception ex)
            {
                throw new StructureException(ex.ToString());
            }
        }

        private void DropCreateExecutes()
        {
            try
            {
                //Clean a structure temp table
                Stack<string> tbls = new Stack<string>();
                foreach (sql_w.StructureTables str_table in Enum.GetValues(typeof(sql_w.StructureTables)))
                {
                    tbls.Push(str_table.ToString());
                }

                foreach (string table in tbls)
                {
                    if ((table == sql_w.StructureTables.StructureRootForestTemp.ToString())||(table == sql_w.StructureTables.StructureChildForestTemp.ToString()))
                    {
                        sql_w.Execute(string.Format(sql_w._dropTrigger, table)); //Drop trigger
                        sql_w.Execute(string.Format(sql_w._dropSequence, table)); //Drop sequence
                    }
                    sql_w.Execute(string.Format(sql_w._dropTable, table)); //Drop table
                }

                //Create a structure temp tables, sequences and triggers
                foreach (sql_w.StructureTables str_table in Enum.GetValues(typeof(sql_w.StructureTables)))
                {
                    switch (str_table) //Create temp table
                    {
                        case sql_w.StructureTables.StructureTemp:
                            sql_w.Execute(sql_w._createTable_StructureTemp); break;
                        case sql_w.StructureTables.StructureRootTemp:
                            sql_w.Execute(sql_w._createTable_StructureRootTemp); break;
                        case sql_w.StructureTables.StructureRootForestTemp:
                            sql_w.Execute(sql_w._createTable_StructureRootForestTemp); break;
                        case sql_w.StructureTables.StructureChildTemp:
                            sql_w.Execute(sql_w._createTable_StructureChildTemp); break;
                        case sql_w.StructureTables.StructureChildForestTemp:
                            sql_w.Execute(sql_w._createTable_StructureChildForestTemp); break;
                        case sql_w.StructureTables.Structures:
                            sql_w.Execute(sql_w._createTable_STRUCTURES); break;
                        default: break;
                    }

                    if ((str_table == sql_w.StructureTables.StructureRootForestTemp) || (str_table == sql_w.StructureTables.StructureChildForestTemp))
                    {
                        sql_w.Execute(string.Format(sql_w._createSequence, str_table)); //Create sequence
                        sql_w.Execute(string.Format(sql_w._createTrigger, str_table)); //Create trigger
                    }
                }
                sql_w.Execute(sql_w._createProcedure_StructureRootTemp_Insert);
                sql_w.Execute(sql_w._createProcedure_StructureChildTemp_Insert);
                sql_w.Execute(sql_w._createView_ALLSTRUCTURES); //Create view on production
            }
            catch (Exception ex)
            {
                throw new StructureException(ex.ToString());
            }
        }

        private void ExportData()
        {
            try
            {
                //Export structures
                foreach (var item in ListAllStructures)
                {
                    sql_w.Execute(string.Format(sql_w._insert_StructureTemp, item.ID, item.Name));
                }

                //Export root (first level)
                foreach (var item in ListAllStructuresRoot)
                {
                    sql_w.ExecuteProcedure("STRUCTUREROOTTEMP_INSERT", item.ID);
                    //sql_w.Execute(string.Format(sql_w._insert_StructureRootTemp, item.ID));
                }

                //Export root forest
                foreach (var item in ListAllStructuresRootForest)
                {
                    sql_w.Execute(string.Format(sql_w._insert_StructureRootForestTemp, item.ID, item.ID_struct, item.ID_root));
                }

                //Export child (second level)
                foreach (var item in ListAllStructuresChild)
                {
                    sql_w.ExecuteProcedure("STRUCTURECHILDTEMP_INSERT", item.ID);
                    //sql_w.Execute(string.Format(sql_w._insert_StructureChildTemp, item.ID));
                }

                //Export child forest
                foreach (var item in ListAllStructuresChildForest)
                {
                    sql_w.Execute(string.Format(sql_w._insert_StructureChildForestTemp, item.ID, item.ID_struct, item.ID_root, item.ID_child));
                }

                //Insert result data from production view AllStructures to Structures table on test
                sql_w.Execute(sql_w._insert_STRUCTURES);
            }
            catch (Exception ex)
            {
                throw new StructureException(ex.ToString());
            }
        }
        
        private IList<Attribs.JIRA_Structure_Attribs> GetObjStructures(string text)
        {
            try
            {
                JObject jObj = JObject.Parse(text);

                //get json result objects into a list
                IList<JToken> results = jObj["structures"].Children().ToList();

                //serialize JSON results into objects
                IList<Attribs.JIRA_Structure_Attribs> listObj = new List<Attribs.JIRA_Structure_Attribs>();
                foreach (JToken result in results)
                {
                    Attribs.JIRA_Structure_Attribs obj = JsonConvert.DeserializeObject<Attribs.JIRA_Structure_Attribs>(result.ToString());
                    listObj.Add(obj);
                }
                return listObj;
            }
            catch (Exception ex)
            {
                throw new StructureException(ex.ToString());
            }
        }

        private List<string> GetForestRoot(string textJson)
        {
            try
            {
                List<string> forestRootList = new List<string>();
                var formula = JsonConvert.DeserializeAnonymousType(textJson, new { Formula = "" }).Formula.Split(',');
                foreach (var item in formula)
                {
                    Regex reg = new Regex(@"[0]{1}$");
                    Match match = reg.Match(item);
                    if (match.Success)
                    {
                        forestRootList.Add(item);
                    }
                }
                return forestRootList;
            }
            catch (Exception ex)
            {
                throw new StructureException(ex.ToString());
            }
        }

        private List<string> GetForestChild(string textJson)
        {
            try
            {
                return JsonConvert.DeserializeAnonymousType(textJson, new { Formula = "" }).Formula.Split(',').ToList<string>();
            }
            catch (Exception ex)
            {
                throw new StructureException(ex.ToString());
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
                    Debug.WriteLine(result);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new StructureException(ex.ToString());
            }
        }

        private string GetEncodedCredentials()
        {
            string mergedCredentials = string.Format("{0}:{1}", Constants._jiraProdUsername, Constants._jiraProdPassword);
            byte[] byteCredentials = UTF8Encoding.UTF8.GetBytes(mergedCredentials);
            return Convert.ToBase64String(byteCredentials);
        }

        //Get data using RESTSHARP EXAMPLE
        public string GetDataTest(string baseuri)
        {
            var client = new RestClient(baseuri);
            var request = new RestRequest("rest/structure/1.0/structure", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", "Basic " + GetEncodedCredentials());
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            return null;
        }
    }
}
