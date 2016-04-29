using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace HPSM_client
{
    class HTML_Worker
    {
        public string FilePath { get; set; }

        //string headerXPath = "//TD[@id='shapka']"; //Шапка
        //string rowXPath = "//TR[TD/@class='EF_TXT']"; //Строка

        public HTML_Worker(string path)
        {
            try
            {
                FilePath = path;
            } 
            catch (Exception ex)
            {
                throw new HPSMException(ex.ToString());
            }
        }

        public void ParseHTML(ref IList<Attribs.HPSM_Attribs> HPSM_RowList)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.Load(FilePath);

                foreach (HtmlNode nodeRow in doc.DocumentNode.SelectNodes("//tr"))//tr
                {
                    int arrSize = doc.DocumentNode.SelectNodes("//td[@id='shapka']").Count;
                    string[] tmpArr = new string[arrSize];
                    int counter = 0;
                    foreach (HtmlNode nodeCol in nodeRow.ChildNodes)//td
                    {
                        foreach (var attr in nodeCol.Attributes)
                        {
                            if ((attr.Name == "class") && (attr.Value == "E")) break;
                            else if ((attr.Name == "class") && (attr.Value == "EF_TXT"))
                            {
                                tmpArr[counter] = nodeCol.InnerHtml;
                                counter++;
                            }
                        }
                    }

                    if (tmpArr[arrSize - 1] != null) //Если добавлен последний елемент
                        HPSM_RowList.Add(new Attribs.HPSM_Attribs
                        {
                            I_NUMBER = tmpArr[0],
                            DT = Convert.ToDateTime(string.Format("{0:dd/MM/yy}", tmpArr[1])),
                            TIME_SPEND = (tmpArr[2] == null || tmpArr[2] == "") ? 0 : int.Parse(tmpArr[2]),
                            FIO = tmpArr[3] == null ? "" : tmpArr[3],
                            KE = tmpArr[4] == null ? "" : tmpArr[4],
                            OPERATOR = tmpArr[5] == null ? "" : tmpArr[5]
                        });
                }
            }
            catch (Exception ex)
            {
                throw new HPSMException(ex.ToString());
            }
        }
    }
}
