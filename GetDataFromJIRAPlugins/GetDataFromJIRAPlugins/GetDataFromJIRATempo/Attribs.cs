using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel;

namespace GetDataFromJIRATempo
{
    class Attribs
    {
        [JsonConverter(typeof(JsonPathConverter))]
        public class JIRA_Tempo_Attribs
        {
            [DefaultValue("0")]
            public int ID_identity { get; set; }//IDENTITY
            [JsonProperty("id", Required = Required.Always)]
            public int ID_timesheet { get; set; }//Уникальный ID записи Timesheet   --id
            [JsonProperty("issue.id", Required = Required.Always)]
            public int ID_issue { get; set; }//ID запроса   --issue - id
            [JsonProperty("issue.key", Required = Required.Always)]
            public string Issue_name { get; set; }//Name запроса    --issue - key
            [JsonProperty("issue.summary", Required = Required.Always)]
            public string Summary { get; set; }//Тема запроса   --issue - summary
            [JsonProperty("timeSpentSeconds", Required = Required.Always)]
            public int Timespent { get; set; }//затраченное время в секундах --timeSpentSeconds/3600
            [JsonProperty("dateStarted", Required = Required.Always)]
            public DateTime Workdate { get; set; }//Дата внесения трудозатрат   --dateStarted
            [JsonProperty("author.displayName", Required = Required.Always)]
            public string Full_name { get; set; }//ФИО сотрудника   --author - displayName
            [JsonProperty("author.name", Required = Required.Always)]
            public string User_name { get; set; }//Login содрудника --author - name
            [JsonProperty("issue.issueType.name", Required = Required.Always)]
            public string Issue_type { get; set; }//Тип запроса --issue - issueType - name
            [JsonProperty("issue.projectId", Required = Required.Always)]
            public int ID_project { get; set; }//ID проекта --issue - projectId
            [JsonProperty("comment", Required = Required.AllowNull)]
            public string Description { get; set; }//Описание работы    --comment
            [JsonProperty("worklogAttributes", Required = Required.AllowNull)]
            public List<JIRA_mapping> Worklog_attribs { get; set; }//Массив кастомных атрибутов

            public string WorkType_val { get; set; }
            public string AS_num { get; set; }
            public string AS_val { get; set; }
        }

        public enum JIRA_customfield_attribs
        {
            _АС_ = 1,
            _Типработ_ = 2
        };

        public class JIRA_Project
        {
            public int ID_project { get; set; }
            public string Name_project { get; set; }
            public string Name_project_PKEY { get; set; }
        }

        public class JIRA_mapping
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }
    }
}
