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
