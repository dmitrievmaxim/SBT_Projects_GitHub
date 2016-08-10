using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TryJira
{
    //http://www.codeproject.com/Tips/828384/Creating-an-Issue-with-Multiple-Attachments-in-Jir
    class Program
    {
        static void Main(string[] args)
        {
            Jira objJira = new Jira();
            /*objJira.Url = "http://192.168.99.118/jira/";
            objJira.JsonString = 
        @"{""fields"": {
       ""project"":
       { 
           ""key"": ""ASBPS""
       },
       ""summary"": ""REST EXAMPLE"",
       ""description"": ""Описание"",
       ""issuetype"": {
           ""name"": ""Дефект""
       }
   }
}";*/


            objJira.Url = "https://osok-atlasminsk.sbertech.by/jira/";

            //required fields
            objJira.JsonString = @"{""fields"": {

				""project"": {
                    ""id"": ""10000""
                },
                ""summary"": ""Issue created from REST"",
                ""issuetype"": {
                    ""id"": ""10001""
                },
				""customfield_10005"":""Epic name"",
				

				""customfield_10800"":""2016-07-01"",
				""customfield_10015"":{
					""id"":""10008""
				},
				""customfield_10016"":""12345"",
				""customfield_10200"":""Винни-Пух"",
				""customfield_10300"":{
                    ""name"":""Barkovskaya_O""
                },
                ""description"":""Какое-то описание"",
				""duedate"":""2016-07-30""
			}
        }";



            objJira.UserName="Dmitriev_M";
            objJira.Password="d22912m";
            objJira.filePaths = new List<string>() { @"C:\Users\SBT-Dmitriev-MV\DEV\tasks\Список АС\Список АС.xls", @"C:\Users\SBT-Dmitriev-MV\DEV\tasks\Список АС\Ресстр 31.05 с укороченными названиями.xlsx" };
            objJira.AddJiraIssue();
        }
    }
}
