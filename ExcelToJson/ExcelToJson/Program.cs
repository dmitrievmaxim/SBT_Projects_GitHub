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

namespace ExcelToJson
{
    class Program
    {
        static void Main(string[] args)
        {
            var pathExcel = @"C:\Users\SBT-Dmitriev-MV\DEV\tasks\Реестр.xls";
            var sheetName = "АС";
            var pathJson = @"C:\Users\SBT-Dmitriev-MV\DEV\tasks\projectsSource.json";
            int count ;

            var connectionString = String.Format(@"
                Provider=Microsoft.ACE.OLEDB.12.0;
                Data Source={0};
                Extended Properties=""Excel 12.0 Xml;HDR=YES""
                ", pathExcel);
            using (var conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = String.Format(
                    @"Select * From [{0}$]",
                    sheetName);

                using (var rdr = cmd.ExecuteReader())
                {
                    List<Object> source = new List<Object>();
                    while (rdr.Read())
                    {
                        if (rdr.GetValue(0).ToString() != "")
                        {
                            var obj = new
                            {
                                key = rdr[0].ToString(),
                                value = rdr[0] + " " + rdr[1]
                            };
                            source.Add(obj);
                            Console.WriteLine(rdr[0] + " " + rdr[1]);
                        }
                    }

                    //var query = from DbDataRecord row in rdr
                    //            where row.GetValue(0).ToString() != ""
                    //            select new { 
                    //                key = row[0],
                    //                value = row[0] + " " + row[1]
                    //            }; 

                    var json = JsonConvert.SerializeObject(source);
                    File.WriteAllText(pathJson, json);
                }
            }

            StringBuilder sb;
            using (StreamReader reader = new StreamReader(pathJson))
            {
                sb = new StringBuilder(reader.ReadToEnd());
                sb.Replace("{\"key\"", "\n{\"key\"");
            }

            using (StreamWriter sw = new StreamWriter(pathJson, false))
            {
                sw.WriteLine(sb);
            }

            Console.WriteLine("Выполнено");
            Console.ReadKey();
        }
    }
}
