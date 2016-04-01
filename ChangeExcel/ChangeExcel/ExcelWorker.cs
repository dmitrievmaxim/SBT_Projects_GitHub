using System;
using System.Collections.Generic;
using System.ComponentModel; //Скачать из Nuget
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace ChangeExcel
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
            } catch (Exception ex)
              {
                  Console.WriteLine(ex.ToString());
              }
        }
        
        //Reading From Excel
        public BindingList<Tempo> ReadExcel()
        {
            BindingList<Tempo> EmpList = new BindingList<Tempo>();
            try
            {
                for (int index = 2; index <= lastRow; index++)
                {
                    System.Array MyValues = (System.Array)MySheet.get_Range("A" +
                       index.ToString(), "D" + index.ToString()).Cells.Value;
                    EmpList.Add(new Tempo
                    {
                        key = MyValues.GetValue(1, 1).ToString(),
                        value = MyValues.GetValue(1, 2).ToString(),
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return EmpList;
        }

        //Writing to Excel
        public void WriteExcel(IList<Tempo> listJsonObj, string column)
        {
            int counterExRow = 0;
            string IdJsonRow = "";
            try
            {
                 for (int index = 2; index <= lastRow; index++)
                {
                    System.Array myValues = (System.Array)MySheet.get_Range(column +
                       index.ToString(), GetExcelColumnName(ExcelColumnNameToNumber(column) + 1) + index.ToString()).Cells.Value;
                    foreach (var num in listJsonObj)
                    {
                        //Debug.WriteLine(index);
                        counterExRow = index;
                        IdJsonRow = num.key;
                        if ((!string.IsNullOrEmpty(myValues.GetValue(1, 1).ToString())) && (num.key.Trim() == myValues.GetValue(1, 1).ToString().Trim()))
                        {
                            MySheet.Cells[index, ExcelColumnNameToNumber(column) + 1].Value = num.value.Replace(num.key, "");
                        }
                    }
                }
                MyBook.Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Exception in excel line: " + counterExRow + "\n and Json row with key: " + IdJsonRow);
            }
        }

        public static int ExcelColumnNameToNumber(string columnName)
        {
            int sum = 0;
            try
            {
                if (string.IsNullOrEmpty(columnName)) throw new ArgumentNullException("columnName");
                columnName = columnName.ToUpperInvariant();
                for (int i = 0; i < columnName.Length; i++)
                {
                    sum *= 26;
                    sum += (columnName[i] - 'A' + 1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return sum;
        }

        public static string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;
            try
            {
                while (dividend > 0)
                {
                    modulo = (dividend - 1) % 26;
                    columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                    dividend = (int)((dividend - modulo) / 26);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return columnName;
        }

        public void ExcelStop()
        {
            MyBook.Close();
            MyApp.Quit();
        }
    }
}
