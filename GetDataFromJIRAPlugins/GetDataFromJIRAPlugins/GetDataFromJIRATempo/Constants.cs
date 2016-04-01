using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataFromJIRAPlugins
{
    class Constants
    {
        //JIRA
        public static string _jiraProdBaseURL = "https://atlasminsk.sbertech.by/jira/";
        public static string _jiraRESTProdBaseURL = "https://atlasminsk.sbertech.by/jira/rest/api/latest/";
        public static string _jiraProdUsername = "TempoAdmin"; //"Dmitriev_M";
        public static string _jiraProdPassword = "Ntvgjflvby"; //"d22912m";

        //Tempo
        public static string _tempoRest = @"rest/tempo-timesheets/3/worklogs?dateFrom={0}&dateTo={1}&projectKey={2}"; //Tempo Timeseet REST API

        //Tempo json attribs source (JSON from JIRA REST END POINT)
        public static string _workTypeSource = @"rest/scriptrunner/latest/custom/getTempoWorkTypeOfActivitiesFromFile";
        public static string _asSource = @"rest/scriptrunner/latest/custom/getTempoWorkProjectsFromFile";

        //DB
        public static string _jiraProdConnectionString = @"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.99.114)(PORT=1500))(CONNECT_DATA=(SERVICE_NAME=SBTJ2)));User Id=SBT_REP;Password=S8T_R3P;";
        public static string _jiraTestConnectionString = @"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.99.102)(PORT=1500))(CONNECT_DATA=(SERVICE_NAME=SBTJ2)));User Id=SBT_REP;Password=S8T_R3P;";

    }
}

