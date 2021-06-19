using System;
using System.Runtime.InteropServices;


using UniHelper;

namespace DataProcess
{
    public class SeqToServer
    {

        //0x80 , 
        //0x5A , 0x5A ,
        //0x04 , 0x03 , 0x02 , 0x01 , 
        //0xA0, 0xA0 , 0xA0, 0xA0 , 
        //0x11 , 0x01, 0x11 , 0x00 , 
        //0x00 , 0x00, 0x00 , 0x00 , 
        //0x00 , 
        //0x00 , 0x00, 0x00, 0x00, 
        //0x82, 0x1A, 
        //0x50, 0xCE, 0xE3, 0x1D,

        [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 32)]
        private struct seqHeader
        {
            public byte version; 
            public UInt16 mid;  

            public UInt32 sid;                    
            public UInt32 did;                      
            public UInt32 bid;                     
            public UInt32 no;                     
            public byte flag;                   
            public UInt32 resvd;                    
            public UInt16 JD;                     
            public UInt32 JS;              
            public UInt16 length;


            public seqHeader(UInt16 jd,UInt32 js,UInt16 len)
            {
                this.version = 0x80;
                this.mid = 0x5A5A;
                this.sid = 0x01020304;
                this.did = 0xA0A0A0A0;
                this.bid = 0x00110111;
                this.no = 0;
                this.flag = 0x00;
                this.resvd = 0;
                this.JD = jd;
                this.JS = js;
                this.length = len;


        }

        }
        public static byte[] generateSequence(byte[] buffer)
        {
            UInt16 jd = UniFunction.CalDaysFrom2000(DateTime.Now);
            UInt32 js = UniFunction.CalSecondsFromToday(DateTime.Now);

            seqHeader NewSeqHeader = new seqHeader(jd,js,(UInt16)buffer.Length
                );

            byte[] sendbuf = new byte[buffer.Length + 32];

            byte[] bufHeader = UniSerialize.StructToByte(NewSeqHeader);

            Array.Copy(bufHeader, sendbuf, 32);
            Array.Copy(buffer, 0, sendbuf, 32, buffer.Length);

            return sendbuf;
        }
    }
}
