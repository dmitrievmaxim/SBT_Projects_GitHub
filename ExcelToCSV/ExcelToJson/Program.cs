using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Linq;
using System.Data.Common;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Office.Interop.Excel;
using ChangeExcel;

namespace ExcelToJson
{
    class Program
    {
        //--------------------ОПИСАНИЕ УТИЛИТЫ-----------------//
        /*
         Данная утилита предназначена для подготовки данных формата excel к импорту (формат CSV) в кастомное поле JIRA
         
         */
        static void Main(string[] args)
        {
            var pathExcel = @"C:\Users\SBT-Dmitriev-MV\Desktop\ExcelToCSV\Реестр_АС_11.03.xls";
            var sheetName = "АС";
            var pathCSV = @"C:\Users\SBT-Dmitriev-MV\Desktop\ExcelToCSV\Реестр_АС_11.03_res.csv";
            int count ;

            var connectionString = String.Format(@"
                Provider=Microsoft.ACE.OLEDB.12.0;
                Data Source={0};
                Extended Properties=""Excel 12.0 Xml;HDR=YES""
                ", pathExcel);

            List<string> source = new List<string>();
            using (var conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = String.Format(
                    @"Select * From [{0}$]",
                    sheetName);

                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        if (rdr.GetValue(0).ToString() != "")
                        {
                            source.Add(rdr[0] + " " + rdr[1]);
                            Console.WriteLine(rdr[0] + " " + rdr[1]);
                        }
                    }
                }
            }

            ExcelWorker worker = new ExcelWorker(pathCSV);
            worker.WriteExcel(source);
            worker.ExcelStop();

            Console.WriteLine("Выполнено");
            Console.ReadKey();
        }
    }
}
