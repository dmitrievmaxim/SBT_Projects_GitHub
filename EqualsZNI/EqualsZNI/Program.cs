using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ChangeExcel
{
    class Program
    {
        const string excelPathStructure = @"C:\Users\SBT-Dmitriev-MV\Desktop\Козырева\tmp\changeRequest.xls";
        const string exelPathReport = @"C:\Users\SBT-Dmitriev-MV\Desktop\Козырева\tmp\trud_09.02.xlsx";
        //const string jsonAsPath = @"C:\Users\SBT-Dmitriev-MV\DEV\scripts\ПРОМ\projectsSource.json";
        //const string jsonWorkType = @"C:\Users\SBT-Dmitriev-MV\DEV\scripts\ПРОМ\typesOfActivitiesSource.json";

        public enum attribs
        {
             L = 1, //Тип работ
             N = 2 //АС
        }

        static void Main(string[] args)
        {
            ExcelWorker excelWorker = new ExcelWorker(excelPathStructure);
            BindingList<Tempo> structList = excelWorker.ReadExcel(2);
            excelWorker = new ExcelWorker(exelPathReport);
            BindingList<Tempo> repList = excelWorker.ReadExcel(4);
            foreach (var id_struct in structList)
            {
                foreach (var id_trud in repList)
                {
                    if (id_struct.key == id_trud.key)
                        Console.WriteLine(id_trud.key);
                }
            }
            
            excelWorker.ExcelStop();
            Console.WriteLine("Успешно");
            Console.ReadKey();
        }
    }
}
