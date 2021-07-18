using System;

namespace UniHelper
{
    public static class UniFunction
    {

        /// <summary>
        /// 计算相对于2000年1月1日的积日，2000年1月1日为第1天
        /// </summary>
        /// <param name="dtNow">当前日期</param>
        /// <returns></returns>
        public static ushort CalDaysFrom2000(DateTime dtNow)
        {
            try
            {
                DateTime dtStart = new DateTime(2000, 1, 1, 0, 0, 0);
                ushort interval = 0;
                for (DateTime dtTemp = dtStart; dtTemp < dtNow; dtTemp = dtTemp.AddDays(1))
                    interval++;

                return interval;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 将UTC时间转换为uint
        /// </summary>
        /// <param name="dtNow">当前日期</param>
        /// <returns></returns>
        public static uint xDateSeconds(DateTime xdatenow)
        {
            long xdateseconds = 0;
            //DateTime xdatenow = DateTime.UtcNow;     //当前UTC时间

            long xminute = 60;      //一分种60秒
            long xhour = 3600;
            long xday = 86400;
            long byear = 1970;//从1970-1-1 0：00：00开始到现在所过的秒
            long[] xmonth = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };
            long[] xyear = { 365, 366 };
            long num = 0;
            xdateseconds += xdatenow.Second;    //算秒
            xdateseconds += xdatenow.Minute * xminute;      //算分
            xdateseconds += xdatenow.Hour * xhour;      //算时
            xdateseconds += (xdatenow.Day - 1) * xday;        //算天
            //算月(月换成天算)
            if (DateTime.IsLeapYear(xdatenow.Year))
            {
                xdateseconds += (xmonth[xdatenow.Month - 1] + 1) * xday;
            }
            else
            {
                xdateseconds += (xmonth[xdatenow.Month - 1]) * xday;
            }
            //算年（年换成天算）
            long lyear = xdatenow.Year - byear;
            for (int i = 0; i < lyear; i++)
            {
                if (DateTime.IsLeapYear((int)byear + i))
                {
                    num++;
                }
            }
            xdateseconds += ((lyear - num) * xyear[0] + num * xyear[1]) * xday;
            return (uint)xdateseconds;
        }

        /// <summary>
        /// 计算相对于当日零时的积秒，量化单位为0.1ms
        /// </summary>
        /// <param name="dtNow">当前时间</param>
        /// <returns></returns>
        public static uint CalSecondsFromToday(DateTime dtNow)
        {
            try
            {
                DateTime dtToday = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                uint interval = Convert.ToUInt32((dtNow.Ticks - dtToday.Ticks) / 1000.0);

                return interval;
            }
            catch
            {
                return 0;
            }
        }
    }
}
