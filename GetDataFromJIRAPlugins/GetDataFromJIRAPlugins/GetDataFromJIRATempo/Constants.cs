using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prop = GetDataFromJIRATempo.Properties;

namespace GetDataFromJIRAStructure
{
    class Constants
    {
        //JIRA
        public static string _jiraProdBaseURL = prop.TempoSettings.Default.jiraProdBaseURL;
        public static string _jiraRESTProdBaseURL = prop.TempoSettings.Default.jiraRESTProdBaseURL;
        public static string _jiraProdUsername = prop.TempoSettings.Default.jiraProdUsername;
        public static string _jiraProdPassword = prop.TempoSettings.Default.jiraProdPassword;

        //Tempo
        public static string _tempoRest = @"rest/tempo-timesheets/3/worklogs?dateFrom={0}&dateTo={1}&projectKey={2}"; //Tempo Timeseet REST API

        //Tempo json attribs source (JSON from JIRA REST END POINT)
        public static string _workTypeSource = @"rest/scriptrunner/latest/custom/getTempoWorkTypeOfActivitiesFromFile";
        public static string _asSource = @"rest/scriptrunner/latest/custom/getTempoWorkProjectsFromFile";

        //DB
        public static string _jiraProdConnectionString = prop.TempoSettings.Default.jiraProdConnectionString;
        public static string _jiraTestConnectionString = prop.TempoSettings.Default.jiraTestConnectionString;

        //APP
        public static int _deltaTime = prop.TempoSettings.Default.deltaTime;
        public static int _threadLimit = prop.TempoSettings.Default.threadLimit;
    }
}

