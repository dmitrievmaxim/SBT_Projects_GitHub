using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataFromJIRAStructure
{
    public class Attribs
    {
        public class JIRA_Structure_Attribs
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }

        public class JIRA_Structure_Root
        {
            public int ID { get; set; }
        }

        public class JIRA_Structure_Root_Forest
        {
            public int ID { get; set; } //IDENTITY
            public int ID_struct { get; set; }
            public int ID_root { get; set; }
        }

        public class JIRA_Structure_Child
        {
            public int ID { get; set; }
        }

        public class JIRA_Structure_Child_Forest
        {
            public int ID { get; set; } //IDENTITY
            public int ID_struct { get; set; }
            public int ID_root { get; set; }
            public int ID_child { get; set; }
        }


        /*
        public class Jira_Structure_Root_Forest_Attribs
        {
            public int ID { get; set; }
            public int ID_root_fk { get; set; }
            public int Value { get; set; }
        }

        public class Jira_Structure_Child_Forest_Attribs
        {
            public int ID { get; set; }
            public int ID_root_forest_fk { get; set; }
            public int Value { get; set; }
        }
         * */

        public class JIRA_Tempo_Attribs
        {

        }
    }
}
