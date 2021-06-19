using JSONHelper;
using System.Collections.Generic;

namespace DataProcess
{
    public class satDataProcess
    {

        private bool hasProtocol = false;

        public Dictionary<string, string> dicForShow { get; private set; }
        public Dictionary<string, string> dicForSave { get; private set; }
        //public Dictionary<string, string> dicChinese { get; private set; }


        private JSONProc satJson = new JSONProc();
        public satDataProcess()
        {

        }

        public void satDataProcess_Initz(string path)
        {
            satJson.JP_Initz(path);
            hasProtocol = true;
        }

        public Dictionary<string,string> getSatDataName()
        {
            if (hasProtocol)
                return satJson.getJsonChinese();
            else
                return null;
        }

        public void DataProcessFunc(byte[] buffer)
        {
            if(hasProtocol)
            {
                satJson.decodeBuf(buffer);

                dicForShow = satJson.dicForShow;
                dicForSave = satJson.dicForSave;
                //return satJson.dicForSave;
                return;
            }
            dicForShow = null;
            dicForSave = null;

        }


    }
}
