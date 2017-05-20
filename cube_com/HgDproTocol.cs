﻿using System;
using System.Text;

namespace CubeCOM
{
    public static class HgDproTocol
    {


        private static byte[] down_info_buf = new byte[512];
        private static byte down_info_buf_length = 0;

        private static byte rec_state = 0;

        private static void analysis_rec_buffer(byte[] buffer)
        {
          
            byte tmp_cnt = 0;
            byte[] valid_buff = new byte[buffer.Length];
            UInt16 valid_length = 0;

            for (int kd = 0; kd < buffer.Length - 1; kd++)
            {
                if ((buffer[kd] == 0xDB) && (buffer[kd + 1] == 0xDC))
                {
                    valid_buff[valid_length++] = 0xC0;
                    kd++;
                }

                else if ((buffer[kd] == 0xDB) && (buffer[kd + 1] == 0xDD))
                {
                    valid_buff[valid_length++] = 0xDB;
                    kd++;
                }

                else
                {
                    valid_buff[valid_length++] = buffer[kd];
                }

            }



            foreach (byte Buf in valid_buff)
            {
                switch (rec_state)
                {
                    case 0:

                        if ((Buf == 0xB8))
                        {
                            down_info_buf_length = 0;
                            down_info_buf[down_info_buf_length++] = Buf;
                            rec_state = 1;
                        }

                        else if (Buf == 0x34)
                        {
                            down_info_buf_length = 0;
                            down_info_buf[down_info_buf_length++] = Buf;
                            rec_state = 0xF1;
                        }

                        break;

                    case 1:

                        if ((Buf == 0x54))
                        {
                            rec_state = 2;
                            down_info_buf[down_info_buf_length++] = Buf;
                        }
                        else if (Buf == 0x64)
                        {

                            rec_state = 3;
                            down_info_buf[down_info_buf_length++] = Buf;
                        }

                        else
                        {
                            down_info_buf_length = 0;
                            rec_state = 0;

                        }

                        break;

                    #region 星务信息
                    case 0xF1:
                        {
                            if (Buf == 0x50)
                            {
                                rec_state = 0xF2;
                                down_info_buf[down_info_buf_length++] = Buf;
                            }
                            else
                            {
                                down_info_buf_length = 0;
                                rec_state = 0;

                            }

                            break;
                        }
                    case 0xF2:
                        {   //收取2个无用字节
                            //buf_tmp[tmp_cnt++] = Buf;
                            down_info_buf[down_info_buf_length++] = Buf;
                            if (tmp_cnt++ >= 1)
                                rec_state = 0xF3;
                            break;
                        }
                    case 0xF3:
                        if ((Buf == 0x1A))      //星务
                        {
                            down_info_buf[down_info_buf_length++] = Buf;
                            rec_state = 210;
                        }
                        else
                        {
                            rec_state = 0;
                            down_info_buf_length = 0;
                        }
                        break;
                    case 210:
                        {
                            if ((Buf == 0x50))      //星务
                            {
                                down_info_buf[down_info_buf_length++] = Buf;
                                rec_state = 211;
                            }
                            else if (Buf == 0x51)   //姿控
                            {
                                down_info_buf[down_info_buf_length++] = Buf;
                                rec_state = 212;
                            }

                            else if (Buf == 0x53)   //星务响应
                            {
                                down_info_buf[down_info_buf_length++] = Buf;
                                rec_state = 213;
                            }

                            else if (Buf == 0x56)   //音频帧
                            {
                                down_info_buf[down_info_buf_length++] = Buf;
                                rec_state = 216;
                            }

                            else
                            {
                                rec_state = 0;
                                down_info_buf_length = 0;
                            }

                            break;
                        }



                    case 212:
                        {
                            down_info_buf[down_info_buf_length++] = Buf;
                            if (down_info_buf_length >= cubeCOMM.adcs_length + 12)
                            {
                                //rec_down_info_count++;      //接收到的指令数加1
                                rec_state = 0;
                                //cubeCOMM.get_info_from_adcs_buf(down_info_buf, ref adcs_info);

                                //adcs_displayAndsave();

                            }
                            break;
                        }

                    case 213:
                        {
                            down_info_buf[down_info_buf_length++] = Buf;
                            if (down_info_buf_length >= 10)
                            {


                                rec_state = 0;

                            }
                            break;
                        }

                    case 216:
                        {
                            down_info_buf[down_info_buf_length++] = Buf;
                            if (down_info_buf_length >= 108)
                            {

                                rec_state = 0;
                            }
                            break;
                        }
                    #endregion

                    #region UV通信帧
                    case 2:     //收取两个无用字节
                        {
                            //buf_tmp[tmp_cnt++] = Buf;
                            down_info_buf[down_info_buf_length++] = Buf;
                            if (tmp_cnt++ >= 1)
                                rec_state = 20;
                            break;
                        }

                    case 20:
                        {

                            if (Buf == 0x1C)        //UV状态
                            {
                                down_info_buf[down_info_buf_length++] = Buf;
                                rec_state = 220;
                            }

                            else
                            {
                                rec_state = 0;
                                down_info_buf_length = 0;
                            }
                            break;
                        }



                    #region UV通信机状态信息
                    case 220:

                        if ((Buf == 0xA1))
                        {
                            down_info_buf[down_info_buf_length++] = Buf;
                            rec_state = 221;
                        }
                        else if (Buf == 0xA2)
                        {
                            down_info_buf[down_info_buf_length++] = Buf;
                            rec_state = 222;
                        }
                        else if (Buf == 0xA3)
                        {
                            down_info_buf[down_info_buf_length++] = Buf;
                            rec_state = 223;
                        }
                        else
                        {
                            rec_state = 0;
                            down_info_buf_length = 0;
                        }

                        break;


                    #endregion
                    #endregion



                    default:
                        break;
                }
            }
        }



        #region 哈工大UV通信机上行指令编码
        #region 编码表
        private static byte[] RandomSeq = new byte[256]
        {
            0xff, 0x7f, 0x3f, 0x9f, 0xcf, 0xe7, 0x73, 0x39, 0x9c, 0xce, 0x67, 0x33, 0x99, 0xcc, 0xe6, 0xf3,
            0x79, 0x3c, 0x9e, 0x4f, 0xa7, 0xd3, 0x69, 0xb4, 0x5a, 0x2d, 0x96, 0xcb, 0x65, 0xb2, 0x59, 0x2c,
            0x16, 0x8b, 0xc5, 0xe2, 0x71, 0xb8, 0x5c, 0x2e, 0x97, 0x4b, 0x25, 0x12, 0x09, 0x04, 0x82, 0xc1,
            0x60, 0xb0, 0xd8, 0xec, 0xf6, 0x7b, 0xbd, 0x5e, 0xaf, 0x57, 0xab, 0xd5, 0x6a, 0xb5, 0xda, 0x6d,
            0x36, 0x9b, 0x4d, 0x26, 0x13, 0x89, 0x44, 0x22, 0x91, 0x48, 0x24, 0x92, 0x49, 0xa4, 0xd2, 0xe9,
            0xf4, 0xfa, 0x7d, 0xbe, 0x5f, 0x2f, 0x17, 0x0b, 0x85, 0x42, 0x21, 0x90, 0xc8, 0x64, 0x32, 0x19,
            0x8c, 0x46, 0xa3, 0x51, 0xa8, 0xd4, 0xea, 0xf5, 0x7a, 0x3d, 0x1e, 0x0f, 0x07, 0x83, 0x41, 0x20,
            0x10, 0x88, 0xc4, 0x62, 0x31, 0x18, 0x0c, 0x06, 0x03, 0x01, 0x80, 0x40, 0xa0, 0x50, 0x28, 0x94,
            0x4a, 0xa5, 0x52, 0xa9, 0x54, 0xaa, 0x55, 0x2a, 0x15, 0x8a, 0x45, 0xa2, 0xd1, 0xe8, 0x74, 0xba,
            0xdd, 0xee, 0x77, 0xbb, 0x5d, 0xae, 0xd7, 0xeb, 0x75, 0x3a, 0x9d, 0x4e, 0x27, 0x93, 0xc9, 0xe4,
            0x72, 0xb9, 0xdc, 0x6e, 0x37, 0x1b, 0x0d, 0x86, 0x43, 0xa1, 0xd0, 0x68, 0x34, 0x1a, 0x8d, 0xc6,
            0xe3, 0xf1, 0xf8, 0xfc, 0x7e, 0xbf, 0xdf, 0x6f, 0xb7, 0x5b, 0xad, 0xd6, 0x6b, 0x35, 0x9a, 0xcd,
            0x66, 0xb3, 0xd9, 0x6c, 0xb6, 0xdb, 0xed, 0x76, 0x3b, 0x1d, 0x0e, 0x87, 0xc3, 0xe1, 0x70, 0x38,
            0x1c, 0x8e, 0xc7, 0x63, 0xb1, 0x58, 0xac, 0x56, 0x2b, 0x95, 0xca, 0xe5, 0xf2, 0xf9, 0x7c, 0x3e,
            0x1f, 0x8f, 0x47, 0x23, 0x11, 0x08, 0x84, 0xc2, 0x61, 0x30, 0x98, 0x4c, 0xa6, 0x53, 0x29, 0x14,
            0x0a, 0x05, 0x02, 0x81, 0xc0, 0xe0, 0xf0, 0x78, 0xbc, 0xde, 0xef, 0xf7, 0xfb, 0xfd, 0xfe, 0xff
        };


        private static UInt32[] crc32_tab = new UInt32[256] {
            0x00000000, 0x77073096, 0xEE0E612C, 0x990951BA, 0x076DC419, 0x706AF48F, 0xE963A535, 0x9E6495A3,
            0x0EDB8832, 0x79DCB8A4, 0xE0D5E91E, 0x97D2D988, 0x09B64C2B, 0x7EB17CBD, 0xE7B82D07, 0x90BF1D91,
            0x1DB71064, 0x6AB020F2, 0xF3B97148, 0x84BE41DE, 0x1ADAD47D, 0x6DDDE4EB, 0xF4D4B551, 0x83D385C7,
            0x136C9856, 0x646BA8C0, 0xFD62F97A, 0x8A65C9EC, 0x14015C4F, 0x63066CD9, 0xFA0F3D63, 0x8D080DF5,
            0x3B6E20C8, 0x4C69105E, 0xD56041E4, 0xA2677172, 0x3C03E4D1, 0x4B04D447, 0xD20D85FD, 0xA50AB56B,
            0x35B5A8FA, 0x42B2986C, 0xDBBBC9D6, 0xACBCF940, 0x32D86CE3, 0x45DF5C75, 0xDCD60DCF, 0xABD13D59,
            0x26D930AC, 0x51DE003A, 0xC8D75180, 0xBFD06116, 0x21B4F4B5, 0x56B3C423, 0xCFBA9599, 0xB8BDA50F,
            0x2802B89E, 0x5F058808, 0xC60CD9B2, 0xB10BE924, 0x2F6F7C87, 0x58684C11, 0xC1611DAB, 0xB6662D3D,
            0x76DC4190, 0x01DB7106, 0x98D220BC, 0xEFD5102A, 0x71B18589, 0x06B6B51F, 0x9FBFE4A5, 0xE8B8D433,
            0x7807C9A2, 0x0F00F934, 0x9609A88E, 0xE10E9818, 0x7F6A0DBB, 0x086D3D2D, 0x91646C97, 0xE6635C01,
            0x6B6B51F4, 0x1C6C6162, 0x856530D8, 0xF262004E, 0x6C0695ED, 0x1B01A57B, 0x8208F4C1, 0xF50FC457,
            0x65B0D9C6, 0x12B7E950, 0x8BBEB8EA, 0xFCB9887C, 0x62DD1DDF, 0x15DA2D49, 0x8CD37CF3, 0xFBD44C65,
            0x4DB26158, 0x3AB551CE, 0xA3BC0074, 0xD4BB30E2, 0x4ADFA541, 0x3DD895D7, 0xA4D1C46D, 0xD3D6F4FB,
            0x4369E96A, 0x346ED9FC, 0xAD678846, 0xDA60B8D0, 0x44042D73, 0x33031DE5, 0xAA0A4C5F, 0xDD0D7CC9,
            0x5005713C, 0x270241AA, 0xBE0B1010, 0xC90C2086, 0x5768B525, 0x206F85B3, 0xB966D409, 0xCE61E49F,
            0x5EDEF90E, 0x29D9C998, 0xB0D09822, 0xC7D7A8B4, 0x59B33D17, 0x2EB40D81, 0xB7BD5C3B, 0xC0BA6CAD,
            0xEDB88320, 0x9ABFB3B6, 0x03B6E20C, 0x74B1D29A, 0xEAD54739, 0x9DD277AF, 0x04DB2615, 0x73DC1683,
            0xE3630B12, 0x94643B84, 0x0D6D6A3E, 0x7A6A5AA8, 0xE40ECF0B, 0x9309FF9D, 0x0A00AE27, 0x7D079EB1,
            0xF00F9344, 0x8708A3D2, 0x1E01F268, 0x6906C2FE, 0xF762575D, 0x806567CB, 0x196C3671, 0x6E6B06E7,
            0xFED41B76, 0x89D32BE0, 0x10DA7A5A, 0x67DD4ACC, 0xF9B9DF6F, 0x8EBEEFF9, 0x17B7BE43, 0x60B08ED5,
            0xD6D6A3E8, 0xA1D1937E, 0x38D8C2C4, 0x4FDFF252, 0xD1BB67F1, 0xA6BC5767, 0x3FB506DD, 0x48B2364B,
            0xD80D2BDA, 0xAF0A1B4C, 0x36034AF6, 0x41047A60, 0xDF60EFC3, 0xA867DF55, 0x316E8EEF, 0x4669BE79,
            0xCB61B38C, 0xBC66831A, 0x256FD2A0, 0x5268E236, 0xCC0C7795, 0xBB0B4703, 0x220216B9, 0x5505262F,
            0xC5BA3BBE, 0xB2BD0B28, 0x2BB45A92, 0x5CB36A04, 0xC2D7FFA7, 0xB5D0CF31, 0x2CD99E8B, 0x5BDEAE1D,
            0x9B64C2B0, 0xEC63F226, 0x756AA39C, 0x026D930A, 0x9C0906A9, 0xEB0E363F, 0x72076785, 0x05005713,
            0x95BF4A82, 0xE2B87A14, 0x7BB12BAE, 0x0CB61B38, 0x92D28E9B, 0xE5D5BE0D, 0x7CDCEFB7, 0x0BDBDF21,
            0x86D3D2D4, 0xF1D4E242, 0x68DDB3F8, 0x1FDA836E, 0x81BE16CD, 0xF6B9265B, 0x6FB077E1, 0x18B74777,
            0x88085AE6, 0xFF0F6A70, 0x66063BCA, 0x11010B5C, 0x8F659EFF, 0xF862AE69, 0x616BFFD3, 0x166CCF45,
            0xA00AE278, 0xD70DD2EE, 0x4E048354, 0x3903B3C2, 0xA7672661, 0xD06016F7, 0x4969474D, 0x3E6E77DB,
            0xAED16A4A, 0xD9D65ADC, 0x40DF0B66, 0x37D83BF0, 0xA9BCAE53, 0xDEBB9EC5, 0x47B2CF7F, 0x30B5FFE9,
            0xBDBDF21C, 0xCABAC28A, 0x53B39330, 0x24B4A3A6, 0xBAD03605, 0xCDD70693, 0x54DE5729, 0x23D967BF,
            0xB3667A2E, 0xC4614AB8, 0x5D681B02, 0x2A6F2B94, 0xB40BBE37, 0xC30C8EA1, 0x5A05DF1B, 0x2D02EF8D
        };
        #endregion

        private static byte[] HEAD_SALT_TC = Encoding.Default.GetBytes("HaGongDa");//new byte[];
        private static byte[] TAIL_SALT_TC = Encoding.Default.GetBytes("nANlIgONG");

        private static void TC_encode(byte[] src, int len, ref byte[] dst)
        {
            int i = 0;
            for (i = 0; i < len; i++)
                dst[i] = src[i];

            dst[len] = 0xfd;
            dst[len + 1] = 0xff;
            dst[len + 2] = 0xff;
            dst[len + 3] = 0xff;

            UInt32 crc = 0xFFFFFFFF;
            for (i = 0; i < 4; i++)         //Header CRC
            {
                crc = Calc_CRC_Byte(crc, dst[i]);
            }
            for (i = 0; i < 8; i++)         //Add Head Salt
            {
                crc = Calc_CRC_Byte(crc, HEAD_SALT_TC[i]);
            }
            for (i = 4; i < len + 4; i++)   //Data Field CRC
            {
                crc = Calc_CRC_Byte(crc, dst[i]);
            }
            for (i = 0; i < 9; i++)         //Add Tail Salt
            {
                crc = Calc_CRC_Byte(crc, TAIL_SALT_TC[i]);
            }

            //*((uint32_t*)(dst + len + 4)) = crc;
            byte[] crc_buf = System.BitConverter.GetBytes(crc);

            for (i = 0; i < 4; i++)
                dst[len + 4 + i] = crc_buf[i];

            for (i = 0; i < len + 8; i++)
            {
                dst[i] = (byte)(dst[i] ^ RandomSeq[i]);
            }
        }

        private static UInt32 Calc_CRC_Byte(UInt32 crc, byte buf)
        {
            return (((crc >> 8) & 0x00FFFFFF) ^ crc32_tab[(crc ^ buf) & (UInt32)0xFF]);
        }

        #endregion
    }
}
