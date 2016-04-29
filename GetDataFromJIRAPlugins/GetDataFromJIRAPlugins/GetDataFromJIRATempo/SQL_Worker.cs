using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Oracle.DataAccess.Client;
using GetDataFromJIRATempo;


namespace GetDataFromJIRAStructure
{
    class SQL_Worker
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public enum TempoTables
        {
            TempoLabor = 1,
            TempoLaborTmp = 2
        }

        public static string _dropTable = @"BEGIN EXECUTE IMMEDIATE 'DROP TABLE {0}'; EXCEPTION WHEN OTHERS THEN IF SQLCODE != -942 THEN RAISE; END IF; END;";
        public static string _dropSequence = @"BEGIN EXECUTE IMMEDIATE 'DROP SEQUENCE {0}_s'; EXCEPTION WHEN OTHERS THEN IF SQLCODE != -2289 THEN RAISE; END IF; END;";
        public static string _dropTrigger = @"BEGIN EXECUTE IMMEDIATE 'DROP TRIGGER {0}_t'; EXCEPTION WHEN OTHERS THEN IF SQLCODE != -4080 THEN RAISE; END IF; END;";

        public static string _createSequence = @"BEGIN EXECUTE IMMEDIATE 'CREATE SEQUENCE {0}" + "_s'; EXCEPTION WHEN OTHERS THEN IF SQLCODE = -955 THEN NULL; ELSE RAISE; END IF; END;";
        public static string _createTrigger = @"BEGIN EXECUTE IMMEDIATE 'CREATE TRIGGER {0}_t BEFORE INSERT ON {0} FOR EACH ROW WHEN (new.id = 0) BEGIN SELECT {0}_s.NEXTVAL INTO :new.id FROM dual; END;'; EXCEPTION WHEN OTHERS THEN IF SQLCODE = -4081 THEN NULL; ELSE RAISE; END IF; END;";

        public static string _createTable_TempoLabor = @"BEGIN EXECUTE IMMEDIATE 'CREATE TABLE " + TempoTables.TempoLabor + @"(ID NUMBER(10) NOT NULL, ID_timesheet NUMBER(10) NOT NULL, ID_issue NUMBER(10) NOT NULL, Issue_name varchar(30) NOT NULL, Summary varchar(1000) NOT NULL, Timespent FLOAT NOT NULL, Workdate DATE NOT NULL, Full_name varchar(200) NOT NULL, User_name varchar(100) NOT NULL, Issue_type varchar(100) NOT NULL, ID_project NUMBER(10) NOT NULL, Description varchar(4000) NULL, Work_type_val varchar(1000) NULL, As_num varchar(1000) NULL, As_val varchar(1000) NULL, Account_val varchar(1000) NULL, CONSTRAINT ID_timesheet_pk PRIMARY KEY(ID_timesheet))'; EXCEPTION WHEN OTHERS THEN IF SQLCODE = -955 THEN NULL; ELSE RAISE; END IF; END;";
        public static string _createTable_TempoLaborTmp = @"BEGIN EXECUTE IMMEDIATE 'CREATE TABLE " + TempoTables.TempoLaborTmp + @"(ID NUMBER(10) NOT NULL, ID_timesheet NUMBER(10) NOT NULL, ID_issue NUMBER(10) NOT NULL, Issue_name varchar(30) NOT NULL, Summary varchar(1000) NOT NULL, Timespent FLOAT NOT NULL, Workdate DATE NOT NULL, Full_name varchar(200) NOT NULL, User_name varchar(100) NOT NULL, Issue_type varchar(100) NOT NULL, ID_project NUMBER(10) NOT NULL, Description varchar(4000) NULL, Work_type_val varchar(1000) NULL, As_num varchar(1000) NULL, As_val varchar(1000) NULL, Account_val varchar(1000) NULL, CONSTRAINT ID_timesheet_tmp_pk PRIMARY KEY(ID_timesheet))'; EXCEPTION WHEN OTHERS THEN IF SQLCODE = -955 THEN NULL; ELSE RAISE; END IF; END;";


        public static string _insert_TempoLaborTmp = @"BEGIN EXECUTE IMMEDIATE 'INSERT INTO " + TempoTables.TempoLaborTmp + @" VALUES ({0},{1},{2},''{3}'',''{4}'',ROUND({5}/3600,2), TO_DATE(''{6}'', ''DD.MM.YYYY''),''{7}'',''{8}'',''{9}'',{10},''{11}'',''{12}'',''{13}'',''{14}'', ''{15}'')'; END;";

        public static string _update_TempoLabor = @"UPDATE (SELECT t1.ID_ISSUE item_1,
               t2.ID_ISSUE item_2,
               t1.ISSUE_NAME item_3,
               t2.ISSUE_NAME item_4,
               t1.SUMMARY item_5,
               t2.SUMMARY item_6,
               t1.TIMESPENT item_7,
               t2.TIMESPENT item_8,
               t1.WORKDATE item_9,
               t2.WORKDATE item_10,
               t1.FULL_NAME item_11,
               t2.FULL_NAME item_12,
               t1.USER_NAME item_13,
               t2.USER_NAME item_14,
               t1.ISSUE_TYPE item_15,
               t2.ISSUE_TYPE item_16,
               t1.ID_PROJECT item_17,
               t2.ID_PROJECT item_18,
               t1.DESCRIPTION item_19,
               t2.DESCRIPTION item_20,
               t1.WORK_TYPE_VAL item_21,
               t2.WORK_TYPE_VAL item_22,
               t1.AS_NUM item_23,
               t2.AS_NUM item_24,
               t1.AS_VAL item_25,
               t2.AS_VAL item_26,
               t1.ACCOUNT_VAL item_27,
               t2.ACCOUNT_VAL item_28
          FROM TEMPOLABOR t1,
               TEMPOLABORTMP t2
         WHERE t1.ID_TIMESHEET = t2.ID_TIMESHEET)
   SET item_1 = item_2, item_3 = item_4, item_5 = item_6, item_7 = item_8, item_9 = item_10, item_11 = item_12, item_13 = item_14, item_15 = item_16, item_17 = item_18, item_19 = item_20, item_21 = item_22, item_23 = item_24, item_25 = item_26, item_27 = item_28";

        public static string _insert_TempoLabor = @"INSERT INTO TEMPOLABOR SELECT 0, ID_TIMESHEET, ID_ISSUE, ISSUE_NAME, SUMMARY, TIMESPENT, WORKDATE, FULL_NAME, USER_NAME, ISSUE_TYPE, ID_PROJECT, DESCRIPTION, WORK_TYPE_VAL, AS_NUM, AS_VAL, ACCOUNT_VAL FROM TEMPOLABORTMP tmp WHERE NOT EXISTS (SELECT ID FROM TEMPOLABOR t WHERE t.ID_TIMESHEET = tmp.ID_TIMESHEET)";

        public static string _getProjects = @"SELECT ID, PNAME, PKEY FROM JIRA.project";
        
        public static void Execute(string sql)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(Constants._jiraTestConnectionString))
                {
                    con.Open();
                    using (OracleCommand cmd = new OracleCommand(sql, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw new TempoException(ex.ToString());
            }
        }

        public static void Execute(string sql, string connectionstring, ref IList<Attribs.JIRA_Project> projects)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(connectionstring))
                {
                    con.Open();
                    using (OracleCommand cmd = new OracleCommand(sql, con))
                    {
                        OracleDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                            projects.Add(new Attribs.JIRA_Project{ID_project = reader.GetInt32(0), Name_project = reader.GetString(1), Name_project_PKEY = reader.GetString(2)});
                        reader.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TempoException(ex.ToString());
            }
        }

        public static void ExecuteProcedure(string procName, int param)
        {
            try 
            {
                using (OracleConnection con = new OracleConnection(Constants._jiraTestConnectionString))
                {
                    using (OracleCommand cmd = new OracleCommand(procName, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("ID_VAL", "NUMBER").Value = param;
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw new TempoException(ex.ToString());
            }
        }
    }
}
