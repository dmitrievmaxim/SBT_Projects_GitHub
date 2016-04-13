using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;


namespace HPSM_client
{
    class Program
    {
        public static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public readonly static string currentDate = DateTime.Now.ToShortDateString();
        static void Main(string[] args)
        {
            log.Info("---------------START----------------");
            BasicConfigurator.Configure();
            HPSM_Worker work = new HPSM_Worker();
            log.Info("Import SUCCESS\n");
            log.Info("---------------FINISH----------------");
        }
    }
}
