using System.Runtime.InteropServices;

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
    };
}
