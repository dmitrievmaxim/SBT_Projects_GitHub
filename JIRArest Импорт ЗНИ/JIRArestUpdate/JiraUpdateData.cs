using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jira;
using RestSharp;
using param = JIRArestUpdate.Constants.Parameters;
using System.Diagnostics;

namespace JIRArestUpdate
{
    class JiraUpdateData
    {
        string _jiraUrl = Constants.Parameters.JiraUrl;
        readonly string _accountId = Constants.Parameters.AccountId;
        readonly string _password = Constants.Parameters.Password;

        private Item Entity { get; set; }

        public JiraUpdateData(Item item)
        {
            Entity = item;
            FindIssueFromZni();
        }

        private string Execute(RestRequest request)
        {
            var client = new RestClient(_jiraUrl);
            client.Authenticator = new HttpBasicAuthenticator(_accountId, _password);
            request.AddParameter("AccountSid", _accountId, ParameterType.UrlSegment);
            var response = client.Execute(request);
            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var jiraManagerException = new ApplicationException(message, response.ErrorException);
                throw jiraManagerException;
            }
            return response.Content;
        }

        private T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient(_jiraUrl);
            client.Authenticator = new HttpBasicAuthenticator(_accountId, _password);
            request.AddParameter("AccountSid", _accountId, ParameterType.UrlSegment);
            var response = client.Execute<T>(request);
            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var jiraManagerException = new ApplicationException(message, response.ErrorException);
                throw jiraManagerException;
            }
            return response.Data;
        }

        public void FindIssueFromZni()
        {
            RestRequest request = new RestRequest("search?fields=" + param.IdZniField + "," + param.DateZniField + "&jql=project%20=%20ASBPS%20AND%20cf[10016]~\"{key}\"", Method.GET);
            request.AddUrlSegment("key", Entity.zni);
            request.RequestFormat = DataFormat.Json;
            var response = Execute(request);
            List<JiraIssue> issues = Util.JsonWorker.GetJiraIssues(Util.JsonWorker.GetJsonArray(response));
            foreach (var issue in issues)
            {
                if (issue.currentIdZni == Entity.zni)
                {
                    if (issue.currentDateZni == Entity.date) continue;
                    Program.Counter++;
                    SetJiraIssue(issue);
                    Trace.WriteLine(Program.Counter + "\t" + issue.key + "\told date:" + issue.currentDateZni + "\tnew date:" + Entity.date);
                    Console.WriteLine(Program.Counter + "\t" + issue.key + "\told date:" + issue.currentDateZni + "\tnew date:" + Entity.date);
                }
            }
        }

        public void SetJiraIssue(JiraIssue issue)
        {
            RestRequest request = new RestRequest("issue/{key}", Method.PUT);
            request.AddUrlSegment("key", issue.key);
            request.RequestFormat = DataFormat.Json;
            string jSonContent = @"{""fields"":{""" + Constants.Parameters.DateZniField + @""":""" + Entity.date + @"""}}";
            request.AddParameter("application/json", jSonContent, ParameterType.RequestBody);
            var response = Execute(request);
            Console.WriteLine(response);
        }
    }
}
