using System;
using System.IO;
using System.Diagnostics;

namespace DataIO
{
    public class IOFuctions
    {
        private DirectoryInfo File_PATH;

        private StreamWriter File_FrameASCII;                       ///实时数据存储文件

        /// <summary>
        /// 创建保存数据文件
        /// </summary>
        public void CreateFrameFile()
        {
            string PATH = Directory.GetCurrentDirectory();
            string time_day = DateTime.Now.ToString("yyyy-MM-dd");
            File_PATH = new DirectoryInfo(PATH + "\\遥测数据存储\\HK_DAT'" + time_day);
            File_PATH.Create();

            string time_now = "\\HK_Dat_ASCII" + 
                 '-' + DateTime.Now.ToLongTimeString().ToString().Replace(':', '-') + ".dat";
            File_FrameASCII = new StreamWriter(File_PATH + time_now);
        }


        public void WriteFrameFile(byte[] buf)
        {
            try
            {
                foreach(byte Buf in buf)
                    File_FrameASCII.Write(Buf.ToString("X2")+"   ");
                File_FrameASCII.Write('\n');
            }    
            catch (Exception e)
            {
                Trace.WriteLine("ADCS文件IO错误:" + e.Message + e.StackTrace);
                File_FrameASCII.Close();
            }

        }

        /// <summary>
        /// 关闭保存数据文件
        /// </summary>
        public void CloseFile()
        {
            if (File_FrameASCII != null)
                File_FrameASCII.Close();

        }
    }
}
