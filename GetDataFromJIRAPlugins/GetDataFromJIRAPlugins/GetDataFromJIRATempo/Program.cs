﻿using GetDataFromJIRAPlugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace GetDataFromJIRATempo
{
    public class Program
    {
        public static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            BasicConfigurator.Configure();

            LoggingExtensions.Logging.Log.InitializeWith<LoggingExtensions.log4net.Log4NetLog>();
            //JIRA Authorization
            JIRA_Authorization auth = new JIRA_Authorization();
            if (auth.RunQuery(JIRA_Authorization.JiraResource.project))
            {
                Console.WriteLine("Authorization SUCCESS\n");
                log.Info("Authorization SUCCESS\n");
            }
            else
            {
                Console.WriteLine("Authorization FAILED\n");
                log.Info("Authorization FAILED\n");
            }

            JIRA_Worker jiraWorker = new JIRA_Worker();
            Console.WriteLine("Import SUCCESS\n");
            log.Info("Import SUCCESS\n");
            Console.ReadKey();
        }
    }
}