using System;
using System.Collections.Generic;
using System.ComponentModel; //Скачать из Nuget
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using HPSM_client;
using ExcelLib = ExcelLibrary.SpreadSheet;
using OfficeOpenXml;
using System.IO;

namespace ExcelWorker
{
    class ExcelWorker
    {
        private static Excel.Workbook MyBook = null;
        private static Excel.Application MyApp = null;
        private static Excel.Worksheet MySheet = null;

        private int lastRow { get; set; }

        public ExcelWorker(string path)
        {
            try
            {
                MyApp = new Excel.Application();
                MyApp.Visible = false;
                MyBook = MyApp.Workbooks.Open(path);
                MySheet = (Excel.Worksheet)MyBook.Sheets[1]; // Explicit cast is not required here
                lastRow = MySheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
            } 
            catch (Exception ex)
            {
                throw new HPSMException(ex.ToString());
            }
        }
        
        //Reading From Excel
        public void ReadExcel(ref IList<Attribs.HPSM_Attribs> HPSM_RowList)
        {
            try
            {
                for (int index = 10; index <= lastRow; index++)
                {
                    System.Array MyValues = (System.Array)MySheet.get_Range("A" +
                       index.ToString(), "E" + index.ToString()).Cells.Value;

                    if ((MyValues.GetValue(1, 1) == "") || (MyValues.GetValue(1, 1) == null))
                        continue;
                    HPSM_RowList.Add(new Attribs.HPSM_Attribs
                    {
                        I_NUMBER = MyValues.GetValue(1, 1).ToString(),
                        DT = Convert.ToDateTime(string.Format("{0:dd/MM/yy}", MyValues.GetValue(1, 2).ToString())),
                        TIME_SPEND = int.Parse(MyValues.GetValue(1, 3).ToString()),
                        FIO = MyValues.GetValue(1, 4)==null?"":MyValues.GetValue(1, 4).ToString(),
                        KE = MyValues.GetValue(1, 5)==null?"":MyValues.GetValue(1, 5).ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                throw new HPSMException(ex.ToString());
            }
        }

        public void ExcelStop()
        {
            MyBook.Close();
            MyApp.Quit();
        }
    }


    //КЛАСС ДЛЯ РАБОТЫ С ФОРМАТОМ XLS (ExcelLibrary)
    class Excel_Worker_ExcelLibrary
    {
        public string FilePath { get; set; }

        public Excel_Worker_ExcelLibrary(string path)
        {
            try
            {
                FilePath = path;
            } 
            catch (Exception ex)
            {
                throw new HPSMException(ex.ToString());
            }
        }
        
        //Reading From Excel
        public void ReadExcel(ref IList<Attribs.HPSM_Attribs> HPSM_RowList)
        {
            try
            {
                FileStream str = new FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite);

                ExcelLib.Workbook workbook = ExcelLib.Workbook.Load(str);
                ExcelLib.Worksheet worksheet = workbook.Worksheets[0];
                int lastRow = worksheet.Cells.LastRowIndex;
                
                for (int index = 9; index <= lastRow; index++)
                {
                    
                    ExcelLib.Row row = worksheet.Cells.GetRow(index);
                    if ((row.GetCell(0).Value == "") || (row.GetCell(0).Value == null))
                        continue;

                    HPSM_RowList.Add(new Attribs.HPSM_Attribs
                    {
                        I_NUMBER = row.GetCell(0).Value.ToString(),
                        DT = Convert.ToDateTime(string.Format("{0:dd/MM/yy}", row.GetCell(1).ToString())),
                        TIME_SPEND = row.GetCell(2).Value == null ? 0 : int.Parse(row.GetCell(2).ToString()),
                        FIO = row.GetCell(3).Value == null ? "" : row.GetCell(3).ToString(),
                        KE = row.GetCell(4).Value == null ? "" : row.GetCell(4).ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                throw new HPSMException(ex.ToString());
            }
        }
    }

    //КЛАСС ДЛЯ РАБОТЫ С ФОРМАТОМ XLSX (EPPlus.dll)
    class Excel_Worker_EPPLUS
    {
        public string FilePath { get; set; }

        public Excel_Worker_EPPLUS(string path)
        {
            try
            {
                FilePath = path;
            } 
            catch (Exception ex)
            {
                throw new HPSMException(ex.ToString());
            }
        }
        
        //Reading From Excel
        public void ReadExcel(ref IList<Attribs.HPSM_Attribs> HPSM_RowList)
        {
            using (ExcelPackage p = new ExcelPackage())
            {
                using (var stream = new FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite))
                {
                    p.Load(stream);

                    ExcelWorksheet ws = p.Workbook.Worksheets[1];
                    int lRow = ws.Dimension.End.Row;

                    for (int index = 10; index <= lRow; index++)
                    {
                        if ((ws.Cells[index, 1].Value == "") || (ws.Cells[index, 1].Value == null))
                            continue;

                        Console.WriteLine(ws.Cells[index, 1].Value.ToString());

                        HPSM_RowList.Add(new Attribs.HPSM_Attribs
                        {
                            I_NUMBER = ws.Cells[index, 1].Value.ToString(),
                            DT = Convert.ToDateTime(string.Format("{0:dd/MM/yy}", ws.Cells[index, 2].Value.ToString())),
                            TIME_SPEND = ws.Cells[index, 3].Value == null ? 0 : int.Parse(ws.Cells[index, 3].Value.ToString()),
                            FIO = ws.Cells[index, 4].Value == null ? "" : ws.Cells[index, 4].Value.ToString(),
                            KE = ws.Cells[index, 5].Value == null ? "" : ws.Cells[index, 5].Value.ToString()
                        });
                    }
                }
            }
        }
    }





}
