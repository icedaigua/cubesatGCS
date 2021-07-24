using JSONHelper;
using satMsg;
using System;
using System.Data;
using UniHelper;

namespace DataProcess
{
    public class RecvMsgParse
    {
        private byte[] entireBytes;

        private JSONProcess satJson; //= new JSONProcess();
        private TianYuanPackage tyPack = new TianYuanPackage();

        private DataTable dt = new DataTable();
        
        public RecvMsgParse(string path)
        {
            satJson = new JSONProcess(path);
        }

        public DataTable ParseMessage(byte[] bvals)
        {
            entireBytes = getEntirePackage(bvals);  //获取完整的帧

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

        public DataTable[] getHouseKeepingPackageHeader()
        {
            return satJson.dtArray;
        }


        private DataTable getHouseKeepingPackage(byte[] bvals)
        {

            return satJson.DecodePackage(bvals,tyPack.epdu.getAPID());
        }

        /// <summary>
        /// 原始数据生成DateTable
        /// </summary>
        /// <param name="bvals"></param>
        /// <returns></returns>
        public DataTable originDataToDataTable()
        {
            if (entireBytes == null)
                throw new ArgumentException("origin数据为空");
            DataTable dt = new DataTable("origin");

            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);

            //第一列为当前系统时间
            DataColumn dc = new DataColumn();
            dt.Columns.Add(dc);
            dr[0] = DateTime.UtcNow;
            ushort kc = 1;
            foreach (byte b in entireBytes)
            {
                DataColumn dc1 = new DataColumn();
                dt.Columns.Add(dc1);
                dr[kc++] = b.ToString("X2");
            }
            return dt;
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
