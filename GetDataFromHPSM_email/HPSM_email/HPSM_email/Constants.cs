using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HPSM_email.Properties;

namespace HPSM_email
{
    class Constants
    {
        //DB
        public static string _jiraProdConnectionString = @"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.99.114)(PORT=1500))(CONNECT_DATA=(SERVICE_NAME=SBTJ2)));User Id=SBT_REP;Password=S8T_R3P;";
        public static string _jiraTestConnectionString = HPSM_Settings.Default.jiraTestConnectionString;
    }
}
