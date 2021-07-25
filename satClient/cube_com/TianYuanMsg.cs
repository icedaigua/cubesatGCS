using System;
using System.Runtime.InteropServices;
using UniHelper;

namespace satMsg
{
   
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 256)]
    public struct TianYuanPackage
    {
        public TTCHeader ttc;
        public MPDUHeader mpdu;
        public EPDUHeader epdu;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 196)]//6
        public byte[] frame;
        public ushort crc;

        public TianYuanPackage(byte scid, ushort apid)
        {
            ttc = new TTCHeader(scid);
            epdu = new EPDUHeader(apid);
            mpdu = new MPDUHeader();
            frame = new byte[196];
            crc = 0;
        }
    };


    #region 生成测试用码流
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PlatForm
    {
        public byte soft_id;                            //1
        public ushort reboot_count;                     //2

        public ushort rec_cmd_count;                    //2
        public ushort down_count;                       //2

        public uint last_reset_time;                  //4
        public byte work_mode;                          //1

        public uint utc_time;                         //4
        public short temp_hk;                           //2

        public uint on_off_status;                            //4
        public short batt_TEMP1;                               //2
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ADCS
    {
        public float velox;
        public float posix;

    }
    public struct SATFRAMEOBC
    {
        public TTCHeader tth;
        public MPDUHeader mpdu;
        public EPDUHeader epdu;
        public PlatForm pl;

        public SATFRAMEOBC(byte scid, ushort apid)
        {
            tth = new TTCHeader(scid);
            epdu = new EPDUHeader(apid);
            mpdu = new MPDUHeader();
            pl = new PlatForm();
        }
    };
    public struct SATFRAMEADCS
    {
        public TTCHeader tth;
        public MPDUHeader mpdu;
        public EPDUHeader epdu;
        public ADCS adcs;

        public SATFRAMEADCS(byte scid, ushort apid)
        {
            tth = new TTCHeader(scid);
            epdu = new EPDUHeader(apid);
            mpdu = new MPDUHeader();
            adcs = new ADCS();
        }
    };


    public class TianYuanMsg
    {
        private SATFRAMEOBC fobc;
        private SATFRAMEADCS fadcs;
        private TianYuanPackage typk;
        private ushort recCNT = 2;
        private ushort downCNT = 3;
        public TianYuanMsg(ushort obcapid = 0x0000, ushort adcsapid = 0x0C00)
        {
           
            byte scid = 0x54;
            fobc = new SATFRAMEOBC(scid, obcapid);
            fadcs = new SATFRAMEADCS(scid, adcsapid);
            typk = new TianYuanPackage(scid, obcapid);

            // TTCHeader tth = new TTCHeader();
            //tth.uiData0 = 0x5581;
            //tth.uiData1 = 0x00000080;

            //MessageBox.Show("scid = :" + fobc.tth.getSCID().ToString("X2"));
            //MessageBox.Show("obcapid = :" + fobc.epdu.getAPID().ToString("X2"));
            //MessageBox.Show("adcsapid = :" + fadcs.epdu.getAPID().ToString("X2"));

            //MessageBox.Show("length = :" + System.Runtime.InteropServices.Marshal.SizeOf(fobc.tth).ToString(""));
            //MessageBox.Show("length = :" + System.Runtime.InteropServices.Marshal.SizeOf(fobc.mpdu).ToString(""));
            //MessageBox.Show("length = :" + System.Runtime.InteropServices.Marshal.SizeOf(fobc.epdu).ToString(""));
        }

        public byte[] createOBCFrame()
        {

            fobc.pl.soft_id = 0x55;                            //1
            fobc.pl.reboot_count = 0x01;                     //2

            fobc.pl.rec_cmd_count = recCNT;                    //2
            fobc.pl.down_count = downCNT;                       //2

            fobc.pl.last_reset_time = 0;                  //4
            fobc.pl.work_mode = 0x10;                          //1

            fobc.pl.utc_time = UniFunction.xDateSeconds(DateTime.UtcNow);                         //4
            fobc.pl.temp_hk = -27;                           //2

            fobc.pl.on_off_status = 0xAA55AA55;                            //4
            fobc.pl.batt_TEMP1 = 27;                               //2



            byte[] bval = UniSerialize.StructToByte((PlatForm)fobc.pl);

            bval.CopyTo(typk.frame, 0);

            typk.epdu.length = (ushort)bval.Length;

            byte[] bval2 = UniSerialize.StructToByte((TianYuanPackage)typk);

            //string str = Encoding.ASCII.GetString(bval);
            //string str = "";
            //foreach(byte b in bval)
            //{
            //    str += "0x" + b.ToString("X2") + "\t";
            //}
            //str += "\n";
            //Trace.WriteLine("", "");
            //Trace.WriteLine("", str);
            //Trace.WriteLine("", "");
            return bval2;
        }

        public byte[] createADCSFrame()
        {

            fadcs.adcs.velox = (float)1.2;                            //1
            fadcs.adcs.posix = (float)123.45;                     //2

            return UniSerialize.StructToByte((SATFRAMEADCS)fadcs);
        }
    }

    #endregion
}
