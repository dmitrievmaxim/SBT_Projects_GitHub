using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Oracle.DataAccess.Client;


namespace GetDataFromJIRAStructure
{
    class SQL_Worker
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public enum StructureTables
        {
            StructureTemp = 1,
            StructureRootTemp = 2,
            StructureRootForestTemp = 3, //содержит EDENTITY field
            StructureChildTemp = 4,
            StructureChildForestTemp = 5, //содержит EDENTITY field
            Structures = 6 //итоговая таблица
        }

        public static string _dropTable = @"BEGIN EXECUTE IMMEDIATE 'DROP TABLE {0}'; EXCEPTION WHEN OTHERS THEN IF SQLCODE != -942 THEN RAISE; END IF; END;";
        public static string _dropSequence = @"BEGIN EXECUTE IMMEDIATE 'DROP SEQUENCE {0}_s'; EXCEPTION WHEN OTHERS THEN IF SQLCODE != -2289 THEN RAISE; END IF; END;";
        public static string _dropTrigger = @"BEGIN EXECUTE IMMEDIATE 'DROP TRIGGER {0}_t'; EXCEPTION WHEN OTHERS THEN IF SQLCODE != -4080 THEN RAISE; END IF; END;";

        public static string _createSequence = @"BEGIN EXECUTE IMMEDIATE 'CREATE SEQUENCE {0}" + "_s'; EXCEPTION WHEN OTHERS THEN IF SQLCODE = -955 THEN NULL; ELSE RAISE; END IF; END;";
        public static string _createTrigger = @"BEGIN EXECUTE IMMEDIATE 'CREATE TRIGGER {0}_t BEFORE INSERT ON {0} FOR EACH ROW WHEN (new.id = 0) BEGIN SELECT {0}_s.NEXTVAL INTO :new.id FROM dual; END;'; EXCEPTION WHEN OTHERS THEN IF SQLCODE = -4081 THEN NULL; ELSE RAISE; END IF; END;";

        //Процедуры добавления только уникальных ID
        public static string _createProcedure_StructureRootTemp_Insert = @"BEGIN EXECUTE IMMEDIATE 'CREATE OR REPLACE PROCEDURE STRUCTUREROOTTEMP_INSERT (ID_VAL IN NUMBER) AS ID_VAL_COUNT NUMBER; BEGIN SELECT COUNT(*) INTO ID_VAL_COUNT FROM " + StructureTables.StructureRootTemp + " WHERE ID=ID_VAL; IF ID_VAL_COUNT = 0 THEN INSERT INTO " + StructureTables.StructureRootTemp + " (ID) VALUES (STRUCTUREROOTTEMP_INSERT.ID_VAL); END IF; END STRUCTUREROOTTEMP_INSERT;'; END;";
        public static string _createProcedure_StructureChildTemp_Insert = @"BEGIN EXECUTE IMMEDIATE 'CREATE OR REPLACE PROCEDURE STRUCTURECHILDTEMP_INSERT (ID_VAL IN NUMBER) AS ID_VAL_COUNT NUMBER; BEGIN SELECT COUNT(*) INTO ID_VAL_COUNT FROM " + StructureTables.StructureChildTemp + " WHERE ID=ID_VAL; IF ID_VAL_COUNT = 0 THEN INSERT INTO " + StructureTables.StructureChildTemp + "(ID) VALUES (STRUCTURECHILDTEMP_INSERT.ID_VAL); END IF; END STRUCTURECHILDTEMP_INSERT;'; END;";

        public static string _createTable_StructureTemp = @"BEGIN EXECUTE IMMEDIATE 'CREATE TABLE " + StructureTables.StructureTemp + "(ID number(10) NOT NULL, NAME varchar(200) NOT NULL, CONSTRAINT ID_struct_pk PRIMARY KEY (ID))'; EXCEPTION WHEN OTHERS THEN IF SQLCODE = -955 THEN NULL; ELSE RAISE; END IF; END;";
        public static string _createTable_StructureRootTemp = @"BEGIN EXECUTE IMMEDIATE 'CREATE TABLE " + StructureTables.StructureRootTemp + "(ID number(10) NOT NULL, CONSTRAINT ID_root_pk PRIMARY KEY (ID))'; EXCEPTION WHEN OTHERS THEN IF SQLCODE = -955 THEN NULL; ELSE RAISE; END IF; END;";
        public static string _createTable_StructureRootForestTemp = @"BEGIN EXECUTE IMMEDIATE 'CREATE TABLE " + StructureTables.StructureRootForestTemp + "(ID number(10) NOT NULL, ID_struct number(10) NOT NULL, ID_root number(10) NOT NULL, CONSTRAINT ID_root_forest_pk PRIMARY KEY (ID_struct, ID_root), CONSTRAINT fk_id_struct FOREIGN KEY (ID_struct) REFERENCES StructureTemp(ID), CONSTRAINT fk_id_root FOREIGN KEY (ID_root) REFERENCES StructureRootTemp(ID))'; EXCEPTION WHEN OTHERS THEN IF SQLCODE = -955 THEN NULL; ELSE RAISE; END IF; END;";
        public static string _createTable_StructureChildTemp = @"BEGIN EXECUTE IMMEDIATE 'CREATE TABLE " + StructureTables.StructureChildTemp + "(ID number(10) NOT NULL, CONSTRAINT ID_child_pk PRIMARY KEY (ID))'; EXCEPTION WHEN OTHERS THEN IF SQLCODE = -955 THEN NULL; ELSE RAISE; END IF; END;";
        public static string _createTable_StructureChildForestTemp = @"BEGIN EXECUTE IMMEDIATE 'CREATE TABLE " + StructureTables.StructureChildForestTemp + "(ID number(10) NOT NULL, ID_struct number(10) NOT NULL, ID_root number(10) NOT NULL, ID_child number(10) NOT NULL, CONSTRAINT ID_child_forest_pk PRIMARY KEY (ID_struct, ID_root, ID_child), CONSTRAINT fk_id_root_forest FOREIGN KEY (ID_struct, ID_root) REFERENCES StructureRootForestTemp(ID_struct, ID_root), CONSTRAINT fk_id_child FOREIGN KEY (ID_child) REFERENCES StructureChildTemp(ID))'; EXCEPTION WHEN OTHERS THEN IF SQLCODE = -955 THEN NULL; ELSE RAISE; END IF; END;";
        public static string _createTable_STRUCTURES = @"BEGIN EXECUTE IMMEDIATE 'CREATE TABLE " + StructureTables.Structures + "(ID NUMBER(10, 0), ID_STRUCT NUMBER(10, 0), NAME_STRUCT VARCHAR2(200 BYTE) NOT NULL, ID_ROOT NUMBER(10, 0), NAME_ROOT VARCHAR2(296 CHAR), ID_CHILD NUMBER, NAME_CHILD VARCHAR2(296 CHAR))'; EXCEPTION WHEN OTHERS THEN IF SQLCODE = -955 THEN NULL; ELSE RAISE; END IF; END;";

        //Создание результирующего view ALLSTRUCTURES (на проде)
        public static string _createView_ALLSTRUCTURES = @"BEGIN EXECUTE IMMEDIATE '
CREATE VIEW ALLSTRUCTURES AS
SELECT result.""ID"",result.""ID_STRUCT"",str.NAME NAME_STRUCT,result.""ID_ROOT"", iss_1.ISSUE NAME_ROOT, result.""ID_CHILD"", iss_2.ISSUE NAME_CHILD FROM (
--ROOT без детей
SELECT ID,ID_STRUCT, ID_ROOT, null ID_CHILD FROM STRUCTUREROOTFORESTTEMP s_1 WHERE NOT EXISTS (SELECT *  FROM STRUCTURECHILDFORESTTEMP s_2 WHERE s_1.ID_ROOT=s_2.ID_ROOT AND s_1.ID_STRUCT=s_2.ID_STRUCT)
UNION
--ROOT с детьми
SELECT ID, ID_STRUCT, ID_ROOT, null ID_CHILD FROM STRUCTUREROOTFORESTTEMP s_1 WHERE EXISTS (SELECT * FROM STRUCTURECHILDFORESTTEMP s_2 WHERE s_1.ID_ROOT=s_2.ID_ROOT AND s_1.ID_STRUCT=s_2.ID_STRUCT)
UNION
--Дети
SELECT s_child_forest.* FROM STRUCTURECHILDFORESTTEMP s_child_forest LEFT JOIN
(SELECT ID_STRUCT, ID_ROOT FROM STRUCTUREROOTFORESTTEMP s_1 WHERE EXISTS (SELECT * FROM STRUCTURECHILDFORESTTEMP s_2 WHERE s_1.ID_ROOT=s_2.ID_ROOT AND s_1.ID_STRUCT=s_2.ID_STRUCT)) tbl 
ON tbl.ID_STRUCT=s_child_forest.ID_STRUCT AND tbl.ID_ROOT=s_child_forest.ID_ROOT
) result INNER JOIN STRUCTURETEMP str ON result.ID_STRUCT=str.ID LEFT JOIN ISSUES iss_1 ON result.ID_ROOT=iss_1.ID LEFT JOIN ISSUES iss_2 ON result.ID_CHILD=iss_2.ID
ORDER BY ID_CHILD';  EXCEPTION WHEN OTHERS THEN IF SQLCODE = -955 THEN NULL; ELSE RAISE; END IF; END;";

        public static string _insert_StructureTemp = @"BEGIN EXECUTE IMMEDIATE 'INSERT INTO " + StructureTables.StructureTemp + "(ID, NAME) VALUES ({0},''{1}'')'; END;";
        public static string _insert_StructureRootTemp = @"BEGIN EXECUTE IMMEDIATE 'INSERT INTO " + StructureTables.StructureRootTemp + "(ID) VALUES ({0})'; END;";
        public static string _insert_StructureRootForestTemp = @"BEGIN EXECUTE IMMEDIATE 'INSERT INTO " + StructureTables.StructureRootForestTemp + "(ID, ID_struct, ID_root) VALUES ({0},{1},{2})'; END;";
        public static string _insert_StructureChildTemp = @"BEGIN EXECUTE IMMEDIATE 'INSERT INTO " + StructureTables.StructureChildTemp + "(ID) VALUES ({0})'; END;";
        public static string _insert_StructureChildForestTemp = @"BEGIN EXECUTE IMMEDIATE 'INSERT INTO " + StructureTables.StructureChildForestTemp + "(ID, ID_STRUCT, ID_root, ID_child) VALUES ({0},{1},{2},{3})'; END;";
        public static string _insert_STRUCTURES = @"BEGIN EXECUTE IMMEDIATE 'INSERT INTO " + StructureTables.Structures + @" SELECT * FROM ALLSTRUCTURES'; END;";

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
                throw new StructureException(ex.ToString());
            }
        }

        public static void ExecuteProcedure(string procName, int param)
        {
            try 
            {
                using (OracleConnection con = new OracleConnection(Constants._jiraProdConnectionString))
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
                throw new StructureException(ex.ToString());
            }
        }
    }
}
