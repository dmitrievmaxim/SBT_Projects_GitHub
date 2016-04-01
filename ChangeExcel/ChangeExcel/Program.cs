using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ChangeExcel
{
    class Program
    {
        const string excelPath = @"C:\Users\SBT-Dmitriev-MV\DEV\tasks\Эдуард\LABORALLPROJECT.xls";
        const string jsonAsPath = @"C:\Users\SBT-Dmitriev-MV\DEV\scripts\ПРОМ\projectsSource.json";
        const string jsonWorkType = @"C:\Users\SBT-Dmitriev-MV\DEV\scripts\ПРОМ\typesOfActivitiesSource.json";

        public enum attribs
        {
             O = 1, //Тип работ
             Q = 2 //АС
        }

        static void Main(string[] args)
        {
            string jsonAsText = System.IO.File.ReadAllText(jsonAsPath);
            string jsonWorkTypeText = System.IO.File.ReadAllText(jsonWorkType);
            ExcelWorker excelWorker = new ExcelWorker(excelPath);
            jsonWorker jsonWorker = new jsonWorker();
            jsonWorker.jsonText = jsonAsText;
            excelWorker.WriteExcel(jsonWorker.Work(), attribs.Q.ToString());
            jsonWorker.jsonText = jsonWorkTypeText;
            excelWorker.WriteExcel(jsonWorker.Work(), attribs.O.ToString());
            excelWorker.ExcelStop();
            Console.WriteLine("Успешно");
            Console.ReadKey();
        }
    }
}
