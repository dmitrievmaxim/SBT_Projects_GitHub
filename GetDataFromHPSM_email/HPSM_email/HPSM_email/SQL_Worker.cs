using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;

namespace HPSM_email
{
    class SQL_Worker
    {
        public enum HPSMTables
        {
            HPSMLabor = 1
        }
        public static string _dropTable = @"BEGIN EXECUTE IMMEDIATE 'DROP TABLE {0}'; EXCEPTION WHEN OTHERS THEN IF SQLCODE != -942 THEN RAISE; END IF; END;";
        public static string _dropSequence = @"BEGIN EXECUTE IMMEDIATE 'DROP SEQUENCE {0}_s'; EXCEPTION WHEN OTHERS THEN IF SQLCODE != -2289 THEN RAISE; END IF; END;";
        public static string _dropTrigger = @"BEGIN EXECUTE IMMEDIATE 'DROP TRIGGER {0}_t'; EXCEPTION WHEN OTHERS THEN IF SQLCODE != -4080 THEN RAISE; END IF; END;";

        public static string _createSequence = @"BEGIN EXECUTE IMMEDIATE 'CREATE SEQUENCE {0}" + "_s'; EXCEPTION WHEN OTHERS THEN IF SQLCODE = -955 THEN NULL; ELSE RAISE; END IF; END;";
        public static string _createTrigger = @"BEGIN EXECUTE IMMEDIATE 'CREATE TRIGGER {0}_t BEFORE INSERT ON {0} FOR EACH ROW WHEN (new.id = 0) BEGIN SELECT {0}_s.NEXTVAL INTO :new.id FROM dual; END;'; EXCEPTION WHEN OTHERS THEN IF SQLCODE = -4081 THEN NULL; ELSE RAISE; END IF; END;";

        //public static string _createTable_HPSMLabor = @"BEGIN EXECUTE IMMEDIATE 'CREATE TABLE " + HPSMTables.HPSMLabor + @"(ID NUMBER(10) NOT NULL, I_NUMBER VARCHAR(50) NOT NULL, DT DATE NOT NULL, TIME_SPEND FLOAT NOT NULL, FIO VARCHAR(200) NOT NULL, KE VARCHAR(500) NULL, CONSTRAINT I_NUMBER_pk PRIMARY KEY(I_NUMBER))'; END;";

        public static string _createTable_HPSMLabor = @"BEGIN EXECUTE IMMEDIATE 'CREATE TABLE " + HPSMTables.HPSMLabor + @"(ID NUMBER(10) NOT NULL, I_NUMBER VARCHAR(50) NOT NULL, DT DATE NOT NULL, TIME_SPEND FLOAT NOT NULL, FIO VARCHAR(200) NOT NULL, KE VARCHAR(500) NULL, CONSTRAINT I_NUMBER_pk PRIMARY KEY(I_NUMBER))'; EXCEPTION WHEN OTHERS THEN IF SQLCODE = -955 THEN NULL; ELSE RAISE; END IF; END;";


        public static string _insert_HPSMLabor = @"BEGIN EXECUTE IMMEDIATE 'INSERT INTO " + HPSMTables.HPSMLabor + @" VALUES ({0},''{1}'', TO_DATE(''{2}'', ''DD.MM.YYYY''), ROUND({3}/60, 2),''{4}'', ''{5}'')'; END;";

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
                throw new HPSMException(ex.ToString());
            }
        }

    }
}
