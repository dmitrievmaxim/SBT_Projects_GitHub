using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using log4net;
using log4net.Config;

namespace GetDataFromJIRAPlugins
{
    public class JIRA_Authorization
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public enum JiraResource
        {
            project
        }

        public bool RunQuery(JiraResource resource, string argument = null, string data = null, string method = "GET")
        {
            try
            {
                log.Info("Start auth");
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                string url = string.Format("{0}{1}/", Constants._jiraRESTProdBaseURL, resource.ToString());

                if (argument != null)
                {
                    url = string.Format("{0}{1}/", url, argument);
                }

                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.ContentType = "application/json";
                request.Method = method;

                if (data != null)
                {
                    using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                    {
                        writer.Write(data);
                    }
                }

                string base64Credentials = GetEncodedCredentials();
                request.Headers.Add("Authorization", "Basic " + base64Credentials);
                //request.UseDefaultCredentials = false;


                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                string result = string.Empty;
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    Debug.WriteLine(result);
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                Console.WriteLine(ex.ToString());
                return false;
            } 
        }

        private string GetEncodedCredentials()
        {
            string mergedCredentials = string.Format("{0}:{1}", Constants._jiraProdUsername, Constants._jiraProdPassword);
            byte[] byteCredentials = UTF8Encoding.UTF8.GetBytes(mergedCredentials);
            return Convert.ToBase64String(byteCredentials);
        }
    }
}
