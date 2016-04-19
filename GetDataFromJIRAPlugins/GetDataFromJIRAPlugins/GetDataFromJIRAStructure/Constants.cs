using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prop = GetDataFromJIRAPlugins.Properties;

namespace GetDataFromJIRAPlugins
{
    class Constants
    {
        //JIRA
        public static string _jiraProdBaseURL = prop.StructureSettings.Default.jiraProdBaseURL;
        public static string _jiraRESTProdBaseURL = prop.StructureSettings.Default.jiraRESTProdBaseURL;
        public static string _jiraProdUsername = prop.StructureSettings.Default.jiraProdUsername;
        public static string _jiraProdPassword = prop.StructureSettings.Default.jiraProdPassword;

        //Structure
        public static string _structRest = "rest/structure/1.0/"; //Structure REST API resources
        public static string _getStructList = "structure/"; //GET list structures 
        public static string _getStructForest = "structure/{0}/forest"; //GET a forest {0} - id structure
        public static string _getStructForestIssue = "structure/{0}/forest?root={1}"; //GET a forest issues {0} - id structure, {1} - root issue (example 12345:0 without 0)

        //DB
        public static string _jiraTestConnectionString = prop.StructureSettings.Default.jiraTestConnectionString;
        public static string _jiraProdConnectionString = prop.StructureSettings.Default.jiraProdConnectionString;
    }
}

