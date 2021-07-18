using JSONHelper;
using System.Collections.Generic;
using satMsg;


namespace DataProcess
{
    public class RecvMsgParse
    {

        private bool hasProtocol = false;

        //public Dictionary<string, string> dicForShow { get; private set; }
        //public Dictionary<string, string> dicForSave { get; private set; }
        //public Dictionary<string, string> dicChinese { get; private set; }


        private JSONProcess satJson; //= new JSONProcess();
        
        public RecvMsgParse(string path)
        {
            satJson = new JSONProcess(path);
        }

        //public void RecvMsgInitz(string path)
        //{
        //    satJson = new JSONProcess(path);
        //    //satJson.jsonProcInitz(path);
        //    //hasProtocol = true;
        //}


        public TTCHeader getTTCHeader(byte[] bvals)
        {
            return new TTCHeader();
        }

        public EPDUHeader getEPDUHeader(byte[] bvals)
        {
            return new EPDUHeader();
        }

        public byte getMessageType(byte[] bvals)
        {
            return 0;
        }

        public byte[] getHouseKeepingPackage(byte[] bvals)
        {
            satJson.DecodePackage(bvals);

            return new byte[4];
        }

        //public Dictionary<string,string> getSatDataName()
        //{
        //    if (hasProtocol)
        //        return satJson.getJsonChinese();
        //    else
        //        return null;
        //}

        //public void DataProcessFunc(byte[] buffer)
        //{
        //    if(hasProtocol)
        //    {
        //        satJson.decodeBuf(buffer);

        //      //  dicForShow = satJson.dicForShow;
        //      //  dicForSave = satJson.dicForSave;
        //        //return satJson.dicForSave;
        //        return;
        //    }
        //    //dicForShow = null;
        //    //dicForSave = null;

        //}


    }
}
