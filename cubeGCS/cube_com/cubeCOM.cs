using System;
using System.Runtime.InteropServices;

namespace CubeCOM
{
    public static class cubeCOMM
    {

        #region 指令定义
        public const byte
                     INS_APP_HK_GET = 0x02,//
                     INS_HK_GET = 0x03,//下行星上状态遥测数据
             
                     INS_DOWN_CMD_ON = 0x05,  //下行星上指令数据
                     INS_APP_STR_DOWN = 0x06,//

            INS_OBC_RST = 0x08,//星务计算机重启
            INS_DOWN_CMD_OFF = 0x09,//停止下行星上指令数据

            INS_OBC_EPS_ON		=	0x0A ,
            INS_OBC_EPS_OFF		=	0x0B ,
            INS_OBC_WORKMODE	=	0x0C ,


            //开关指令
            INS_MTQ_ON = 0x10, //磁棒开
            INS_MTQ_OFF = 0x11, //磁棒关

            INS_GPS_A_ON = 0x12, //GPSA开
            INS_GPS_A_OFF = 0x13, //GPSA关
            INS_ADCS_ON = 0x15, //ADCS开机
            INS_ADCS_OFF = 0x14, //ADCS关机

            INS_RSV_ON = 0x16, //保留开关1开
                     INS_RSV_OFF = 0x17, //保留开关1关

            INS_MW_A_ON = 0x18, //动量轮A开         
            INS_MW_A_OFF = 0x19, //动量轮A关
            INS_MW_B_ON = 0x1A, //动量轮B开
            INS_MW_B_OFF = 0x1B, //动量轮B关

            INS_SLBRD_ON = 0x1C, //帆板开
            INS_SLBRD_OFF = 0x1D, //帆板关
            INS_USB_ON = 0x1E, //天线开
                     INS_USB_OFF = 0x1F, //USB关

            INS_S1_ON = 0x20, //动量轮C开
            INS_S1_OFF = 0x21, //动量轮C关

            INS_S2_ON = 0x22, //天线电源开
            INS_S2_OFF = 0x23, //天线电源关

            INS_S3_ON = 0x24, //陀螺仪开
            INS_S3_OFF = 0x25, //陀螺仪关

            INS_S4_ON = 0x26, //动量轮D开
            INS_S4_OFF = 0x27, //动量轮D关
                     INS_MTQ1_DIR_POS = 0x28, //磁棒1正方向
                     INS_MTQ1_DIR_NAG = 0x29, //磁棒1反方向
                     INS_MTQ2_DIR_POS = 0x2A, //磁棒2正方向
                     INS_MTQ2_DIR_NAG = 0x2B, //磁棒2反方向
                     INS_MTQ3_DIR_POS = 0x2C, //磁棒3正方向
                     INS_MTQ3_DIR_NAG = 0x2D, //磁棒3反方向

            INS_PIANZHI_MODE = 0x2E,    //偏置动量模式
            INS_ZERO_MODE = 0x2F,       //零动量模式

            INS_DET = 0x31, //重新阻尼
            INS_STA = 0x32,  //永久阻尼使能
            INS_DUMP_FOEV_DIS = 0x33, //永久阻尼禁止

            INS_SW_MAG_A_ON = 0x34, //磁强计A开
            INS_SW_MAG_B_ON = 0x35, //磁强计B开


                     INS_SW_MW_A = 0x36, //切换至动量轮A
                     INS_SW_MW_B = 0x37, //切换至动量轮B

            INS_SW_MAG_A_OFF = 0x38, //磁强计A关
            INS_SW_MAG_B_OFF = 0x39, //磁强计B关

            INS_SW_BATT_WARM_ON = 0x3A, //电源加热开
            INS_SW_BATT_WARM_OFF = 0x3B, //电源加热关


            INS_ERROR_ENABLE = 0x3D,
            INS_RSH = 0x3E,
            INS_CLOSE_ALL = 0x3F,

            //系统指令 
            INS_SW_1200 = 0x41, //BPSK1200切换
            INS_SW_9600 = 0x42,  //BPSK9600开
            INS_CW_ON = 0x43, //CW开
            INS_COM_TRAN_OFF = 0x44, //通信机发射机关机

            INS_COM_PERIOD = 0x45, //下行更新

            //数据注入指令

            INS_CTL_P_PRA = 0x51, //三轴稳定控制律注入
            INS_CTL_D_PRA = 0x52, //三轴稳定控制律注入
            INS_ZJD_CTL = 0x53, //章进动控制系数

            INS_DMP_FLAG = 0x54, //阻尼标志位
            INS_FLT_FLAG = 0x55, //测量标志位
            INS_CTL_FLAG = 0x56, //控制标志位
            INS_GYRO_FILTER_ON = 0x57, //开启陀螺仪滤波
            INS_GYRO_FILTER_OFF = 0x58, //关闭陀螺仪滤波
            INS_CNT_CTL_FLAG = 0x59, //

            INS_ORB_TLE_FLAG = 0x5A, //TLE轨道上注

            INS_MAG_FILTER_ON = 0x5B,    //启动磁强计滤波器
            INS_QR = 0x5C,    //QR参数注入
            INS_MAG_FILTER_OFF = 0x5D,    //关闭磁强计滤波

            INS_TIME_IN = 0x5E,    //姿控参数注入

            //INS_EQUP_INPUT = 0x61, //器件健康状态变更指令
            //INS_TEL_ADRS = 0x62, //遥测存储指针变更
            //INS_GPS_ADRS = 0x63, //GPS存储指针变更

            //INS_TIME_IN = 0x64, //时间注入


            //INS_FIPEX_SCRIPT_IN = 0x65, //Fipex指令注入

            //INS_FIPEX_ON = 0x66,
            //INS_FIPEX_OFF = 0x67,
            //INS_FIPEX_DOWN = 0x68,



            //INS_CAMERA = 0x70,
            //INS_DOWN_SAVED_WAV = 0x72,
            //INS_UP_NEW_WAV = 0x73,
            //INS_DOWN_NEW_WAV = 0x74,



            INS_ADCS = 0xFF;              //获取ADCS遥测

        #endregion

        public const byte ctrl_length = 15,
                            para_length = 23,
                            orbit_length = 79;

        public const byte
                        obc_length = 82,
                        adcs_length = 221
                       ;

        #region 下行帧定义
        public const byte FRAME_NULL = 0x00, FRAME_START = 0x01,
                        FRAME_OBC = 0x50, FRAME_ADCS = 0x51, FRAME_UV = 0x10,
                            FRAME_RESPONSE = 0x53, FRAME_CAMERA = 0x56;
        #endregion

        #region 星务下行帧

        [StructLayout(LayoutKind.Sequential, Pack = 1, Size = obc_length)]
        public struct down_obc_ST
        {
            // header 2
            public byte header0;  //0xeb 0x50               //2
            public byte header1;  //0xeb 0x50         
            
            //obc  32
            public byte sat_id;                             //1
            public byte soft_id;                            //1
            public UInt16 reboot_count;                     //2
            public byte reset_cause;                        //1

            public UInt16 rec_cmd_count;                    //2
            public byte rec_cmd_ID;
            public byte cmd_proc_status;

            public UInt16 down_count;                       //2
            public UInt32 last_reset_time;                  //4
            public byte work_mode;                          //1

            public UInt32 utc_time;                         //4
            public Int16 temp_hk;                           //2

            public UInt32 on_off_status;                            //4

            public byte flash_block;                                //1
            public byte flash_index;                                //1
            public byte sram_block;                                 //1
            public byte sram_index;					                //1


            public UInt32 file_sd_time_latest;                      //4
            public byte sd_card_status;                             //1
            public UInt16 sd_saved_cnt;                             //2

            public UInt32 camera_time_latest;                      //4
            public UInt16 camera_saved_cnt;                             //2

            //eps 37
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public sbyte[] temp_batt_board;                         //2
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public sbyte[] temp_eps;                                //4


            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] sun_c;                                  //6
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] sun_v;                                  //6
       
            public UInt16 out_BusC;                                 //2
            public UInt16 out_BusV;                                 //2
            public UInt16 UV_board_C;                               //2

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] Vol_5_C;                                //6
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public byte[] Bus_c;                                  //5

            public UInt16 eps_switch_status;                      //2

        };

        [StructLayout(LayoutKind.Sequential, Pack = 1, Size = adcs_length)]
        public struct down_adcs_ST
        {
            public byte header0;  //0xeb 0x50         //2
            public byte header1;  //0xeb 0x50         

            public UInt16 rst_cnt;                      //2
            public UInt16 rcv_cnt;                      //2
            public UInt16 ack_cnt;                      //2
            public UInt32 rst_time;                     //4
            public UInt32 utc_time;                     //4
            public Int16 temp_cpu;                      //2
            public byte adcs_ctrl_mode;                 //1  //downAdcsmagDotDmpFlg downAdcspitFltComFlg downAdcsattStaFlg
            public UInt16 downAdcscntDmp;               //2
            public UInt16 downAdcscntPitcom;            //2
            public UInt16 downAdcscntAttSta;            //2
            public byte error;                          //1


            public UInt16 sw_status;                    //2
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]    //6
            public Int16[] downAdcsMagnetometer;    //double to int16_t  原始值个位不要 除以10取整

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]    //6
            public Int16[] downAdcsGyro_Meas;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]    //8
            public UInt16[] downAdcsSun1_Meas;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]    //8
            public UInt16[] downAdcsSun2_Meas;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]    //8
            public UInt16[] downAdcsSun3_Meas;
            public byte downAdcsSunSensorFlag;                      //1

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]    //12
            public float[] downAdcsSun_Meas;
            public UInt16 downAdcsWheelSpeed_Meas;//取整          //2             
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]     //6
            public Int16[] downAdcsMTQOut;          // *1000

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]    //6
            public Int16[] downAdcsMagInO;          //double to int16_t  原始值个位不要 除以10取整

            public Int16 downAdcsPitAngle;              //2
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]    //4
            public Int16[] downAdcsPitFltState;
            public float downAdcsPitFltNormP;                       //4

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]    //12
            public float[] downAdcsTwoVector_euler;

            public UInt16 downAdcsTwoVectorCnt;                      //2
            public UInt16 downAdcsMagSunFltCnt;                      //2
            public UInt16 downAdcsMagGyroFltCnt;                      //2

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]    //16
            public float[] downAdcsMagSunFltQ;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]    //12
            public float[] downAdcsMagSunFltW;
            public float downAdcsMagSunFltNormP;                       //4

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]    //16
            public float[] downAdcsMagGyroFltQ;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]    //12
            public float[] downAdcsMagGyroFltW;
            public float downAdcsMagGyroFltNormP;                       //4



            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]    //12
            public float[] downAdcsOrbPos;            // 位置downAdcsOrb[0-2]  速度downAdcsOrb[3-5]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]    //6
            public Int16[] downAdcsOrbVel;          //取整


            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]   //20
            public Int16[] adc;


        }


        public static void get_info_from_obc_buf(byte[] down_buf, ref down_obc_ST down_info)
        {

            byte[] obc_buf = new byte[obc_length];

            for (int kc = 0; kc < obc_length; kc++)
                obc_buf[kc] = down_buf[kc];


            down_info = (down_obc_ST)BytesToStuct(obc_buf, down_info.GetType());
        }

        public static void get_info_from_adcs_buf(byte[] down_buf, ref down_adcs_ST down_info)
        {

            byte[] adcs_buf = new byte[adcs_length];

            for (int kc = 0; kc < adcs_length; kc++)
                adcs_buf[kc] = down_buf[kc];


            down_info = (down_adcs_ST)BytesToStuct(adcs_buf, down_info.GetType());
        }
        #endregion




        #region 星务上行命令帧

        [StructLayout(LayoutKind.Sequential,Pack =1,Size = ctrl_length)]
        public struct UP_ctrl_cmd
        {
            public UInt32 id;
            public byte len;
            public byte pid;
            public byte func;
            public UInt32 delay_time;
            public UInt32 CRC;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, Size = para_length)]
        public struct UP_para_cmd
        {
           public UInt32    id;
            public byte len;
            public byte      pid;
           public byte      func;
           public UInt32    delay_time;
           public UInt32    data;
           public UInt32    resvd;
           public UInt32    CRC;
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1, Size = orbit_length)]
        public struct UP_orbit_cmd
        {

              public  UInt32 id;
              public byte len;
              public  byte pid;
              public  byte func;
              public  UInt32 delay_time;
              [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
              public  double[] data;
              public  UInt32 CRC;
         }


        #endregion


  
    

        #region 上行序列化函数


        public static void generate_up_ctrl_cmd_cs(byte[] up_ctrl_buf, byte pid, byte func, UInt32 delay_time)
        {
            UP_ctrl_cmd ctrl_cmd = new UP_ctrl_cmd();

            byte    flags = 0,sport = 1,dport = 18,
                    dst = 26,src = 1,pri = 0;

            ctrl_cmd.id = generate_csp_header(flags, sport, dport, dst, src, pri);

            ctrl_cmd.len = ctrl_length;
            ctrl_cmd.func = func;
            ctrl_cmd.pid = pid;
            ctrl_cmd.delay_time = delay_time;


            ctrl_cmd.id = hton32(ctrl_cmd.id);

            byte[] up_ctrl_c = StructToBytes((UP_ctrl_cmd)ctrl_cmd);
            
            //byte[] up_ctrl_tosend = new byte[up_ctrl_c.Length];
            //TC_encode(up_ctrl_c, ctrl_length - 8, ref up_ctrl_tosend);

            for (int kc = 0; kc < ctrl_length; kc++)
                up_ctrl_buf[kc] = up_ctrl_c[kc];



        }


        public static void generate_up_para_cmd_cs(byte[] up_para_buf, byte pid, byte func, UInt32 delay_time, UInt32 para, UInt32 para2)
        {
            UP_para_cmd para_cmd = new UP_para_cmd();

            byte flags = 0,
                sport = 1,
                dport = 18,
                dst = 26,
                src = 1,
                pri = 0;

            para_cmd.id = generate_csp_header(flags, sport, dport, dst, src, pri);

            para_cmd.len = para_length;
            para_cmd.func = func;
            para_cmd.pid = pid;
            para_cmd.delay_time = delay_time;

            para_cmd.data = para;
            para_cmd.resvd = para2;


            para_cmd.id = hton32(para_cmd.id);

            byte[] up_ctrl_c = StructToBytes((UP_para_cmd)para_cmd);

            for (int kc = 0; kc < para_length; kc++)
                up_para_buf[kc] = up_ctrl_c[kc];

        }

        public static void generate_up_orbit_cmd_cs(byte[] up_orbit_buf, byte pid, byte func, UInt32 delay_time, double[] orbit_para)
		{
            UP_orbit_cmd orbit_cmd = new UP_orbit_cmd();
          
            byte flags = 0,
                sport = 1,
                dport = 18,
                dst = 26,
                src = 1,
                pri = 0;

            orbit_cmd.id = generate_csp_header(flags, sport, dport, dst, src, pri);


            orbit_cmd.len = orbit_length;
            orbit_cmd.func = func;
            orbit_cmd.pid = pid;
            orbit_cmd.delay_time = delay_time;

            for (int kc = 0; kc < 8; kc++)
                orbit_cmd.data[kc] = orbit_para[kc];


            orbit_cmd.id = hton32(orbit_cmd.id);

            byte[] up_orbit_c = StructToBytes((UP_orbit_cmd)orbit_cmd);


            for (int kc = 0; kc < orbit_length; kc++)
                up_orbit_buf[kc] = up_orbit_c[kc];
        }

        private static UInt32 generate_csp_header(byte flags, byte sport, byte dport, byte dst, byte src, byte pri)
        {
            UInt32 header = 0;

            header = (UInt32)((pri & 0x03) << 31) +
            (UInt32)((src & 0x1F) << 25) +
            (UInt32)((dst & 0x1F) << 20) +
            (UInt32)((dport & 0x3F) << 14) +
            (UInt32)((sport & 0x3F) << 8) +
            (UInt32)(flags << 0)
             ;
            return header;
        }

        private static UInt32 hton32(UInt32 h32)
        {
            return (((h32 & 0xff000000) >> 24) |
                ((h32 & 0x000000ff) << 24) |
                ((h32 & 0x0000ff00) << 8) |
                ((h32 & 0x00ff0000) >> 8));
        }


        #endregion



        #region 通用转换函数


        private static object BytesToStuct(byte[] bytes, Type type)
        {
            //得到结构体的大小
            int size = Marshal.SizeOf(type);
            //byte数组长度小于结构体的大小
            if (size > bytes.Length)
            {
                //返回空
                return null;
            }
            //分配结构体大小的内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //将byte数组拷到分配好的内存空间
            Marshal.Copy(bytes, 0, structPtr, size);
            //将内存空间转换为目标结构体
            object obj = Marshal.PtrToStructure(structPtr, type);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            //返回结构体
            return obj;
        }
        private static byte[] StructToBytes(object structObj)
       {
            //得到结构体的大小
            int size = Marshal.SizeOf(structObj);
            //创建byte数组
            byte[] bytes = new byte[size];
            //分配结构体大小的内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //将结构体拷到分配好的内存空间
            Marshal.StructureToPtr(structObj, structPtr, false);
            //从内存空间拷到byte数组
            Marshal.Copy(structPtr, bytes, 0, size);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            //返回byte数组
            return bytes;
        }


        /// <summary>
        /// 结构体转字节数组（按大端模式）
        /// </summary>
        /// <param name="obj">struct type</param>
        /// <returns></returns>
        private static byte[] StructureToByteArrayEndian(object obj)
        {
            object thisBoxed = obj;   //copy ，将 struct 装箱
            Type test = thisBoxed.GetType();

            int offset = 0;
            byte[] data = new byte[Marshal.SizeOf(thisBoxed)];

            object fieldValue;
            TypeCode typeCode;
            byte[] temp;
            // 列举结构体的每个成员，并Reverse
            foreach (var field in test.GetFields())
            {
                fieldValue = field.GetValue(thisBoxed); // Get value

                typeCode = Type.GetTypeCode(fieldValue.GetType());  // get type

                switch (typeCode)
                {
                    case TypeCode.Single: // float
                        {
                            temp = BitConverter.GetBytes((Single)fieldValue);
                            Array.Reverse(temp);
                            Array.Copy(temp, 0, data, offset, sizeof(Single));
                            break;
                        }
                    case TypeCode.Int32:
                        {
                            temp = BitConverter.GetBytes((Int32)fieldValue);
                            Array.Reverse(temp);
                            Array.Copy(temp, 0, data, offset, sizeof(Int32));
                            break;
                        }
                    case TypeCode.UInt32:
                        {
                            temp = BitConverter.GetBytes((UInt32)fieldValue);
                            Array.Reverse(temp);
                            Array.Copy(temp, 0, data, offset, sizeof(UInt32));
                            break;
                        }
                    case TypeCode.Int16:
                        {
                            temp = BitConverter.GetBytes((Int16)fieldValue);
                            Array.Reverse(temp);
                            Array.Copy(temp, 0, data, offset, sizeof(Int16));
                            break;
                        }
                    case TypeCode.UInt16:
                        {
                            temp = BitConverter.GetBytes((UInt16)fieldValue);
                            Array.Reverse(temp);
                            Array.Copy(temp, 0, data, offset, sizeof(UInt16));
                            break;
                        }
                    case TypeCode.Int64:
                        {
                            temp = BitConverter.GetBytes((Int64)fieldValue);
                            Array.Reverse(temp);
                            Array.Copy(temp, 0, data, offset, sizeof(Int64));
                            break;
                        }
                    case TypeCode.UInt64:
                        {
                            temp = BitConverter.GetBytes((UInt64)fieldValue);
                            Array.Reverse(temp);
                            Array.Copy(temp, 0, data, offset, sizeof(UInt64));
                            break;
                        }
                    case TypeCode.Double:
                        {
                            temp = BitConverter.GetBytes((Double)fieldValue);
                            Array.Reverse(temp);
                            Array.Copy(temp, 0, data, offset, sizeof(Double));
                            break;
                        }
                    case TypeCode.Byte:
                        {
                            data[offset] = (Byte)fieldValue;
                            break;
                        }
                    default:
                        {
                            //System.Diagnostics.Debug.Fail("No conversion provided for this type : " + typeCode.ToString());
                            break;
                        }
                }; // switch
                if (typeCode == TypeCode.Object)
                {
                    int length = ((byte[])fieldValue).Length;
                    Array.Copy(((byte[])fieldValue), 0, data, offset, length);
                    offset += length;
                }
                else
                {
                    offset += Marshal.SizeOf(fieldValue);
                }
            } // foreach

            return data;
        } // Swap

       
        
        /// <summary>
        /// 字节数组转结构体(按大端模式)
        /// </summary>
        /// <param name="bytearray">字节数组</param>
        /// <param name="obj">目标结构体</param>
        /// <param name="startoffset">bytearray内的起始位置</param>
        public static void ByteArrayToStructureEndian(byte[] bytearray, ref object obj, int startoffset)
        {
            int len = Marshal.SizeOf(obj);
            IntPtr i = Marshal.AllocHGlobal(len);
            byte[] temparray = (byte[])bytearray.Clone();
            // 从结构体指针构造结构体
            obj = Marshal.PtrToStructure(i, obj.GetType());
            // 做大端转换
            object thisBoxed = obj;
            Type test = thisBoxed.GetType();
            int reversestartoffset = startoffset;
            // 列举结构体的每个成员，并Reverse
            foreach (var field in test.GetFields())
            {
                object fieldValue = field.GetValue(thisBoxed); // Get value
                
                TypeCode typeCode = Type.GetTypeCode(fieldValue.GetType());  //Get Type
                if (typeCode != TypeCode.Object)  //如果为值类型
                {
                    Array.Reverse(temparray, reversestartoffset, Marshal.SizeOf(fieldValue));
                    reversestartoffset += Marshal.SizeOf(fieldValue);
                }
                else  //如果为引用类型
                {
                    reversestartoffset += ((byte[])fieldValue).Length;
                }
            }
            try
            {
                //将字节数组复制到结构体指针
                Marshal.Copy(temparray, startoffset, i, len);
            }
            catch (Exception ex) { Console.WriteLine("ByteArrayToStructure FAIL: error " + ex.ToString()); }
            obj = Marshal.PtrToStructure(i, obj.GetType());
            Marshal.FreeHGlobal(i);  //释放内存
        }
#endregion

    }
}
