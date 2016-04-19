using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1C_client.Properties;

namespace _1C_client
{
    class Constants
    {
        //DB
        public static string _jiraProdConnectionString = @"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.99.114)(PORT=1500))(CONNECT_DATA=(SERVICE_NAME=SBTJ2)));User Id=SBT_REP;Password=S8T_R3P;";
        public static string _jiraTestConnectionString = Client_1C_Settings.Default.jiraTestConnectionString;
    }
}
