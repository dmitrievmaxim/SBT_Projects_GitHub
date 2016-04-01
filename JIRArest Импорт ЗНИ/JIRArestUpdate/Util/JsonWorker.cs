using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace JIRArestUpdate.Util
{
    static class JsonWorker
    {
        public static JObject[] GetJsonArray(string input)
        {
            var resultObjects = AllChildren(JObject.Parse(input)).First(c => c.Type == JTokenType.Array && c.Path.Contains("issues")).Children<JObject>();
            return resultObjects.ToArray();
        }

        private static IEnumerable<JToken> AllChildren(JToken json)
        {
            foreach (var c in json.Children())
            {
                yield return c;
                foreach (var cc in AllChildren(c))
                {
                    yield return cc;
                }
            }
        }

        public static List<JiraIssue> GetJiraIssues(JObject[] input)
        {
            List<JiraIssue> jIssuesArr = new List<JiraIssue>();

            foreach (JObject result in input)
            {
                jIssuesArr.Add(new JiraIssue
                {
                    key = result.Property("key").Value.ToString() ?? null,
                    currentIdZni = Check(result.Property("fields"), Constants.Parameters.IdZniField),
                    currentDateZni = Check(result.Property("fields"), Constants.Parameters.DateZniField)
                });
            }
            return jIssuesArr;
        }

        private static string Check(JProperty obj, string param){
            if (obj != null)
            {
                if ((obj.Value as JObject).Property(Constants.Parameters.IdZniField) != null)
                    return (obj.Value as JObject).Property(param).Value.ToString();
                else return null;
            }
            else return null;
        }
    }
}
