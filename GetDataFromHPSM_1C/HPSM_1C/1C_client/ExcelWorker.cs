using System;
using System.Collections.Generic;
using System.ComponentModel; //Скачать из Nuget
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel_interop = Microsoft.Office.Interop.Excel;
using _1C_client;
using ExcelLib = ExcelLibrary.SpreadSheet;
using OfficeOpenXml;
using System.IO;
using System.Reflection;
using System.Data;

namespace ExcelWorker
{
    //КЛАСС ДЛЯ РАБОТЫ С EXCEL (УСТАНОВЛЕН OFFICE)
    class ExcelWorker
    {
        private static Excel_interop.Workbook MyBook = null;
        private static Excel_interop.Application MyApp = null;
        private static Excel_interop.Worksheet MySheet = null;

        private int lastRow { get; set; }

        public ExcelWorker(string path)
        {
            try
            {
                MyApp = new Excel_interop.Application();
                MyApp.Visible = false;
                MyBook = MyApp.Workbooks.Open(path);
                MySheet = (Excel_interop.Worksheet)MyBook.Sheets[1]; // Explicit cast is not required here
                MySheet.Unprotect();
                lastRow = MySheet.Cells.SpecialCells(Excel_interop.XlCellType.xlCellTypeLastCell).Row;
            } 
            catch (Exception ex)
            {
                throw new _1CException(ex.ToString());
            }
        }
        
        //Reading From Excel
        public void ReadExcel(ref IList<Attribs._1C_Attribs> _1C_RowList)
        {
            try
            {
                for (int index = 5; index <= lastRow; index++)
                {
                    System.Array MyValues = (System.Array)MySheet.get_Range("A" +
                       index.ToString(), "V" + index.ToString()).Cells.Value;

                    if ((MyValues.GetValue(1, 1) == "") || (MyValues.GetValue(1, 1) == null))
                        continue;
                    _1C_RowList.Add(new Attribs._1C_Attribs
                    {
                        ID_VACANCY = MyValues.GetValue(1, 1)==null?"":MyValues.GetValue(1, 1).ToString(),
                        PERSONNEL_NUMBER = MyValues.GetValue(1, 2) == null ? "" : MyValues.GetValue(1, 2).ToString(),
                        FIO = MyValues.GetValue(1, 3) == null ? "" : MyValues.GetValue(1, 3).ToString(),
                        LAST_NAME = MyValues.GetValue(1, 4) == null ? "" : MyValues.GetValue(1, 4).ToString(),
                        FIRST_NAME = MyValues.GetValue(1, 5) == null ? "" : MyValues.GetValue(1, 5).ToString(),
                        SECOND_NAME = MyValues.GetValue(1, 6) == null ? "" : MyValues.GetValue(1, 6).ToString(),
                        SAP = MyValues.GetValue(1, 7) == null ? "" : MyValues.GetValue(1, 7).ToString(),
                        SIGN = MyValues.GetValue(1, 8) == null ? "" : MyValues.GetValue(1, 8).ToString(),
                        CUSTOMER = MyValues.GetValue(1, 9) == null ? "" : MyValues.GetValue(1, 9).ToString(),
                        COUNT = MyValues.GetValue(1, 10) == null ? 0 : float.Parse(MyValues.GetValue(1, 10).ToString()),
                        BLOCK = MyValues.GetValue(1, 11) == null ? "" : MyValues.GetValue(1, 11).ToString(),
                        CATEGORY = MyValues.GetValue(1, 12) == null ? "" : MyValues.GetValue(1, 12).ToString(),
                        OFFICE_SBT = MyValues.GetValue(1, 13) == null ? "" : MyValues.GetValue(1, 13).ToString(),
                        DEPARTMENT_SBT = MyValues.GetValue(1, 14) == null ? "" : MyValues.GetValue(1, 14).ToString(),
                        GRADE = MyValues.GetValue(1, 15) == null ? 0 :  int.Parse(MyValues.GetValue(1, 15).ToString()),
                        CHIEF = MyValues.GetValue(1, 16) == null ? "" : MyValues.GetValue(1, 16).ToString(),
                        OFFICE_CORP = MyValues.GetValue(1, 17) == null ? "" : MyValues.GetValue(1, 17).ToString(),
                        DEPARTMENT_CORP = MyValues.GetValue(1, 18) == null ? "" : MyValues.GetValue(1, 18).ToString(),
                        ORG_POSITION = MyValues.GetValue(1, 19) == null ? "" : MyValues.GetValue(1, 19).ToString(),
                        EMAIL = MyValues.GetValue(1, 20) == null ? "" : MyValues.GetValue(1, 20).ToString(),
                        CORP_POSITION = MyValues.GetValue(1, 21) == null ? "" : MyValues.GetValue(1, 21).ToString(),
                        FIO_PREV = MyValues.GetValue(1, 22) == null ? "" : MyValues.GetValue(1, 22).ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                throw new _1CException(ex.ToString());
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
                throw new _1CException(ex.ToString());
            }
        }
        
        //Reading From Excel
        public void ReadExcel(ref IList<Attribs._1C_Attribs> _1C_RowList)
        {
            try
            {
                FileStream str = new FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite);
                ExcelLib.Workbook workbook = ExcelLib.Workbook.Load(str);
                ExcelLib.Worksheet worksheet = workbook.Worksheets[0];
                int lastRow = worksheet.Cells.LastRowIndex;
                
                for (int index = 4; index <= lastRow; index++)
                {
                    
                    ExcelLib.Row row = worksheet.Cells.GetRow(index);
                    if ((row.GetCell(0).Value == "") || (row.GetCell(0).Value == null))
                        continue;

                    _1C_RowList.Add(new Attribs._1C_Attribs
                    {
                        ID_VACANCY = row.GetCell(0).Value == null ? "" : row.GetCell(0).Value.ToString(),
                        PERSONNEL_NUMBER = row.GetCell(2).Value == null ? "" : row.GetCell(2).Value.ToString(),
                        FIO = row.GetCell(3).Value == null ? "" : row.GetCell(3).Value.ToString(),
                        LAST_NAME = row.GetCell(4).Value == null ? "" : row.GetCell(4).Value.ToString(),
                        FIRST_NAME = row.GetCell(5).Value == null ? "" : row.GetCell(5).Value.ToString(),
                        SECOND_NAME = row.GetCell(6).Value == null ? "" : row.GetCell(6).Value.ToString(),
                        SAP = row.GetCell(7).Value == null ? "" : row.GetCell(7).Value.ToString(),
                        SIGN = row.GetCell(8).Value == null ? "" : row.GetCell(8).Value.ToString(),
                        CUSTOMER = row.GetCell(9).Value == null ? "" : row.GetCell(9).Value.ToString(),
                        COUNT = row.GetCell(10).Value == null ? 0 : float.Parse(row.GetCell(10).Value.ToString()),
                        BLOCK = row.GetCell(11).Value == null ? "" : row.GetCell(11).Value.ToString(),
                        CATEGORY = row.GetCell(12).Value == null ? "" : row.GetCell(12).Value.ToString(),
                        OFFICE_SBT = row.GetCell(13).Value == null ? "" : row.GetCell(13).Value.ToString(),
                        DEPARTMENT_SBT = row.GetCell(14).Value == null ? "" : row.GetCell(14).Value.ToString(),
                        GRADE = row.GetCell(15).Value == null ? 0 : int.Parse(row.GetCell(15).Value.ToString()),
                        CHIEF = row.GetCell(16).Value == null ? "" : row.GetCell(16).Value.ToString(),
                        OFFICE_CORP = row.GetCell(17).Value == null ? "" : row.GetCell(17).Value.ToString(),
                        DEPARTMENT_CORP = row.GetCell(18).Value == null ? "" : row.GetCell(18).Value.ToString(),
                        ORG_POSITION = row.GetCell(19).Value == null ? "" : row.GetCell(19).Value.ToString(),
                        EMAIL = row.GetCell(20).Value == null ? "" : row.GetCell(20).Value.ToString(),
                        CORP_POSITION = row.GetCell(21).Value == null ? "" : row.GetCell(21).Value.ToString(),
                        FIO_PREV = row.GetCell(22).Value == null ? "" : row.GetCell(22).Value.ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                throw new _1CException(ex.ToString());
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
                throw new _1CException(ex.ToString());
            }
        }
        
        //Reading From Excel
        public void ReadExcel(ref IList<Attribs._1C_Attribs> _1C_RowList)
        {
            using (ExcelPackage p = new ExcelPackage())
            {
                using (var stream = new FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite))
                {
                    p.Load(stream);

                    ExcelWorksheet ws = p.Workbook.Worksheets[1];
                    int lRow = ws.Dimension.End.Row;

                    for (int index = 5; index <= lRow; index++)
                    {
                        if ((ws.Cells[index, 1].Value == "") || (ws.Cells[index, 1].Value == null))
                            continue;

                        _1C_RowList.Add(new Attribs._1C_Attribs
                        {
                            ID_VACANCY = ws.Cells[index, 1].Value == null ? "" : ws.Cells[index, 1].Value.ToString(),
                            PERSONNEL_NUMBER = ws.Cells[index, 2].Value == null ? "" : ws.Cells[index, 2].Value.ToString(),
                            FIO = ws.Cells[index, 3].Value == null ? "" : ws.Cells[index, 3].Value.ToString(),
                            LAST_NAME = ws.Cells[index, 4].Value == null ? "" : ws.Cells[index, 4].Value.ToString(),
                            FIRST_NAME = ws.Cells[index, 5].Value == null ? "" : ws.Cells[index, 5].Value.ToString(),
                            SECOND_NAME = ws.Cells[index, 6].Value == null ? "" : ws.Cells[index, 6].Value.ToString(),
                            SAP = ws.Cells[index, 7].Value == null ? "" : ws.Cells[index, 7].Value.ToString(),
                            SIGN = ws.Cells[index, 8].Value == null ? "" : ws.Cells[index, 8].Value.ToString(),
                            CUSTOMER = ws.Cells[index, 9].Value == null ? "" : ws.Cells[index, 9].Value.ToString(),
                            COUNT = ws.Cells[index, 10].Value == null ? 0 : float.Parse(ws.Cells[index, 10].Value.ToString()),
                            BLOCK = ws.Cells[index, 11].Value == null ? "" : ws.Cells[index, 11].Value.ToString(),
                            CATEGORY = ws.Cells[index, 12].Value == null ? "" : ws.Cells[index, 12].Value.ToString(),
                            OFFICE_SBT = ws.Cells[index, 13].Value == null ? "" : ws.Cells[index, 13].Value.ToString(),
                            DEPARTMENT_SBT = ws.Cells[index, 14].Value == null ? "" : ws.Cells[index, 14].Value.ToString(),
                            GRADE = ws.Cells[index, 15].Value == null ? 0 : int.Parse(ws.Cells[index, 15].Value.ToString()),
                            CHIEF = ws.Cells[index, 16].Value == null ? "" : ws.Cells[index, 16].Value.ToString(),
                            OFFICE_CORP = ws.Cells[index, 17].Value == null ? "" : ws.Cells[index, 17].Value.ToString(),
                            DEPARTMENT_CORP = ws.Cells[index, 18].Value == null ? "" : ws.Cells[index, 18].Value.ToString(),
                            ORG_POSITION = ws.Cells[index, 19].Value == null ? "" : ws.Cells[index, 19].Value.ToString(),
                            EMAIL = ws.Cells[index, 20].Value == null ? "" : ws.Cells[index, 20].Value.ToString(),
                            CORP_POSITION = ws.Cells[index, 21].Value == null ? "" : ws.Cells[index, 21].Value.ToString(),
                            FIO_PREV = ws.Cells[index, 22].Value == null ? "" : ws.Cells[index, 22].Value.ToString()
                        });
                    }
                }
            }
        }
    }
}
