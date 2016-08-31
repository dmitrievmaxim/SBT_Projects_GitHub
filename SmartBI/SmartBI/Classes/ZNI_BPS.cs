using SmartBI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartBI.Classes
{
    public class ZNI_BPS
    {
        public List<SUM_PROJECT_ACTIVITY> ZNI_BPS_SUM_PROJECT_ACTIVITY { get; set; }
        public List<SUM_TASK_NOT_LINKED_ASBPS> ZNI_BPS_SUM_TASK_NOT_LINKED_ASBPS { get; set; }
        public List<SUM_ZNI_TASK_ASBPS> ZNI_BPS_SUM_ZNI_TASK_ASBPS { get; set; }

        public ZNI_BPS(Entities db)
        {
            ZNI_BPS_SUM_PROJECT_ACTIVITY = (from i in db.SUM_PROJECT_ACTIVITY select i).ToList<SUM_PROJECT_ACTIVITY>();
            ZNI_BPS_SUM_TASK_NOT_LINKED_ASBPS = (from i in db.SUM_TASK_NOT_LINKED_ASBPS select i).ToList<SUM_TASK_NOT_LINKED_ASBPS>();
            ZNI_BPS_SUM_ZNI_TASK_ASBPS = (from i in db.SUM_ZNI_TASK_ASBPS select i).ToList<SUM_ZNI_TASK_ASBPS>();
        }
    }
}