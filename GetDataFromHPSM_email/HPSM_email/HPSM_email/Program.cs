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
        public static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            HPSM_Worker work = new HPSM_Worker();
            log.Info("Import SUCCESS\n");
            Console.WriteLine("Import SUCCESS\n");
            Console.ReadKey();
        }
    }
}
