using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace satCompent.Helper
{
    public class ByteHelper
    {
        /// <summary>
        /// 获取字节的指定位的值，算法的思想是:先对这个字节从high位至第7位处理为0,然后对这个处理后的字节逻辑右移low位.
        /// </summary>
        /// <param name="bt"></param>
        /// <param name="low"></param>
        /// <param name="high"></param>
        /// <returns></returns>
        public static byte GetBit(byte by, int low, int high)
        {
            byte bits = by;  //初始化为整个字节
            if (low < 0 || high > 7 || low > high)
            {
                return 0;
            }

            for (int i = high + 1; i < 8; i++)
            {
                switch (i)
                {
                    case 0:  //将字节的第一位置0
                        bits &= 0xFE;
                        break;
                    case 1:  //将字节的第二位置0
                        bits &= 0xFD;
                        break;
                    case 2:  //将字节的第三位置0
                        bits &= 0xFB;
                        break;
                    case 3:  //将字节的第四位置0
                        bits &= 0xF7;
                        break;
                    case 4:  //将字节的第五位置0
                        bits &= 0xEF;
                        break;
                    case 5:  //将字节的第六位置0
                        bits &= 0xDF;
                        break;
                    case 6:  //将字节的第七位置0
                        bits &= 0xBF;
                        break;
                    case 7:  //将字节的第八位置0
                        bits &= 0x7F;
                        break;
                    default:
                        break;
                }
            }

            bits >>= low;  //将字节右移low位

            return bits;
        }


        //获取指定位是否为1
        public static bool GetBit(byte val, int index)
        {
            return (val & (1 << index)) > 0;
        }

        //将指定位设为1
        public static byte SetBit(byte val, int index)
        {
            val |= (byte)(1 << index);

            return val;
        }

        //将指定位设为0
        public static byte ClearBit(byte val, int index)
        {
            val &= (byte)((1 << 8) - 1 - (1 << index));

            return val;
        }

        //将指定位取反
        public static byte ReverseBit(byte val, int index)
        {
            val ^= (byte)(1 << index);

            return val;
        }

        //将1个字节的高低位反转
        public static byte ReverseByte(byte val)
        {
            byte reverse = 0x0;  //初始化为0x0

            for (int index = 0; index < 8; index++)
            {
                reverse = Convert.ToByte(((val >> index) & 0x01) | reverse);
                if (index < 7)
                {
                    reverse <<= 1;
                }
            }

            return reverse;
        }

        //将1个字节的高低位反转
        public static byte InvertByte(byte val)
        {
            byte[] cons = new byte[16] { 0x0, 0x8, 0x4, 0xC, 0x2, 0xA, 0x6, 0xE, 0x1, 0x9, 0x5, 0xD, 0x3, 0xB, 0x7, 0xF };
            byte invert = 0x0;  //初始化为0x0

            invert |= Convert.ToByte((cons[val & 0xF]) << 4);
            invert |= cons[val >> 4];

            return invert;
        }

        //将1个字节的高低位反转：蝶式交换
        public static byte ShiftByte(byte val)
        {
            //假设原始的数据位序列: 7 6 5 4  3 2 1 0
            val = Convert.ToByte(((val << 4) | (val >> 4)) & 0xFF);  //交换数据位序列: 3 2 1 0  7 6 5 4
            val = Convert.ToByte((((val << 2) & 0xcc) | ((val >> 2) & 0x33)) & 0xFF);  //交换数据位序列: 1 0 3 2  5 4 7 6
            val = Convert.ToByte((((val << 1) & 0xaa) | ((val >> 1) & 0x55)) & 0xFF);  //交换数据位序列: 0 1 2 3  4 5 6 7

            return val;
        }

        //(8)位->字节
        public static byte Bit2Byte(byte[] bits)
        {
            byte val = 0;

            for (int index = 0; index < 8; index++)
            {
                //通过或的方式累计求和  
                val |= Convert.ToByte((bits[index]) << index);
            }

            return val;
        }

        //字节->(8)位
        public static byte[] Byte2Bit(byte val)
        {
            byte[] bytes = new byte[8];

            for (int index = 0; index < 8; index++)
            {
                bytes[7 - index] = Convert.ToByte((val >> index) & 0x01);
            }

            return bytes;
        }

        //字节数组->16进制字符串
        public static string Bytes2HexStr(byte[] bytes)
        {
            string hexStr = string.Empty;

            if (bytes != null)
            {
                StringBuilder strBuilder = new StringBuilder();

                for (int index = 0; index < bytes.Length; index++)
                {
                    strBuilder.Append(bytes[index].ToString("X2") + " ");
                    //strBuilder.Append(Convert.ToString(bytes[index], 16).PadLeft(2, '0').PadRight(3, ' '));
                }
                hexStr = strBuilder.ToString();
            }
            return hexStr;
        }

        //字节数组->16进制字符串
        public static string Bytes2HexStr(byte[] bytes, int length)
        {
            string hexStr = string.Empty;

            if (bytes != null && length > 0)
            {
                StringBuilder strBuilder = new StringBuilder();

                for (int index = 0; index < length; index++)
                {
                    strBuilder.Append(bytes[index].ToString("X2") + " ");
                    //strBuilder.Append(Convert.ToString(bytes[index], 16).PadLeft(2, '0').PadRight(3, ' '));
                }
                hexStr = strBuilder.ToString();
            }
            return hexStr;
        }

        //16进制字符串->字节数组
        public static byte[] HexStr2Bytes(string hexStr)
        {
            hexStr = hexStr.Replace(" ", "");
            if (hexStr.Length % 2 != 0)  //如果最后一多个字符，则删除
            {
                hexStr = hexStr.Substring(0, hexStr.Length - 1);
            }

            byte[] bytes = new byte[hexStr.Length / 2];

            for (int index = 0; index < hexStr.Length; index += 2)
            {
                bytes[index / 2] = Convert.ToByte(hexStr.Substring(index, 2), 16);
            }

            return bytes;
        }

        //二进制方式将object对象序列化到字节数组中
        public static byte[] SerializeByBinary(Object obj)
        {
            byte[] buffer = null;

            try
            {
                var memoryStream = new MemoryStream();
                var formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, obj);
                byte[] buf = memoryStream.ToArray();
                memoryStream.Close();
                buffer = buf;
            }
            catch
            {
                buffer = null;
            }

            return buffer;
        }

        //二进制方式字节数组中数据还原为对象
        public static object DeSerializeByBinary(byte[] value)
        {
            object obj;
            try
            {
                var memoryStream = new MemoryStream(value);
                var formatter = new BinaryFormatter();
                object o = formatter.Deserialize(memoryStream);
                memoryStream.Close();
                obj = o;
            }
            catch
            {
                obj = null;
            }
            return obj;
        }

        /// <summary>
        /// 将一个字节数组转换为8bit灰度位图
        /// </summary>
        /// <param name="rawValues">显示字节数组</param>
        /// <param name="width">图像宽度</param>
        /// <param name="height">图像高度</param>
        /// <returns>位图</returns>
        public static Bitmap ToGrayBitmap(byte[] rawValues, int width, int height)
        {
            //// 申请目标位图的变量，并将其内存区域锁定
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

            //// 获取图像参数
            int stride = bmpData.Stride;  // 扫描线的宽度
            int offset = stride - width;  // 显示宽度与扫描线宽度的间隙
            IntPtr iptr = bmpData.Scan0;  // 获取bmpData的内存起始位置
            int scanBytes = stride * height;   // 用stride宽度，表示这是内存区域的大小

            //// 下面把原始的显示大小字节数组转换为内存中实际存放的字节数组
            int posScan = 0, posReal = 0;   // 分别设置两个位置指针，指向源数组和目标数组
            byte[] pixelValues = new byte[scanBytes];  //为目标数组分配内存
            for (int x = 0; x < height; x++)
            {
                //// 下面的循环节是模拟行扫描
                for (int y = 0; y < width; y++)
                {
                    pixelValues[posScan++] = rawValues[posReal++];
                }
                posScan += offset;  //行扫描结束，要将目标位置指针移过那段“间隙”
            }

            //// 用Marshal的Copy方法，将刚才得到的内存字节数组复制到BitmapData中
            System.Runtime.InteropServices.Marshal.Copy(pixelValues, 0, iptr, scanBytes);
            bmp.UnlockBits(bmpData);  // 解锁内存区域

            //// 下面的代码是为了修改生成位图的索引表，从伪彩修改为灰度
            ColorPalette tempPalette;
            using (Bitmap tempBmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed))
            {
                tempPalette = tempBmp.Palette;
            }
            for (int i = 0; i < 256; i++)
            {
                tempPalette.Entries[i] = Color.FromArgb(i, i, i);
            }

            bmp.Palette = tempPalette;

            return bmp;
        }

        //CRC16校验，低位先传，高位后传/////////////////////////////////////////////
        public static readonly ushort[] crc16_ccitt_table =
        {
            0x0000, 0x1189, 0x2312, 0x329b, 0x4624, 0x57ad, 0x6536, 0x74bf,
            0x8c48, 0x9dc1, 0xaf5a, 0xbed3, 0xca6c, 0xdbe5, 0xe97e, 0xf8f7,
            0x1081, 0x0108, 0x3393, 0x221a, 0x56a5, 0x472c, 0x75b7, 0x643e,
            0x9cc9, 0x8d40, 0xbfdb, 0xae52, 0xdaed, 0xcb64, 0xf9ff, 0xe876,
            0x2102, 0x308b, 0x0210, 0x1399, 0x6726, 0x76af, 0x4434, 0x55bd,
            0xad4a, 0xbcc3, 0x8e58, 0x9fd1, 0xeb6e, 0xfae7, 0xc87c, 0xd9f5,
            0x3183, 0x200a, 0x1291, 0x0318, 0x77a7, 0x662e, 0x54b5, 0x453c,
            0xbdcb, 0xac42, 0x9ed9, 0x8f50, 0xfbef, 0xea66, 0xd8fd, 0xc974,
            0x4204, 0x538d, 0x6116, 0x709f, 0x0420, 0x15a9, 0x2732, 0x36bb,
            0xce4c, 0xdfc5, 0xed5e, 0xfcd7, 0x8868, 0x99e1, 0xab7a, 0xbaf3,
            0x5285, 0x430c, 0x7197, 0x601e, 0x14a1, 0x0528, 0x37b3, 0x263a,
            0xdecd, 0xcf44, 0xfddf, 0xec56, 0x98e9, 0x8960, 0xbbfb, 0xaa72,
            0x6306, 0x728f, 0x4014, 0x519d, 0x2522, 0x34ab, 0x0630, 0x17b9,
            0xef4e, 0xfec7, 0xcc5c, 0xddd5, 0xa96a, 0xb8e3, 0x8a78, 0x9bf1,
            0x7387, 0x620e, 0x5095, 0x411c, 0x35a3, 0x242a, 0x16b1, 0x0738,
            0xffcf, 0xee46, 0xdcdd, 0xcd54, 0xb9eb, 0xa862, 0x9af9, 0x8b70,
            0x8408, 0x9581, 0xa71a, 0xb693, 0xc22c, 0xd3a5, 0xe13e, 0xf0b7,
            0x0840, 0x19c9, 0x2b52, 0x3adb, 0x4e64, 0x5fed, 0x6d76, 0x7cff,
            0x9489, 0x8500, 0xb79b, 0xa612, 0xd2ad, 0xc324, 0xf1bf, 0xe036,
            0x18c1, 0x0948, 0x3bd3, 0x2a5a, 0x5ee5, 0x4f6c, 0x7df7, 0x6c7e,
            0xa50a, 0xb483, 0x8618, 0x9791, 0xe32e, 0xf2a7, 0xc03c, 0xd1b5,
            0x2942, 0x38cb, 0x0a50, 0x1bd9, 0x6f66, 0x7eef, 0x4c74, 0x5dfd,
            0xb58b, 0xa402, 0x9699, 0x8710, 0xf3af, 0xe226, 0xd0bd, 0xc134,
            0x39c3, 0x284a, 0x1ad1, 0x0b58, 0x7fe7, 0x6e6e, 0x5cf5, 0x4d7c,
            0xc60c, 0xd785, 0xe51e, 0xf497, 0x8028, 0x91a1, 0xa33a, 0xb2b3,
            0x4a44, 0x5bcd, 0x6956, 0x78df, 0x0c60, 0x1de9, 0x2f72, 0x3efb,
            0xd68d, 0xc704, 0xf59f, 0xe416, 0x90a9, 0x8120, 0xb3bb, 0xa232,
            0x5ac5, 0x4b4c, 0x79d7, 0x685e, 0x1ce1, 0x0d68, 0x3ff3, 0x2e7a,
            0xe70e, 0xf687, 0xc41c, 0xd595, 0xa12a, 0xb0a3, 0x8238, 0x93b1,
            0x6b46, 0x7acf, 0x4854, 0x59dd, 0x2d62, 0x3ceb, 0x0e70, 0x1ff9,
            0xf78f, 0xe606, 0xd49d, 0xc514, 0xb1ab, 0xa022, 0x92b9, 0x8330,
            0x7bc7, 0x6a4e, 0x58d5, 0x495c, 0x3de3, 0x2c6a, 0x1ef1, 0x0f78,
        };

        //计算CRC16
        public static ushort cal_crc(byte[] data, int len)
        {
            ushort crc = 0x0000;    //初始化
            int index = 0;

            while (len > 0)
            {
                crc = Convert.ToUInt16((crc >> 8) ^ crc16_ccitt_table[(crc ^ data[index]) & 0xff]);
                len--;
                index++;
            }

            return crc;
        }

        //CRC16校验
        public static bool do_crc(byte[] data, int length)
        {
            ushort crc = 0x0000;    //初始化
            int len = length - 2;
            int index = 0;

            while (len > 0)
            {
                crc = Convert.ToUInt16((crc >> 8) ^ crc16_ccitt_table[(crc ^ data[index]) & 0xff]);
                len--;
                index++;
            }
            return (Convert.ToByte(crc & 0xff) == data[length - 2]) && (Convert.ToByte((crc & 0xff00) >> 8) == data[length - 1]);
        }


        //CRC16查找表高字节
        private static readonly byte[] CRC16TABLE_HI =
        {
	        0x00, 0x11, 0x23, 0x32, 0x46, 0x57, 0x65, 0x74, 0x8C, 0x9D, 0xAF, 0xBE, 0xCA, 0xDB, 0xE9, 0xF8,
	        0x10, 0x01, 0x33, 0x22, 0x56, 0x47, 0x75, 0x64, 0x9C, 0x8D, 0xBF, 0xAE, 0xDA, 0xCB, 0xF9, 0xE8,
	        0x21, 0x30, 0x02, 0x13, 0x67, 0x76, 0x44, 0x55, 0xAD, 0xBC, 0x8E, 0x9F, 0xEB, 0xFA, 0xC8, 0xD9,
	        0x31, 0x20, 0x12, 0x03, 0x77, 0x66, 0x54, 0x45, 0xBD, 0xAC, 0x9E, 0x8F, 0xFB, 0xEA, 0xD8, 0xC9,
	        0x42, 0x53, 0x61, 0x70, 0x04, 0x15, 0x27, 0x36, 0xCE, 0xDF, 0xED, 0xFC, 0x88, 0x99, 0xAB, 0xBA,
	        0x52, 0x43, 0x71, 0x60, 0x14, 0x05, 0x37, 0x26, 0xDE, 0xCF, 0xFD, 0xEC, 0x98, 0x89, 0xBB, 0xAA,
	        0x63, 0x72, 0x40, 0x51, 0x25, 0x34, 0x06, 0x17, 0xEF, 0xFE, 0xCC, 0xDD, 0xA9, 0xB8, 0x8A, 0x9B,
	        0x73, 0x62, 0x50, 0x41, 0x35, 0x24, 0x16, 0x07, 0xFF, 0xEE, 0xDC, 0xCD, 0xB9, 0xA8, 0x9A, 0x8B,
	        0x84, 0x95, 0xA7, 0xB6, 0xC2, 0xD3, 0xE1, 0xF0, 0x08, 0x19, 0x2B, 0x3A, 0x4E, 0x5F, 0x6D, 0x7C,
	        0x94, 0x85, 0xB7, 0xA6, 0xD2, 0xC3, 0xF1, 0xE0, 0x18, 0x09, 0x3B, 0x2A, 0x5E, 0x4F, 0x7D, 0x6C,
	        0xA5, 0xB4, 0x86, 0x97, 0xE3, 0xF2, 0xC0, 0xD1, 0x29, 0x38, 0x0A, 0x1B, 0x6F, 0x7E, 0x4C, 0x5D,
	        0xB5, 0xA4, 0x96, 0x87, 0xF3, 0xE2, 0xD0, 0xC1, 0x39, 0x28, 0x1A, 0x0B, 0x7F, 0x6E, 0x5C, 0x4D,
	        0xC6, 0xD7, 0xE5, 0xF4, 0x80, 0x91, 0xA3, 0xB2, 0x4A, 0x5B, 0x69, 0x78, 0x0C, 0x1D, 0x2F, 0x3E,
	        0xD6, 0xC7, 0xF5, 0xE4, 0x90, 0x81, 0xB3, 0xA2, 0x5A, 0x4B, 0x79, 0x68, 0x1C, 0x0D, 0x3F, 0x2E,
	        0xE7, 0xF6, 0xC4, 0xD5, 0xA1, 0xB0, 0x82, 0x93, 0x6B, 0x7A, 0x48, 0x59, 0x2D, 0x3C, 0x0E, 0x1F,
	        0xF7, 0xE6, 0xD4, 0xC5, 0xB1, 0xA0, 0x92, 0x83, 0x7B, 0x6A, 0x58, 0x49, 0x3D, 0x2C, 0x1E, 0x0F
        };

        //CRC16查找表低字节
        private static readonly byte[] CRC16TABLE_LO = 
        {
	        0x00, 0x89, 0x12, 0x9B, 0x24, 0xAD, 0x36, 0xBF, 0x48, 0xC1, 0x5A, 0xD3, 0x6C, 0xE5, 0x7E, 0xF7,
	        0x81, 0x08, 0x93, 0x1A, 0xA5, 0x2C, 0xB7, 0x3E, 0xC9, 0x40, 0xDB, 0x52, 0xED, 0x64, 0xFF, 0x76,
	        0x02, 0x8B, 0x10, 0x99, 0x26, 0xAF, 0x34, 0xBD, 0x4A, 0xC3, 0x58, 0xD1, 0x6E, 0xE7, 0x7C, 0xF5,
	        0x83, 0x0A, 0x91, 0x18, 0xA7, 0x2E, 0xB5, 0x3C, 0xCB, 0x42, 0xD9, 0x50, 0xEF, 0x66, 0xFD, 0x74,
	        0x04, 0x8D, 0x16, 0x9F, 0x20, 0xA9, 0x32, 0xBB, 0x4C, 0xC5, 0x5E, 0xD7, 0x68, 0xE1, 0x7A, 0xF3,
	        0x85, 0x0C, 0x97, 0x1E, 0xA1, 0x28, 0xB3, 0x3A, 0xCD, 0x44, 0xDF, 0x56, 0xE9, 0x60, 0xFB, 0x72,
	        0x06, 0x8F, 0x14, 0x9D, 0x22, 0xAB, 0x30, 0xB9, 0x4E, 0xC7, 0x5C, 0xD5, 0x6A, 0xE3, 0x78, 0xF1,
	        0x87, 0x0E, 0x95, 0x1C, 0xA3, 0x2A, 0xB1, 0x38, 0xCF, 0x46, 0xDD, 0x54, 0xEB, 0x62, 0xF9, 0x70,
	        0x08, 0x81, 0x1A, 0x93, 0x2C, 0xA5, 0x3E, 0xB7, 0x40, 0xC9, 0x52, 0xDB, 0x64, 0xED, 0x76, 0xFF,
	        0x89, 0x00, 0x9B, 0x12, 0xAD, 0x24, 0xBF, 0x36, 0xC1, 0x48, 0xD3, 0x5A, 0xE5, 0x6C, 0xF7, 0x7E,
	        0x0A, 0x83, 0x18, 0x91, 0x2E, 0xA7, 0x3C, 0xB5, 0x42, 0xCB, 0x50, 0xD9, 0x66, 0xEF, 0x74, 0xFD,
	        0x8B, 0x02, 0x99, 0x10, 0xAF, 0x26, 0xBD, 0x34, 0xC3, 0x4A, 0xD1, 0x58, 0xE7, 0x6E, 0xF5, 0x7C,
	        0x0C, 0x85, 0x1E, 0x97, 0x28, 0xA1, 0x3A, 0xB3, 0x44, 0xCD, 0x56, 0xDF, 0x60, 0xE9, 0x72, 0xFB,
	        0x8D, 0x04, 0x9F, 0x16, 0xA9, 0x20, 0xBB, 0x32, 0xC5, 0x4C, 0xD7, 0x5E, 0xE1, 0x68, 0xF3, 0x7A,
	        0x0E, 0x87, 0x1C, 0x95, 0x2A, 0xA3, 0x38, 0xB1, 0x46, 0xCF, 0x54, 0xDD, 0x62, 0xEB, 0x70, 0xF9,
	        0x8F, 0x06, 0x9D, 0x14, 0xAB, 0x22, 0xB9, 0x30, 0xC7, 0x4E, 0xD5, 0x5C, 0xE3, 0x6A, 0xF1, 0x78
        };

        //计算给定长度数据的16位CRC
        public static int GetCrc16(byte[] data)
        {
            int High = 0xFF;  // 高字节
            int Low = 0xFF;   // 低字节

            if (data != null)
            {
                foreach (byte b in data)
                {
                    int Index = Low ^ b;
                    Low = High ^ CRC16TABLE_LO[Index];
                    High = CRC16TABLE_HI[Index];
                }
            }

            return ~((High << 8) + Low);    // 取反
        }

        //检查给定长度数据的16位CRC是否正确
        /// <reamrks>
        /// 字节数组最后2个字节为校验码，且低字节在前面，高字节在后面
        /// </reamrks>
        public static bool IsCrc16Good(byte[] data, int length)
        {
            int High = 0xFF;
            int Low = 0xFF;
            if (data != null)
            {
                //foreach (byte b in data)
                for (int i = 0; i < length; i++)
                {
                    int Index = Low ^ data[i];
                    Low = High ^ CRC16TABLE_LO[Index];
                    High = CRC16TABLE_HI[Index];
                }
            }

            return (High == 0xF0 && Low == 0xB8);
        }

        /// <summary>
        /// 累加和校验
        /// </summary>
        /// <param name="bs"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static byte SumCheck(byte[] bs, int offset, int len)
        {
            int sum = 0;

            //字节累加
            for (int i = offset; i < offset + len; i++)
            {
                sum = (sum + bs[i]) % 0xFFFF;
            }

            return Convert.ToByte(sum & 0xFF);  //最后1个字节
        }

        /// <summary>
        /// 累加和校验
        /// </summary>
        /// <param name="bs"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static ushort SumCheck2(byte[] bs, int offset, int len)
        {
            int sum = 0;

            //双字节字节累加
            for (int i = offset; i < offset + len; i += 2)
            {
                sum = (sum + (bs[i] << 8 | bs[i + 1])) % 0xFFFF;
            }

            return Convert.ToUInt16(sum & 0xFFFF);  //最后2个字节
        }
    }
}
