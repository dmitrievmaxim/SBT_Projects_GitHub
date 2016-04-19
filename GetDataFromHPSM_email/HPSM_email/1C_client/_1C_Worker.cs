using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sql_w = _1C_client.SQL_Worker;
using prop = _1C_client.Properties;
using System.IO;
using log4net;
using ExcelWorker;

namespace _1C_client
{
    class _1C_Worker
    {
        public static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IList<Attribs._1C_Attribs> _listAllData1C = new List<Attribs._1C_Attribs>();

        public _1C_Worker()
        {
            try
            {
                Files_Worker f_worker = new Files_Worker();
                f_worker.Work();
                string path = prop.Client_1C_Settings.Default.pathToFiles;
                string file = prop.Client_1C_Settings.Default.prevFileName;
                log.Info("File for import: " + file);
                log.Info("File created: " + prop.Client_1C_Settings.Default.prevFileCreatedDate);
                
                DropCreateExecutes(); //Dropes and creates 1C table

                switch (Path.GetExtension(path + file))
                {
                    case ".xls":
                        {
                            ExcelWorker.Excel_Worker_ExcelLibrary exl_ = new ExcelWorker.Excel_Worker_ExcelLibrary(path + file);
                            exl_.ReadExcel(ref _listAllData1C);
                        }
                        break;
                    case ".xlsx": 
                        {
                            ExcelWorker.Excel_Worker_EPPLUS exl_ = new ExcelWorker.Excel_Worker_EPPLUS(path + file);
                            exl_.ReadExcel(ref _listAllData1C);
                        }
                        break;
                    default: break;
                }

                //*****************************//
                //Если на клиентах установлен Excel
                //ExcelWorker.ExcelWorker exl = new ExcelWorker.ExcelWorker(path + file);
                //exl.ReadExcel(ref _listAllData1C);
                //exl.ExcelStop();
                //*****************************//

                ExportData();
            }
            catch (Exception ex)
            {
                throw new _1CException(ex.ToString());
            }
        }

        private void DropCreateExecutes()
        {
            try
            {
                //Get all tables
                Stack<string> tbls = new Stack<string>();
                foreach (sql_w._1CTables str_table in Enum.GetValues(typeof(sql_w._1CTables)))
                {
                    tbls.Push(str_table.ToString());
                }

                //Drop
                foreach (string table in tbls)
                {
                    if ((table == sql_w._1CTables.Data_1C.ToString()))
                    {
                        sql_w.Execute(string.Format(sql_w._dropTrigger, table)); //Drop trigger
                        sql_w.Execute(string.Format(sql_w._dropSequence, table)); //Drop sequence
                    }
                    sql_w.Execute(string.Format(sql_w._dropTable, table)); //Drop table
                }

                //Create a 1C tables, sequences and triggers
                foreach (sql_w._1CTables str_table in Enum.GetValues(typeof(sql_w._1CTables)))
                {
                    switch (str_table)
                    {
                        case sql_w._1CTables.Data_1C:
                            sql_w.Execute(sql_w._createTable_1CData); break;
                        default: break;
                    }

                    if ((str_table == sql_w._1CTables.Data_1C))
                    {
                        sql_w.Execute(string.Format(sql_w._createSequence, str_table)); //Create sequence
                        sql_w.Execute(string.Format(sql_w._createTrigger, str_table)); //Create trigger
                    }
                }
            }
            catch (Exception ex)
            {
                throw new _1CException(ex.ToString());
            }
        }

        private void ExportData()
        {
            try
            {
                //Export in 1C
                foreach (var item in _listAllData1C)
                {
                    System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
                    customCulture.NumberFormat.NumberDecimalSeparator = ".";

                    System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

                    sql_w.Execute(string.Format(sql_w._insert_1CData, 0, item.ID_VACANCY = item.ID_VACANCY ?? "",  item.PERSONNEL_NUMBER = item.PERSONNEL_NUMBER ?? "",  item.FIO = item.FIO ?? "", item.LAST_NAME = item.LAST_NAME ?? "", item.FIRST_NAME = item.FIRST_NAME ?? "", item.SECOND_NAME = item.SECOND_NAME ?? "", item.SAP = item.SAP ?? "", item.SIGN = item.SIGN ?? "",  item.CUSTOMER = item.CUSTOMER ?? "", item.COUNT = item.COUNT ?? 0, item.BLOCK = item.BLOCK ?? "", item.CATEGORY = item.CATEGORY ?? "", item.OFFICE_SBT = item.OFFICE_SBT ?? "", item.DEPARTMENT_SBT = item.DEPARTMENT_SBT ?? "", item.GRADE = item.GRADE ?? 0, item.CHIEF = item.CHIEF ?? "", item.OFFICE_CORP = item.OFFICE_CORP ?? "", item.DEPARTMENT_CORP = item.DEPARTMENT_CORP ?? "", Program.currentDate));
                    
                }
                log.Info("Added " + _listAllData1C.Count + " rows");
            }
            catch (Exception ex)
            {
                throw new _1CException(ex.ToString());
            }
        }
    }
}
