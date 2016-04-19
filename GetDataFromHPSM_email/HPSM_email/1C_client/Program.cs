using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace _1C_client
{
    class Program
    {
        public static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public readonly static string currentDate = DateTime.Now.ToShortDateString();
        static void Main(string[] args)
        {
            log.Info("---------------START----------------");
            log.Info("Started at: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString());
            BasicConfigurator.Configure();
            _1C_Worker work = new _1C_Worker();
            log.Info("Import SUCCESS\n");
            log.Info("Finished at: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString());
            log.Info("---------------FINISH----------------");
        }
    }
}
