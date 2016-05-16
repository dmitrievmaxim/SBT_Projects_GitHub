using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;

namespace _1C_client
{
    class SQL_Worker
    {
        public enum _1CTables
        {
            Data_1C = 1
        }
        public static string _dropTable = @"BEGIN EXECUTE IMMEDIATE 'DROP TABLE {0}'; EXCEPTION WHEN OTHERS THEN IF SQLCODE != -942 THEN RAISE; END IF; END;";
        public static string _dropSequence = @"BEGIN EXECUTE IMMEDIATE 'DROP SEQUENCE {0}_s'; EXCEPTION WHEN OTHERS THEN IF SQLCODE != -2289 THEN RAISE; END IF; END;";
        public static string _dropTrigger = @"BEGIN EXECUTE IMMEDIATE 'DROP TRIGGER {0}_t'; EXCEPTION WHEN OTHERS THEN IF SQLCODE != -4080 THEN RAISE; END IF; END;";

        public static string _createSequence = @"BEGIN EXECUTE IMMEDIATE 'CREATE SEQUENCE {0}" + "_s'; EXCEPTION WHEN OTHERS THEN IF SQLCODE = -955 THEN NULL; ELSE RAISE; END IF; END;";
        public static string _createTrigger = @"BEGIN EXECUTE IMMEDIATE 'CREATE TRIGGER {0}_t BEFORE INSERT ON {0} FOR EACH ROW WHEN (new.id = 0) BEGIN SELECT {0}_s.NEXTVAL INTO :new.id FROM dual; END;'; EXCEPTION WHEN OTHERS THEN IF SQLCODE = -4081 THEN NULL; ELSE RAISE; END IF; END;";

        public static string _createTable_1CData = @"BEGIN EXECUTE IMMEDIATE 'CREATE TABLE " + _1CTables.Data_1C + @"(ID NUMBER(10) NOT NULL, ID_VACANCY VARCHAR(200) NULL, PERSONNEL_NUMBER VARCHAR(15) NULL, FIO VARCHAR(200) NULL, LAST_NAME VARCHAR(100) NULL, FIRST_NAME VARCHAR(100) NULL, SECOND_NAME VARCHAR(100) NULL, SAP VARCHAR(10) NULL, ""SIGN"" VARCHAR(200) NULL, CUSTOMER VARCHAR(1000) NULL, ""COUNT"" NUMBER NULL, BLOCK VARCHAR(1000) NULL, CATEGORY VARCHAR(10) NULL, OFFICE_SBT VARCHAR(1000) NULL, DEPARTMENT_SBT VARCHAR(1000) NULL, GRADE INTEGER NULL, CHIEF VARCHAR(200) NULL, OFFICE_CORP VARCHAR(1000) NULL, DEPARTMENT_CORP VARCHAR(1000) NULL, ORG_POSITION VARCHAR(1000) NULL, EMAIL VARCHAR(100) NULL, CORP_POSITION VARCHAR(1000) NULL, FIO_PREV VARCHAR(200) NULL, AddedDate DATE NOT NULL, CONSTRAINT ID_1C_pk PRIMARY KEY(ID))'; EXCEPTION WHEN OTHERS THEN IF SQLCODE = -955 THEN NULL; ELSE RAISE; END IF; END;";

        public static string _insert_1CData = @"BEGIN EXECUTE IMMEDIATE 'INSERT INTO " + _1CTables.Data_1C + @" VALUES ({0},''{1}'', ''{2}'', ''{3}'', ''{4}'', ''{5}'', ''{6}'', ''{7}'', ''{8}'', ''{9}'', {10}, ''{11}'', ''{12}'', ''{13}'', ''{14}'', {15}, ''{16}'', ''{17}'', ''{18}'', ''{19}'', ''{20}'', ''{21}'', ''{22}'', TO_DATE(''{23}'',''DD.MM.YYYY''))'; END;";

        public static void Execute(string sql)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(Constants._jiraProdConnectionString))
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
                throw new _1CException(ex.ToString());
            }
        }
    }
}
