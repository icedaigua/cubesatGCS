
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace JSONHelper
{
    public class UIJson
    {
        public Dictionary<string, string> dicForShow { get; private set; }
        public Dictionary<string, string> dicForSave { get; private set; }
        public Dictionary<string, string> dicForOrigin { get; private set; }

        public JArray ja { get; private set; }

        public UIJson()
        {

        }

        public void UIJson_Initz(string path = "enlaiUI.json")
        {
            using (StreamReader sr = File.OpenText(path))
            {
                string json = sr.ReadToEnd();
                ja = (JArray)JsonConvert.DeserializeObject(json);

            }
        }

        public Dictionary<string, Dictionary<string, string>> getJsonChinese()
        {
            Dictionary<string, Dictionary<string, string>> dicChinese = new Dictionary<string, Dictionary<string, string>>();


            for (var i = 0; i < ja.Count; i++)
            {
                Dictionary<string, string> dicContent = new Dictionary<string, string>();
                JToken js = JToken.Parse(ja[i].ToString());

                string zone = js["zone"].ToString();
                string zoneCN = js["chinese"].ToString();
              
                JArray jaa = (JArray)js["content"];
                for (var j = 0; j < jaa.Count; j++)
                {
                    JToken jss = JToken.Parse(jaa[j].ToString());
                    string id = jss["id"].ToString();
                    string idCN = jss["chinese"].ToString();

                    dicContent.Add(id, idCN);
                   
                }

                dicChinese.Add(zone, dicContent);
            }

            return dicChinese;
        }

   
    }
}
