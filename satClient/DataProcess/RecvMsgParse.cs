using JSONHelper;
using satMsg;
using System;
using System.Data;
using UniHelper;

namespace DataProcess
{
    public class RecvMsgParse
    {

        private JSONProcess satJson; //= new JSONProcess();
        private TianYuanPackage tyPack = new TianYuanPackage();

        private DataTable dt = new DataTable();
        
        public RecvMsgParse(string path)
        {
            satJson = new JSONProcess(path);
        }

        public DataTable ParseMessage(byte[] bvals)
        {
            byte[] entireBytes = getEntirePackage(bvals);  //获取完整的帧

            if (entireBytes == null) return null;

            tyPack = (TianYuanPackage)UniSerialize.ByteToStruct(entireBytes, tyPack.GetType());

            return getHouseKeepingPackage(getsatFrameBytes());

            //byte[] 
            //return new DataTable();
        }

        private byte[] getEntirePackage(byte[] bvals)
        {

            byte[] copy = new byte[bvals.Length];
           // Array.Copy(bvals, copy, bvals.Length);
            Array.Copy(bvals, 0, copy, 0, bvals.Length);
            return copy;
            //UniSerialize.
        }

        private byte[] getsatFrameBytes()
        {
            byte[] satframeBytes = new byte[tyPack.epdu.length];
            Array.Copy(tyPack.frame, 0, satframeBytes, 0, tyPack.epdu.length);
            return satframeBytes;
        }

        public ushort getMessageType()
        {
            return tyPack.epdu.getAPID();
        }

        public DataTable getHouseKeepingPackage(byte[] bvals)
        {
            
            
            satJson.DecodePackage(bvals,tyPack.epdu.getAPID());

            return new DataTable();
            //return new byte[4];
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
