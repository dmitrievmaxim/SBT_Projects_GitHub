using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;


namespace HPSM_email
{
    class Program
    {
        static void Main(string[] args)
        {
            bool flag;
            string message;
            Files_Worker f_work = new Files_Worker();
            f_work.Work(out flag, out message);
            /*
            HPSM_Worker work = new HPSM_Worker();
            Console.WriteLine("Import SUCCESS\n");
            Console.ReadKey();*/
        }
    }
}
