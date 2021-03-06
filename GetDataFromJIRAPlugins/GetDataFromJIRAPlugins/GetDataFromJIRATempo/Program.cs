﻿using GetDataFromJIRAStructure;
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
        public static DateTime currentTime = DateTime.Now;
        static void Main(string[] args)
        {
            BasicConfigurator.Configure();
            LoggingExtensions.Logging.Log.InitializeWith<LoggingExtensions.log4net.Log4NetLog>();
            log.Info("---------------START----------------");
            log.Info("Started at: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
            //JIRA Authorization
            JIRA_Authorization auth = new JIRA_Authorization();
            if (auth.RunQuery(JIRA_Authorization.JiraResource.project))
            {
                log.Info("Authorization SUCCESS\n");
            }
            else
            {
                log.Info("Authorization FAILED\n");
            }

            JIRA_Worker jiraWorker = new JIRA_Worker();
            log.Info("Import SUCCESS\n");
            log.Info("Finished at: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
            log.Info("---------------FINISH----------------");
        }
    }
}
