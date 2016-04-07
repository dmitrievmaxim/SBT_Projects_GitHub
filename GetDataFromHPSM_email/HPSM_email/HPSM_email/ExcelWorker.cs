using System;
using System.Collections.Generic;
using System.ComponentModel; //Скачать из Nuget
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using HPSM_email;

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
}
