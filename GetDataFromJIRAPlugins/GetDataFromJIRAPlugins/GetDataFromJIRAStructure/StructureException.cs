﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace GetDataFromJIRAPlugins
{
    class StructureException:Exception
    {
        public static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public StructureException(string message)
            : base(message)
        {
            BasicConfigurator.Configure();
            LoggingExtensions.Logging.Log.InitializeWith<LoggingExtensions.log4net.Log4NetLog>();
            Console.WriteLine(message);
            log.Error(message + "\n" + " Import FAILD\n");
        }
    }
}