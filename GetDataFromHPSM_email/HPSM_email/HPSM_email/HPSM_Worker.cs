using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sql_w = HPSM_email.SQL_Worker;
using prop = HPSM_email.Properties;
using System.IO;
using log4net;
using HPSM_email.Properties;

namespace HPSM_email
{
    class HPSM_Worker
    {
        private IList<Attribs.HPSM_Attribs> _listAllTimesheetsHPSM = new List<Attribs.HPSM_Attribs>();

        public HPSM_Worker()
        {
            try
            {
                Files_Worker f_worker = new Files_Worker();
                f_worker.Work();
                string path = HPSM_Settings.Default.pathToFiles;
                string file = HPSM_Settings.Default.prevFileName;
                
                CreateExecutes(); //Creates hpsm table
                ExcelWorker.ExcelWorker exl = new ExcelWorker.ExcelWorker(path + file);
                exl.ReadExcel(ref _listAllTimesheetsHPSM);
                exl.ExcelStop();
                ExportData();
            }
            catch (Exception ex)
            {
                throw new HPSMException(ex.ToString());
            }
        }

        private void CreateExecutes()
        {
            try
            {
                //Clean a HPSM tables
                Stack<string> tbls = new Stack<string>();
                foreach (sql_w.HPSMTables str_table in Enum.GetValues(typeof(sql_w.HPSMTables)))
                {
                    tbls.Push(str_table.ToString());
                }

                //Снять коммент, если потребуется удалять сущности перед созданием в БД
                /*foreach (string table in tbls)
                {
                    if ((table == sql_w.HPSMTables.HPSMLabor.ToString()))
                    {
                        sql_w.Execute(string.Format(sql_w._dropTrigger, table)); //Drop trigger
                        sql_w.Execute(string.Format(sql_w._dropSequence, table)); //Drop sequence
                    }
                    sql_w.Execute(string.Format(sql_w._dropTable, table)); //Drop table
                }*/

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
                throw new HPSMException(ex.ToString());
            }
        }

        private void ExportData()
        {
            try
            {
                //Export in HPSM
                foreach (var item in _listAllTimesheetsHPSM)
                {
                    sql_w.Execute(string.Format(sql_w._insert_HPSMLabor, 0, item.I_NUMBER, item.DT.ToString("d"), item.TIME_SPEND, item.FIO, item.KE = item.KE ?? "", Program.currentDate));
                    
                }
            }
            catch (Exception ex)
            {
                throw new HPSMException(ex.ToString());
            }
        }
    }
}
