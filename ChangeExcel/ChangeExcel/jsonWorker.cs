using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeExcel
{
    class jsonWorker
    {
        public string jsonText { get; set; }

        public jsonWorker()
        {
        }

        public IList<Tempo> Work()
        {
            IList<Tempo> jsonObj = new List<Tempo>();
            try
            {
                JObject textSearch = JObject.Parse(jsonText);
                IList<JToken> results = textSearch["values"].Children().ToList();
                foreach (JToken result in results)
                {
                    Tempo searchResult = JsonConvert.DeserializeObject<Tempo>(result.ToString());
                    jsonObj.Add(searchResult);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return jsonObj;
        }
    }
}
