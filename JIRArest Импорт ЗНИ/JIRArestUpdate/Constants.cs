using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIRArestUpdate
{
    class Constants
    {
        public class Parameters
        {
            //ID полей в JIRA
            public const string IdZniField = "customfield_10016";
            public const string DateZniField = "customfield_10800";

            //Путь к исходным Excel файлам
            public const string Container = @"C:\Users\SBT-Dmitriev-MV\DEV\JIRArest\Data\";

            //Учетные данные
            public const string JiraUrl = @"http://192.168.99.110/jira/rest/api/2/";
            public const string AccountId = @"Dmitriev_M";
            public const string Password = @"12dmv34*";
        }

        class JQLScaffold
        {

        }
    }
}
