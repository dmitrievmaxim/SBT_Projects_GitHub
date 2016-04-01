using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIRArestUpdate
{
    class Program
    {
        public static int Counter = 0;
        static void Main(string[] args)
        {
            List<Item> items = new List<Item>();

            //GET Data from Excel
            foreach (var path in GetPaths())
            {
                ReadExcel.GetExcel(path, ref items);
            }

            //Update Jira Data
            foreach (var item in items.Distinct())
            {
                new JiraUpdateData(item);
            }

            foreach (var str in items.Distinct())
            {
                Trace.WriteLine(str.zni + "\t" + str.date);
            }
            Console.WriteLine("Всего записей в таблицах Excel:{0}\tУникальных:{1}", items.Count, items.Distinct().Count());
            Console.ReadKey();
        }

        private static List<string> GetPaths()
        {
            string container = Constants.Parameters.Container;
            string[] files;
            files = System.IO.Directory.GetFiles(container);
            return files.OfType<string>().ToList<string>();
        }
    }
}
