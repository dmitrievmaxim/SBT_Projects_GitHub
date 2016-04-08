using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Oracle.DataAccess.Client;
using GetDataFromJIRATempo;


namespace GetDataFromJIRAPlugins
{
    class SQL_Worker
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public enum TempoTables
        {
            TempoLabor = 1,
        }

        public static string _dropTable = @"BEGIN EXECUTE IMMEDIATE 'DROP TABLE {0}'; EXCEPTION WHEN OTHERS THEN IF SQLCODE != -942 THEN RAISE; END IF; END;";
        public static string _dropSequence = @"BEGIN EXECUTE IMMEDIATE 'DROP SEQUENCE {0}_s'; EXCEPTION WHEN OTHERS THEN IF SQLCODE != -2289 THEN RAISE; END IF; END;";
        public static string _dropTrigger = @"BEGIN EXECUTE IMMEDIATE 'DROP TRIGGER {0}_t'; EXCEPTION WHEN OTHERS THEN IF SQLCODE != -4080 THEN RAISE; END IF; END;";

        public static string _createSequence = @"BEGIN EXECUTE IMMEDIATE 'CREATE SEQUENCE {0}" + "_s'; EXCEPTION WHEN OTHERS THEN IF SQLCODE = -955 THEN NULL; ELSE RAISE; END IF; END;";
        public static string _createTrigger = @"BEGIN EXECUTE IMMEDIATE 'CREATE TRIGGER {0}_t BEFORE INSERT ON {0} FOR EACH ROW WHEN (new.id = 0) BEGIN SELECT {0}_s.NEXTVAL INTO :new.id FROM dual; END;'; EXCEPTION WHEN OTHERS THEN IF SQLCODE = -4081 THEN NULL; ELSE RAISE; END IF; END;";

        public static string _createTable_TempoLabor = @"BEGIN EXECUTE IMMEDIATE 'CREATE TABLE " + TempoTables.TempoLabor + @"(ID NUMBER(10) NOT NULL, ID_timesheet NUMBER(10) NOT NULL, ID_issue NUMBER(10) NOT NULL, Issue_name varchar(30) NOT NULL, Summary varchar(1000) NOT NULL, Timespent FLOAT NOT NULL, Workdate DATE NOT NULL, Full_name varchar(200) NOT NULL, User_name varchar(100) NOT NULL, Issue_type varchar(100) NOT NULL, ID_project NUMBER(10) NOT NULL, Description varchar(4000) NULL, Work_type_val varchar(1000) NULL, As_num varchar(1000) NULL, As_val varchar(1000) NULL, CONSTRAINT ID_timesheet_pk PRIMARY KEY(ID_timesheet))'; EXCEPTION WHEN OTHERS THEN IF SQLCODE = -955 THEN NULL; ELSE RAISE; END IF; END;";

        public static string _insert_TempoLabor = @"BEGIN EXECUTE IMMEDIATE 'INSERT INTO " + TempoTables.TempoLabor + @" VALUES ({0},{1},{2},''{3}'',''{4}'',ROUND({5}/3600,2), TO_DATE(''{6}'', ''DD.MM.YYYY''),''{7}'',''{8}'',''{9}'',{10},''{11}'',''{12}'',''{13}'',''{14}'')'; END;";

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
