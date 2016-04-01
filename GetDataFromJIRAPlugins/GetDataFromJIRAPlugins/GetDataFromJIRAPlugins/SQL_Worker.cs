using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Oracle.DataAccess.Client;


namespace GetDataFromJIRAPlugins
{
    class SQL_Worker
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Structure tmp tables
        //public enum StructureTables
        //{
        //    StructureRootTemp = 1,
        //    StructureRootForestTemp = 2,
        //    StructureChildForestTemp = 3
        //}

        public enum StructureTables
        {
            StructureTemp = 1,
            StructureRootTemp = 2,
            StructureRootForestTemp = 3, //содержит EDENTITY field
            StructureChildTemp = 4,
            StructureChildForestTemp = 5 ////содержит EDENTITY field
        }

        public static string _dropTable = @"BEGIN EXECUTE IMMEDIATE 'DROP TABLE {0}'; EXCEPTION WHEN OTHERS THEN IF SQLCODE != -942 THEN RAISE; END IF; END;";
        public static string _dropSequence = @"BEGIN EXECUTE IMMEDIATE 'DROP SEQUENCE {0}_s'; EXCEPTION WHEN OTHERS THEN IF SQLCODE != -2289 THEN RAISE; END IF; END;";
        public static string _dropTrigger = @"BEGIN EXECUTE IMMEDIATE 'DROP TRIGGER {0}_t'; EXCEPTION WHEN OTHERS THEN IF SQLCODE != -4080 THEN RAISE; END IF; END;";

        public static string _createSequence = @"BEGIN EXECUTE IMMEDIATE 'CREATE SEQUENCE {0}" + "_s'; END;";
        public static string _createTrigger = @"BEGIN EXECUTE IMMEDIATE 'CREATE OR REPLACE TRIGGER {0}_t BEFORE INSERT ON {0} FOR EACH ROW WHEN (new.id = 0) BEGIN SELECT {0}_s.NEXTVAL INTO :new.id FROM dual; END;'; END;";

        //Процедуры добавления только уникальных ID
        public static string _createProcedure_StructureRootTemp = @"BEGIN EXECUTE IMMEDIATE 'CREATE OR REPLACE PROCEDURE STRUCTUREROOTTEMP_INSERT (ID_VAL IN NUMBER) AS ID_VAL_COUNT NUMBER; BEGIN SELECT COUNT(*) INTO ID_VAL_COUNT FROM " + StructureTables.StructureRootTemp + " WHERE ID=ID_VAL; IF ID_VAL_COUNT = 0 THEN INSERT INTO " + StructureTables.StructureRootTemp + " (ID) VALUES (STRUCTUREROOTTEMP_INSERT.ID_VAL); END IF; END STRUCTUREROOTTEMP_INSERT'; END;";
        public static string _createProcedure_StructureChildTemp = @"BEGIN EXECUTE IMMEDIATE 'CREATE OR REPLACE PROCEDURE STRUCTURECHILDTEMP_INSERT (ID_VAL IN NUMBER) AS ID_VAL_COUNT NUMBER; BEGIN SELECT COUNT(*) INTO ID_VAL_COUNT FROM " + StructureTables.StructureChildTemp + " WHERE ID=ID_VAL; IF ID_VAL_COUNT = 0 THEN INSERT INTO " + StructureTables.StructureChildTemp + "(ID) VALUES (STRUCTURECHILDTEMP_INSERT.ID_VAL); END IF; END STRUCTURECHILDTEMP_INSERT;'; END;";

        public static string _createTable_StructureTemp = @"BEGIN EXECUTE IMMEDIATE 'CREATE TABLE " + StructureTables.StructureTemp + "(ID number(10) NOT NULL, NAME varchar(200) NOT NULL, CONSTRAINT ID_struct_pk PRIMARY KEY (ID))'; END;";
        public static string _createTable_StructureRootTemp = @"BEGIN EXECUTE IMMEDIATE 'CREATE TABLE " + StructureTables.StructureRootTemp + "(ID number(10) NOT NULL, CONSTRAINT ID_root_pk PRIMARY KEY (ID))'; END;";
        public static string _createTable_StructureRootForestTemp = @"BEGIN EXECUTE IMMEDIATE 'CREATE TABLE " + StructureTables.StructureRootForestTemp + "(ID number(10) NOT NULL, ID_struct number(10) NOT NULL, ID_root number(10) NOT NULL, CONSTRAINT ID_root_forest_pk PRIMARY KEY (ID_struct, ID_root), CONSTRAINT fk_id_struct FOREIGN KEY (ID_struct) REFERENCES StructureTemp(ID), CONSTRAINT fk_id_root FOREIGN KEY (ID_root) REFERENCES StructureRootTemp(ID))'; END;";
        public static string _createTable_StructureChildTemp = @"BEGIN EXECUTE IMMEDIATE 'CREATE TABLE " + StructureTables.StructureChildTemp + "(ID number(10) NOT NULL, CONSTRAINT ID_child_pk PRIMARY KEY (ID))'; END;";
        public static string _createTable_StructureChildForestTemp = @"BEGIN EXECUTE IMMEDIATE 'CREATE TABLE " + StructureTables.StructureChildForestTemp + "(ID number(10) NOT NULL, ID_struct number(10) NOT NULL, ID_root number(10) NOT NULL, ID_child number(10) NOT NULL, CONSTRAINT ID_child_forest_pk PRIMARY KEY (ID_struct, ID_root, ID_child), CONSTRAINT fk_id_root_forest FOREIGN KEY (ID_struct, ID_root) REFERENCES StructureRootForestTemp(ID_struct, ID_root), CONSTRAINT fk_id_child FOREIGN KEY (ID_child) REFERENCES StructureChildTemp(ID))'; END;";
        
        public static string _insert_StructureTemp = @"BEGIN EXECUTE IMMEDIATE 'INSERT INTO " + StructureTables.StructureTemp + "(ID, NAME) VALUES ({0},''{1}'')'; END;";
        public static string _insert_StructureRootTemp = @"BEGIN EXECUTE IMMEDIATE 'INSERT INTO " + StructureTables.StructureRootTemp + "(ID) VALUES ({0})'; END;";
        public static string _insert_StructureRootForestTemp = @"BEGIN EXECUTE IMMEDIATE 'INSERT INTO " + StructureTables.StructureRootForestTemp + "(ID, ID_struct, ID_root) VALUES ({0},{1},{2})'; END;";
        public static string _insert_StructureChildTemp = @"BEGIN EXECUTE IMMEDIATE 'INSERT INTO " + StructureTables.StructureChildTemp + "(ID) VALUES ({0})'; END;";
        public static string _insert_StructureChildForestTemp = @"BEGIN EXECUTE IMMEDIATE 'INSERT INTO " + StructureTables.StructureChildForestTemp + "(ID, ID_STRUCT, ID_root, ID_child) VALUES ({0},{1},{2},{3})'; END;";

                
        /*
        public static string _createTable_StructureRootTemp = @"BEGIN EXECUTE IMMEDIATE 'CREATE TABLE " + StructureTables.StructureRootTemp + "(ID number(10) NOT NULL, ID_root number(10) NOT NULL, NAME_root varchar(200) NOT NULL, CONSTRAINT ID_root_pk PRIMARY KEY (ID_root))'; END;";
        public static string _createTable_StructureRootForestTemp = @"BEGIN EXECUTE IMMEDIATE 'CREATE TABLE " + StructureTables.StructureRootForestTemp + "(ID number(10) NOT NULL, ID_root number(10) NOT NULL, VALUE_root_forest number(10) NOT NULL)'; END;";
        public static string _createTable_StructureChildForestTemp = @"BEGIN EXECUTE IMMEDIATE 'CREATE TABLE " + StructureTables.StructureChildForestTemp + "(ID number(10) NOT NULL, ID_root_forest number(10) NOT NULL, VALUE_child_forest number(10) NOT NULL)'; END;";
        */

        /*
        public static string _insert_StructureRootTemp = @"BEGIN EXECUTE IMMEDIATE 'INSERT INTO " + StructureTables.StructureRootTemp + "(ID_root, NAME_root) VALUES ({0},''{1}'')'; END;";
        public static string _insert_StructureRootForestTemp = @"BEGIN EXECUTE IMMEDIATE 'INSERT INTO " + StructureTables.StructureRootForestTemp + "(ID_root, VALUE_root_forest) VALUES ({0},{1})'; END;";
        public static string _insert_StructureChildForestTemp = @"BEGIN EXECUTE IMMEDIATE 'INSERT INTO " + StructureTables.StructureChildForestTemp + "(ID_root_forest, VALUE_child_forest) VALUES ({0},{1})'; END;";
        */
        
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
                log.Error(ex.ToString());
                Console.WriteLine(ex.ToString()); OracleConnection.ClearAllPools();
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
                log.Error(ex.ToString());
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
