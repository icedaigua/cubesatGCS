using System.Runtime.InteropServices;

namespace satMsg
{
    /// <summary>
    /// 遥测帧头
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1,Size =58)]
    public struct TTCHeader
    {
        public uint Header;                                 //4
        public ushort uiData0;                              //2
        public uint uiData1;                                //4
        public byte formatVER;                              //1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]//6
        public byte[] time; 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]//9
        public byte[] rsvd1;
        public byte hkmode;                                 //1
        public byte baudrate;                               //1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)] //30
        public byte[] rsvd2;


        public TTCHeader(byte scid)
        {
            this.Header = 0x1DFCCF1A;
            this.uiData0 = (ushort)(((ushort)scid) << 2);
            this.uiData1 = 0;
            this.formatVER = 0;
            this.time = new byte[6];
            this.rsvd1 = new byte[9];
            this.hkmode = 0;
            this.baudrate = 0xAA;
            this.rsvd2 = new byte[30];
        }
        public byte getVERSION()  //  低2位
        {
            return (byte)(this.uiData0 & 0x0003);
        }

        public byte getSCID()  //  中间8位
        {
           return (byte)((this.uiData0 & 0x03FC) >> 2);
        }
        public byte getVCID() //  高6位
        {
            return (byte)((this.uiData0 & 0xFC00)>>10);
        }
        public uint getFrameCNT()   //低24位 0-23bit
        {

            return (this.uiData1 & 0x00FFFFFF);
        }
        public byte getRealTimeFlag()  //  第25位
        {
            return (byte)((this.uiData1 & 0x01FFFFFF)>>24);
        }
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 2)]
    public struct MPDUHeader
    {
        public ushort uiData;                     //2

        public ushort getFirstHeader()  // //占用高11位
        {

            return (ushort)((this.uiData & 0xFFE0)>>5);
            //set
            //{
            //    this.uiData = (ushort)((value & 0x07FF) | this.uiData);
            //}
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 6)]
    public struct EPDUHeader
        {  
            public ushort uiData0;   //2                           
            public ushort uiData1;//2
            public ushort length; //2

        public EPDUHeader(ushort apid)
        {
            this.uiData0 = (ushort)(apid);
            this.uiData1 = 0;
            this.length = 0;
        }
        public byte getVerNO()                     //占用低三位
        {
            return (byte)(this.uiData0 & 0x0007);
        }
        public byte getType()                     //占用低第四位
        {
            return (byte)((this.uiData0 & 0x08) >> 3);
        }
        public byte getHeader()                     //占用低第五位
        {
           return (byte)((this.uiData0 & 0x0010) >> 4);
        }
        public ushort getAPID()  //占用高11位
        {

            return (ushort)((this.uiData0 & 0xFFE0));
        }

        public byte getGroupID()                     //占用低两位
        {
            return (byte)(this.uiData1 & 0x0003);
        }
        public ushort getPackCNT()                     //占用高十四位
        {
             return (ushort)((this.uiData1 & 0xFFFC)>>2);
        }
    }


    #region for test
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
        public SATFRAMEOBC(byte scid,ushort apid)
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
    #endregion
}
