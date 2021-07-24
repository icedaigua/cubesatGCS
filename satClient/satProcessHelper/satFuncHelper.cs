using System;
using UniHelper;

namespace satProcessHelper
{
    public class satFuncHelper
    {

        public static string computeJD(string arg, string[] para)
        {

            //UniFunction.CalDaysFrom2000();
            return "2018-08-03";
        }

        public static string computeJS(string arg, string[] para)
        {
            return "12:57:42";
        }
        
        public static string computeUTC(string arg, string[] para)
        {
            UInt32 uival = UInt32.Parse(arg);
            double seconds = uival;

            double secs = Convert.ToDouble(seconds);

            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(secs);
            return dt.ToString();
        }

        public static string workmode(string arg, string[] para)
        {
            byte bval = byte.Parse(arg);
            switch(bval)
            {
                case 0:
                    return "Normal";
                case 1:
                    return "Sleep";
                default:
                    return "无效模式";
            }
        }

        public static string computeRunTime(string arg, string[] para)
        {
            UInt32 uival = UInt32.Parse(arg);

            double seconds = Convert.ToDouble(uival);

            //dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(secs);

            TimeSpan rts = new TimeSpan(0, 0, (int)seconds);

            return rts.ToString();
            
        }

        public static string computerPower(string arg, string[] para)
        {
            UInt16 uiVal = UInt16.Parse(arg);

            double fval = (20 * Math.Log10(uiVal * 0.00767));
            return fval.ToString("F4");
        }

        public static string ADtoDigi(string arg, string[] para)
        {
            UInt16 uiVal = UInt16.Parse(arg);

            float paraK = float.Parse(para[2]);
            float paraD = float.Parse(para[3]);

            float fval = (float)(uiVal * 5.0 / 4096 * (paraK) + paraD);
            return fval.ToString("F4");
        }

        public static string LineFunction(string arg, string[] para)
        {
            UInt16 uiVal = UInt16.Parse(arg);

            float paraK = float.Parse(para[2]);
            float paraD = float.Parse(para[3]);

            float fval = (float)(uiVal * (paraK) + paraD);
            return fval.ToString("F4");
        }

        public static string ADtoDigi_obc(string arg, string[] para)
        {
            UInt16 uiVal = UInt16.Parse(arg);

            double fval =  (uiVal < 0x8000) ? ((uiVal >> 4) * 0.0625) : (((uiVal >> 4) - 2048) * 0.0625);
            return fval.ToString("F4");
        }



    }
}
