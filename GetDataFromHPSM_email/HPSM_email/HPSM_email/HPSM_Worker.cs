using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sql_w = HPSM_email.SQL_Worker;

namespace HPSM_email
{
    class HPSM_Worker
    {
        private IList<Attribs.HPSM_Attribs> _listAllTimesheetsHPSM = new List<Attribs.HPSM_Attribs>();

        public HPSM_Worker()
        {
            string path = @"C:\Users\SBT-Dmitriev-MV\DEV\GetDataFromHPSM_email\hpsm_data.XLS";

            DropCreateExecutes(); //Drop and creates tempo table
            ExcelWorker.ExcelWorker exl = new ExcelWorker.ExcelWorker(path);
            exl.ReadExcel(ref _listAllTimesheetsHPSM);
            exl.ExcelStop();
            ExportData();
        }

        private void DropCreateExecutes()
        {
            try
            {
                //Clean a HPSM tables
                Stack<string> tbls = new Stack<string>();
                foreach (sql_w.HPSMTables str_table in Enum.GetValues(typeof(sql_w.HPSMTables)))
                {
                    tbls.Push(str_table.ToString());
                }

                foreach (string table in tbls)
                {
                    if ((table == sql_w.HPSMTables.HPSMLabor.ToString()))
                    {
                        sql_w.Execute(string.Format(sql_w._dropTrigger, table)); //Drop trigger
                        sql_w.Execute(string.Format(sql_w._dropSequence, table)); //Drop sequence
                    }
                    sql_w.Execute(string.Format(sql_w._dropTable, table)); //Drop table
                }

                //Create a HPSM tables, sequences and triggers
                foreach (sql_w.HPSMTables str_table in Enum.GetValues(typeof(sql_w.HPSMTables)))
                {
                    switch (str_table)
                    {
                        case sql_w.HPSMTables.HPSMLabor:
                            sql_w.Execute(sql_w._createTable_HPSMLabor); break;
                        default: break;
                    }

                    if ((str_table == sql_w.HPSMTables.HPSMLabor))
                    {
                        sql_w.Execute(string.Format(sql_w._createSequence, str_table)); //Create sequence
                        sql_w.Execute(string.Format(sql_w._createTrigger, str_table)); //Create trigger
                    }
                }
            }
            catch (Exception ex)
            {
                //log.Error(ex.ToString());
                Console.WriteLine(ex.ToString());
                Debug.WriteLine(ex.ToString());
            }
        }

        private void ExportData()
        {
            /*try
            {
                //Export HPSM
                foreach (var item in _listAllTimesheetsHPSM)
                {
                    sql_w.Execute(string.Format(sql_w._insert_TempoLabor, item.ID_identity, item.ID_timesheet, item.ID_issue, (item.Issue_name = item.Issue_name ?? "").Replace("'", "''''"), (item.Summary = item.Summary ?? "").Replace("'", "''''"), item.Timespent, item.Workdate.ToString("d"), item.Full_name, item.User_name, item.Issue_type, item.ID_project, (item.Description = item.Description ?? "").Replace("'", "''''"), (item.WorkType_val = item.WorkType_val ?? "").Replace("'", "''''"), (item.AS_num = item.AS_num ?? "").Replace("'", "''''"), (item.AS_val = item.AS_val ?? "").Replace("'", "''''")));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                Console.WriteLine(ex.ToString());
                Debug.WriteLine(ex.ToString());
            }*/
        }
    }
}
