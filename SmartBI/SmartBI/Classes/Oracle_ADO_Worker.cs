using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using SmartBI.Models;

namespace SmartBI.Classes
{
    public class Oracle_ADO_Worker
    {
        public static ObjectContext GetActSpecASBPSContext()
        {
            ObjectContext context = (new Entities() as IObjectContextAdapter).ObjectContext;
            using (OracleConnection con = new OracleConnection(@"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.99.114)(PORT=1500))(CONNECT_DATA=(SERVICE_NAME=SBTJ2)));User Id=SBT_REP;Password=S8T_R3P;"))
            {
                ObjectQuery<DbDataRecord> query =
                    context.CreateQuery<DbDataRecord>("SELECT * FROM TEMPOLABOR");
            }
            return context;
        }
    }
}