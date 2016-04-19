﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace _1C_client
{
    class _1CException : Exception
    {
        public static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public _1CException(string message):base(message)
        {
            BasicConfigurator.Configure();
            LoggingExtensions.Logging.Log.InitializeWith<LoggingExtensions.log4net.Log4NetLog>();
            Console.WriteLine(message);
            log.Error(message + "\n" + " Import FAILD\n");
        }
    }
}
