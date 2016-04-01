using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIRArestUpdate
{
    class Item
    {
        public string zni { get; set; }
        public string date { get; set; }
    }

    class JiraIssue
    {
        public string key { get; set; }
        public string currentIdZni { get; set; }
        public string currentDateZni { get; set; }
    }
}
