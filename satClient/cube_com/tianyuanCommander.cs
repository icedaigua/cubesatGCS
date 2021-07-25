using System;
using System.Runtime.InteropServices;

namespace satMsg
{
    public class tianyuanCommander
    {

        #region 星务上行命令帧
        private const ushort CommanderLength = 64;
        public const byte ctrl_length = 15,
                            para_length = 23,
                            orbit_length = 79;
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


        public  void generate_up_ctrl_cmd_cs(byte[] up_ctrl_buf, byte pid, byte func, UInt32 delay_time)
        {
            UP_ctrl_cmd ctrl_cmd = new UP_ctrl_cmd();

            byte    flags = 0,sport = 1,dport = 18,
                    dst = 26,src = 1,pri = 0;

            //ctrl_cmd.id = generate_csp_header(flags, sport, dport, dst, src, pri);

            ctrl_cmd.len = ctrl_length;
            ctrl_cmd.func = func;
            ctrl_cmd.pid = pid;
            ctrl_cmd.delay_time = delay_time;


            //ctrl_cmd.id = hton32(ctrl_cmd.id);

            //byte[] up_ctrl_c = StructToBytes((UP_ctrl_cmd)ctrl_cmd);
            
            //byte[] up_ctrl_tosend = new byte[up_ctrl_c.Length];
            //TC_encode(up_ctrl_c, ctrl_length - 8, ref up_ctrl_tosend);

            //for (int kc = 0; kc < ctrl_length; kc++)
            //    up_ctrl_buf[kc] = up_ctrl_c[kc];



        }


        public  void generate_up_para_cmd_cs(byte[] up_para_buf, byte pid, byte func, UInt32 delay_time, UInt32 para, UInt32 para2)
        {
            UP_para_cmd para_cmd = new UP_para_cmd();

            byte flags = 0,
                sport = 1,
                dport = 18,
                dst = 26,
                src = 1,
                pri = 0;

            //para_cmd.id = generate_csp_header(flags, sport, dport, dst, src, pri);

            para_cmd.len = para_length;
            para_cmd.func = func;
            para_cmd.pid = pid;
            para_cmd.delay_time = delay_time;

            para_cmd.data = para;
            para_cmd.resvd = para2;


            //para_cmd.id = hton32(para_cmd.id);

            //byte[] up_ctrl_c = StructToBytes((UP_para_cmd)para_cmd);

            //for (int kc = 0; kc < para_length; kc++)
            //    up_para_buf[kc] = up_ctrl_c[kc];

        }

        public  void generate_up_orbit_cmd_cs(byte[] up_orbit_buf, byte pid, byte func, UInt32 delay_time, double[] orbit_para)
		{
            UP_orbit_cmd orbit_cmd = new UP_orbit_cmd();
          
            byte flags = 0,
                sport = 1,
                dport = 18,
                dst = 26,
                src = 1,
                pri = 0;

            //orbit_cmd.id = generate_csp_header(flags, sport, dport, dst, src, pri);


            orbit_cmd.len = orbit_length;
            orbit_cmd.func = func;
            orbit_cmd.pid = pid;
            orbit_cmd.delay_time = delay_time;

            for (int kc = 0; kc < 8; kc++)
                orbit_cmd.data[kc] = orbit_para[kc];


            //orbit_cmd.id = hton32(orbit_cmd.id);

            //byte[] up_orbit_c = StructToBytes((UP_orbit_cmd)orbit_cmd);
            //for (int kc = 0; kc < orbit_length; kc++)
            //    up_orbit_buf[kc] = up_orbit_c[kc];
        }


        #endregion


    }
}
