using System.Runtime.InteropServices;

namespace satCompent.Model
{
   

    /// <summary>
    /// Float结构体
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 4)]
    public struct FVal
    {
        [FieldOffset(0)]
        public ushort no;
        [FieldOffset(2)]
        public ushort exceed;
        [FieldOffset(4)]
        public float val;

        public FVal(ushort no, ushort exceed = 0, float val = 0)
        {
            this.no = no;
            this.exceed = exceed;
            this.val = val;
        }
    }

    /// <summary>
    /// Double结构体
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 4)]
    public struct DVal
    {
        [FieldOffset(0)]
        public ushort no;
        [FieldOffset(2)]
        public ushort exceed;
        [FieldOffset(4)]
        public double val;

        public DVal(ushort no, ushort exceed = 0, double val = 0)
        {
            this.no = no;
            this.exceed = exceed;
            this.val = val;
        }
    }

    /// <summary>
    /// GPS，结构化内存排列，总字节数：104
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct MsgOfGps
    {
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string name;  //卫星名[8]
        [FieldOffset(8)]
        public int year;  //年
        [FieldOffset(12)]
        public int month;  //月
        [FieldOffset(16)]
        public int day;  //日
        [FieldOffset(20)]
        public int hour;  //时
        [FieldOffset(24)]
        public int minute;  //分
        [FieldOffset(28)]
        public double second;  //秒

        [FieldOffset(36)]
        public double gpsWeek;  //GPS周
        [FieldOffset(44)]
        public double gpsSecond;  //GPS秒
        [FieldOffset(52)]
        public double posX;  //卫星位置X方向:m
        [FieldOffset(60)]
        public double posY;  //卫星位置Y方向:m
        [FieldOffset(68)]
        public double posZ;  //卫星位置Z方向:m
        [FieldOffset(76)]
        public double speedX;  //卫星速度X方向:m/s
        [FieldOffset(84)]
        public double speedY;  //卫星速度Y方向:m/s
        [FieldOffset(92)]
        public double speedZ;  //卫星速度Z方向:m/s
        [FieldOffset(100)]
        public int valid;  //数据有效标识

        public MsgOfGps(string satellite)
        {
            this.name = satellite;
            this.year = 2000;
            this.month = 1;
            this.day = 1;
            this.hour = 0;
            this.minute = 0;
            this.second = 0;

            this.gpsWeek = this.gpsSecond = this.posX = this.posY = this.posZ = this.speedX = this.speedY = this.speedZ = this.valid = 0;
        }
    }

    /// <summary>
    /// 姿态角，结构化内存排列，总字节数：88
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct MsgOfAttitude
    {
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string name;  //卫星名[8]
        [FieldOffset(8)]
        public int year;  //年
        [FieldOffset(12)]
        public int month;  //月
        [FieldOffset(16)]
        public int day;  //日
        [FieldOffset(20)]
        public int hour;  //时
        [FieldOffset(24)]
        public int minute;  //分
        [FieldOffset(28)]
        public double second;  //秒

        [FieldOffset(36)]
        public double roll;  //滚动角
        [FieldOffset(44)]
        public double pitch;  //俯仰角
        [FieldOffset(52)]
        public double yaw;  //偏航角

        public MsgOfAttitude(string satellite)
        {
            this.name = satellite;
            this.year = 2000;
            this.month = 1;
            this.day = 1;
            this.hour = 0;
            this.minute = 0;
            this.second = 0;

            this.roll = this.pitch = this.yaw = 0;
        }
    }

   
    /// <summary>
    /// 轨道，结构化内存排列，总字节数：84
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct MsgOfOrbit
    {
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string name;  //卫星名[8]
        [FieldOffset(8)]
        public int year;  //年
        [FieldOffset(12)]
        public int month;  //月
        [FieldOffset(16)]
        public int day;  //日
        [FieldOffset(20)]
        public int hour;  //时
        [FieldOffset(24)]
        public int minute;  //分
        [FieldOffset(28)]
        public double second;  //秒

        [FieldOffset(36)]
        public double axis;  //轨道半长轴a(m)
        [FieldOffset(44)]
        public double ecc;  //轨道偏心率e
        [FieldOffset(52)]
        public double dip;  //轨道倾角i
        [FieldOffset(60)]
        public double lng;  //升交点赤经Ω
        [FieldOffset(68)]
        public double arg;  //近地点幅角ω
        [FieldOffset(76)]
        public double mean;  //平近点角M

        public MsgOfOrbit(string satellite)
        {
            this.name = satellite;
            this.year = 2000;
            this.month = 1;
            this.day = 1;
            this.hour = 0;
            this.minute = 0;
            this.second = 0;

            this.axis = this.ecc = this.dip = this.lng = this.arg = this.mean = 0;
        }
    }

    /// <summary>
    /// 电流，结构化内存排列，总字节数：52
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct MsgOfCurr
    {
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string name;  //卫星名[8]
        [FieldOffset(8)]
        public int year;  //年
        [FieldOffset(12)]
        public int month;  //月
        [FieldOffset(16)]
        public int day;  //日
        [FieldOffset(20)]
        public int hour;  //时
        [FieldOffset(24)]
        public int minute;  //分
        [FieldOffset(28)]
        public double second;  //秒

        [FieldOffset(36)]
        public float lineCurr14;  //+14V母线电流:mA
        [FieldOffset(40)]
        public float lineCurr12P;  //+12V电流:mA
        [FieldOffset(44)]
        public float lineCurr12N;  //-12V电流:A
        [FieldOffset(48)]
        public float lineCurr5;  //+5V电流:mA

        public MsgOfCurr(string satellite)
        {
            this.name = satellite;
            this.year = 2000;
            this.month = 1;
            this.day = 1;
            this.hour = 0;
            this.minute = 0;
            this.second = 0;

            this.lineCurr14 = this.lineCurr12P = this.lineCurr12N = this.lineCurr5 = 0;
        }
    }

    /// <summary>
    /// 遥测处理结果，结构化内存排列，总字节数：276
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct MsgOfResult
    {
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string name;  //卫星名[8]
        [FieldOffset(8)]
        public int year;  //年
        [FieldOffset(12)]
        public int month;  //月
        [FieldOffset(16)]
        public int day;  //日
        [FieldOffset(20)]
        public int hour;  //时
        [FieldOffset(24)]
        public int minute;  //分
        [FieldOffset(28)]
        public double second;  //秒

        [FieldOffset(36)]
        public byte pcm;  //PCM
        [FieldOffset(37)]
        public byte bcr;  //BCR
        [FieldOffset(38)]
        public byte state0;  //状态量第1路
        [FieldOffset(39)]
        public byte state1;  //状态量第2路
        [FieldOffset(40)]
        public byte state2;  //状态量第3路
        [FieldOffset(41)]
        public byte state3;  //状态量第4路
        [FieldOffset(42)]
        public ushort digit;  //数字量第1路
        [FieldOffset(44)]
        public FVal panelVolt;  //帆板电压:V
        [FieldOffset(52)]
        public FVal panelCurrXP;  //帆板+X电流:mA
        [FieldOffset(60)]
        public FVal panelTempXP;  //帆板+X温度:℃
        [FieldOffset(68)]
        public FVal panelCurrXN;  //帆板-X电流:mA
        [FieldOffset(76)]
        public FVal panelTempXN;  //帆板-X温度:℃
        [FieldOffset(84)]
        public FVal panelCurrYP;  //帆板+Y电流:mA
        [FieldOffset(92)]
        public FVal panelTempYP;  //帆板+Y温度:℃
        [FieldOffset(100)]
        public FVal panelCurrYN;  //帆板-Y电流:mA
        [FieldOffset(108)]
        public FVal panelTempYN;  //帆板-Y温度:℃
        [FieldOffset(116)]
        public FVal batteryVolt;  //电池组电压:V
        [FieldOffset(124)]
        public FVal batteryCurr;  //电池组电流:mA
        [FieldOffset(132)]
        public FVal batteryTemp;  //电池组温度:℃
        [FieldOffset(140)]
        public FVal lineVolt14;  //+14V母线电压:V
        [FieldOffset(148)]
        public FVal lineCurr14;  //+14V母线电流:mA
        [FieldOffset(156)]
        public FVal lineVolt12P;  //+12V电压:V
        [FieldOffset(164)]
        public FVal lineCurr12P;  //+12V电流:mA
        [FieldOffset(172)]
        public FVal lineVolt12N;  //-12V电压:V
        [FieldOffset(180)]
        public FVal lineCurr12N;  //-12V电流:A
        [FieldOffset(188)]
        public FVal lineVolt5;  //+5V电压:V
        [FieldOffset(196)]
        public FVal lineCurr5;  //+5V电流:mA
        [FieldOffset(204)]
        public FVal subCurrPcm;  //PCM电流:mA
        [FieldOffset(212)]
        public FVal subVoltBd2;  //BD2电压:V
        [FieldOffset(220)]
        public FVal subCurrObc;  //OBC电流:A
        [FieldOffset(228)]
        public FVal tempObc;  //OBC温度:℃
        [FieldOffset(236)]
        public FVal subCurrTx;  //TX电流:mA
        [FieldOffset(244)]
        public FVal tempRf;  //RF温度:℃
        [FieldOffset(252)]
        public FVal rxAgc;  //RX AGC电平遥测:dB
        [FieldOffset(260)]
        public FVal rxAfc;  //RX AFC电平遥测:Hz
        [FieldOffset(268)]
        public FVal txPower;  //TX功放输出功率:mW

        public MsgOfResult(string satellite)
        {
            this.name = satellite;
            this.year = 2000;
            this.month = 1;
            this.day = 1;
            this.hour = 0;
            this.minute = 0;
            this.second = 0;

            this.pcm = this.bcr = 0x0;  //PCM, BCR
            this.state0 = this.state1 = this.state2 = this.state3 = 0x0;  //状态量
            this.digit = 0x0;  //数字量
            this.panelVolt = new FVal(0);  //帆板电压
            this.panelCurrXP = new FVal(1);  //帆板+X电流:mA
            this.panelTempXP = new FVal(2);  //帆板+X温度:℃
            this.panelCurrXN = new FVal(3);  //帆板-X电流:mA
            this.panelTempXN = new FVal(4);  //帆板-X温度:℃
            this.panelCurrYP = new FVal(5);  //帆板+Y电流:mA
            this.panelTempYP = new FVal(6);  //帆板+Y温度:℃
            this.panelCurrYN = new FVal(7);  //帆板-Y电流:mA
            this.panelTempYN = new FVal(8);  //帆板-Y温度:℃
            this.batteryVolt = new FVal(9);  //电池组电压:V
            this.batteryCurr = new FVal(10);  //电池组电流:mA
            this.batteryTemp = new FVal(11);  //电池组温度:℃
            this.lineVolt14 = new FVal(12);  //+14V母线电压:V
            this.lineCurr14 = new FVal(13);  //+14V母线电流:mA
            this.lineVolt12P = new FVal(14);  //+12V电压:V
            this.lineCurr12P = new FVal(15);  //+12V电流:mA
            this.lineVolt12N = new FVal(16);  //-12V电压:V
            this.lineCurr12N = new FVal(17);  //-12V电流:A
            this.lineVolt5 = new FVal(18);  //+5V电压:V
            this.lineCurr5 = new FVal(19);  //+5V电流:mA
            this.subCurrPcm = new FVal(20);  //PCM电流:mA
            this.subVoltBd2 = new FVal(21);  //BD2电压:V
            this.subCurrObc = new FVal(22);  //OBC电流:A
            this.tempObc = new FVal(23);  //OBC温度:℃
            this.subCurrTx = new FVal(24);  //TX电流:mA
            this.tempRf = new FVal(25);  //RF温度:℃
            this.rxAgc = new FVal(26);  //RX AGC电平遥测:dB
            this.rxAfc = new FVal(27);  //RX AFC电平遥测:Hz
            this.txPower = new FVal(28);  //TX功放输出功率:mW
        }
    }


}
