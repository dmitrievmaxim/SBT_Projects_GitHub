using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPSM_email
{
    class Constants
    {
        //JIRA
        public static string _jiraProdBaseURL = "https://atlasminsk.sbertech.by/jira/";
        public static string _jiraRESTProdBaseURL = "https://atlasminsk.sbertech.by/jira/rest/api/latest/";
        public static string _jiraProdUsername = "TempoAdmin"; //"Dmitriev_M";
        public static string _jiraProdPassword = "Ntvgjflvby"; //"d22912m";

        //DB
        public static string _jiraProdConnectionString = @"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.99.114)(PORT=1500))(CONNECT_DATA=(SERVICE_NAME=SBTJ2)));User Id=SBT_REP;Password=S8T_R3P;";
        public static string _jiraTestConnectionString = @"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.99.102)(PORT=1500))(CONNECT_DATA=(SERVICE_NAME=SBTJ2)));User Id=SBT_REP;Password=S8T_R3P;";
    }
}
