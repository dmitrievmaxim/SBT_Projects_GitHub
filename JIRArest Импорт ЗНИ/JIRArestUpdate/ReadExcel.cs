using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace JIRArestUpdate
{
    class ReadExcel
    {
        public static void GetExcel(string path, ref List<Item> list)
        {
            string conString =
                @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";" +
                @"Extended Properties='Excel 8.0;HDR=Yes;'";

            using(OleDbConnection connection = new OleDbConnection(conString))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand("select * from [Отчет$]", connection);
                using(OleDbDataReader dr = command.ExecuteReader())
                {
                    while(dr.Read())
                    {
                        list.Add(new Item { zni = dr[0].ToString(), date = Convert.ToDateTime(dr[1].ToString()).ToString("yyyy-MM-dd")});
                    }
                }
            }
        }
    }
}
