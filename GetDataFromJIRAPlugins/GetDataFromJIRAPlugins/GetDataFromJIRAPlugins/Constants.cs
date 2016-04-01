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
        public static string _jiraProdUsername = "Dmitriev_M";
        public static string _jiraProdPassword = "d22912m";

        //Structure
        public static string _structRest = "rest/structure/1.0/"; //Structure REST API resources
        public static string _getStructList = "structure/"; //GET list structures 
        public static string _getStructForest = "structure/{0}/forest"; //GET a forest {0} - id structure
        public static string _getStructForestIssue = "structure/{0}/forest?root={1}"; //GET a forest issues {0} - id structure, {1} - root issue (example 12345:0 without 0)


        //Tempo


        //DB
        public static string _jiraProdConnectionString = @"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.99.114)(PORT=1500))(CONNECT_DATA=(SERVICE_NAME=SBTJ2)));User Id=SBT_REP;Password=S8T_R3P;";
        public static string _jiraTestConnectionString = @"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.99.102)(PORT=1500))(CONNECT_DATA=(SERVICE_NAME=SBTJ2)));User Id=SBT_REP;Password=S8T_R3P;";


    }
}

