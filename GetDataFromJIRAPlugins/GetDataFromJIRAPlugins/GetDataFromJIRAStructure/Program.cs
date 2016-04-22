using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;

namespace GetDataFromJIRAStructure
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public readonly static string currentDate = DateTime.Now.ToShortDateString();
        static void Main(string[] args)
        {
            log.Info("---------------START----------------");
            log.Info("Started at: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString());
            //JIRA Authorization
            JIRA_Authorization auth = new JIRA_Authorization();
            if (auth.RunQuery(JIRA_Authorization.JiraResource.project))
            {
                log.Info("Authorization SUCCESS\n");
                Console.WriteLine("Authorization SUCCESS\n");
            }
            else
            {
                log.Info("Authorization FAILED\n");
                Console.WriteLine("Authorization FAILED\n");
            }

            JIRA_Worker jiraWorker = new JIRA_Worker();
            log.Info("Import SUCCESS\n");
            log.Info("Finished at: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString());
            log.Info("---------------FINISH----------------");
        }
    }
}