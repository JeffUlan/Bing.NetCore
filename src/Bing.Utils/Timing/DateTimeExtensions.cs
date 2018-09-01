﻿using System;
using System.Linq;
using System.Text;

namespace Bing.Utils.Timing
{
    /// <summary>
    /// 日期时间辅助扩展操作
    /// </summary>
    public static class DateTimeExtensions
    {
        #region 字段

        /// <summary>
        /// 1970年1月1日
        /// </summary>
        internal static readonly DateTime Date1970 = new DateTime(1970, 1, 1);

        /// <summary>
        /// 最小日期
        /// </summary>
        internal static readonly DateTime MinDate = new DateTime(1900, 1, 1);

        /// <summary>
        /// 最大日期
        /// </summary>
        internal static readonly DateTime MaxDate = new DateTime(9999, 12, 31, 23, 59, 59, 999);

        /// <summary>
        /// 初始化js日期时间戳
        /// </summary>
        public static long InitialJavaScriptDateTicks = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;

        #endregion

        #region ToDateTimeString(yyyy-MM-dd HH:mm:ss)

        /// <summary>
        /// 获取格式化字符串，带时分秒，格式："yyyy-MM-dd HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="isRemoveSecond">是否移除秒,true:是,false:否</param>
        /// <returns></returns>
        public static string ToDateTimeString(this DateTime dateTime, bool isRemoveSecond = false)
        {
            if (isRemoveSecond)
            {
                return dateTime.ToString("yyyy-MM-dd HH:mm");
            }
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 获取格式化字符串，带时分秒，格式："yyyy-MM-dd HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="isRemoveSecond">是否移除秒,true:是,false:否</param>
        /// <returns></returns>
        public static string ToDateTimeString(this DateTime? dateTime, bool isRemoveSecond = false)
        {
            if (dateTime == null)
            {
                return string.Empty;
            }
            return ToDateTimeString(dateTime.Value, isRemoveSecond);
        }

        #endregion

        #region ToDateString(yyyy-MM-dd)

        /// <summary>
        /// 获取格式化字符串，不带时分秒，格式："yyyy-MM-dd"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <returns></returns>
        public static string ToDateString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 获取格式化字符串，不带时分秒，格式："yyyy-MM-dd"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <returns></returns>
        public static string ToDateString(this DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return string.Empty;
            }
            return ToDateString(dateTime.Value);
        }

        #endregion

        #region ToTimeString(HH:mm:ss)

        /// <summary>
        /// 获取格式化字符串，不带年月日，格式："HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <returns></returns>
        public static string ToTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("HH:mm:ss");
        }

        /// <summary>
        /// 获取格式化字符串，不带年月日，格式："HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <returns></returns>
        public static string ToTimeString(this DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return string.Empty;
            }
            return ToTimeString(dateTime.Value);
        }

        #endregion

        #region ToMillisecondString(yyyy-MM-dd HH:mm:ss.fff)

        /// <summary>
        /// 获取格式化字符串，带毫秒，格式："yyyy-MM-dd HH:mm:ss.fff"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <returns></returns>
        public static string ToMillisecondString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        /// <summary>
        /// 获取格式化字符串，带毫秒，格式："yyyy-MM-dd HH:mm:ss.fff"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <returns></returns>
        public static string ToMillisecondString(this DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return string.Empty;
            }
            return ToMillisecondString(dateTime.Value);
        }

        #endregion

        #region ToChineseDateString(yyyy年MM月dd日)

        /// <summary>
        /// 获取格式化字符串，不带时分秒，格式："yyyy年MM月dd日"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <returns></returns>
        public static string ToChineseDateString(this DateTime dateTime)
        {
            return string.Format("{0}年{1}月{2}日", dateTime.Year, dateTime.Month, dateTime.Day);
        }

        /// <summary>
        /// 获取格式化字符串，不带时分秒，格式："yyyy年MM月dd日"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <returns></returns>
        public static string ToChineseDateString(this DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return string.Empty;
            }
            return ToChineseDateString(dateTime.Value);
        }

        #endregion

        #region ToChineseDateTimeString(yyyy年MM月dd日 HH时mm分)

        /// <summary>
        /// 获取格式化字符串，带时分秒，格式："yyyy年MM月dd日 HH时mm分"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="isRemoveSecond">是否移除秒</param>
        /// <returns></returns>
        public static string ToChineseDateTimeString(this DateTime dateTime, bool isRemoveSecond = false)
        {
            StringBuilder result = new StringBuilder();
            result.AppendFormat("{0}年{1}月{2}日", dateTime.Year, dateTime.Month, dateTime.Day);
            result.AppendFormat(" {0}时{1}分", dateTime.Hour, dateTime.Minute);
            if (isRemoveSecond == false)
            {
                result.AppendFormat("{0}秒", dateTime.Second);
            }
            return result.ToString();
        }

        /// <summary>
        /// 获取格式化字符串，带时分秒，格式："yyyy年MM月dd日 HH时mm分"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="isRemoveSecond">是否移除秒</param>
        /// <returns></returns>
        public static string ToChineseDateTimeString(this DateTime? dateTime, bool isRemoveSecond = false)
        {
            if (dateTime == null)
            {
                return string.Empty;
            }
            return ToChineseDateTimeString(dateTime.Value, isRemoveSecond);
        }

        #endregion

        #region Description(获取描述)
        /// <summary>
        /// 获取描述
        /// </summary>
        /// <param name="span">时间间隔</param>
        /// <returns></returns>
        public static string Description(this TimeSpan span)
        {
            StringBuilder result = new StringBuilder();
            if (span.Days > 0)
            {
                result.AppendFormat("{0}天", span.Days);
            }
            if (span.Hours > 0)
            {
                result.AppendFormat("{0}小时", span.Hours);
            }
            if (span.Minutes > 0)
            {
                result.AppendFormat("{0}分", span.Minutes);
            }
            if (span.Seconds > 0)
            {
                result.AppendFormat("{0}秒", span.Seconds);
            }
            if (span.Milliseconds > 0)
            {
                result.AppendFormat("{0}毫秒", span.Milliseconds);
            }
            if (result.Length > 0)
            {
                return result.ToString();
            }
            return $"{span.TotalMilliseconds * 1000}毫秒";
        }
        #endregion

        #region IsWeekend(当前时间是否周末)

        /// <summary>
        /// 当前时间是否周末
        /// </summary>
        /// <param name="dateTime">时间点</param>
        /// <returns></returns>
        public static bool IsWeekend(this DateTime dateTime)
        {
            DayOfWeek[] weeks = { DayOfWeek.Saturday, DayOfWeek.Sunday };
            return weeks.Contains(dateTime.DayOfWeek);
        }

        #endregion

        #region IsWorkday(当前时间是否工作日)
        /// <summary>
        /// 当前时间是否工作日
        /// </summary>
        /// <param name="dateTime">时间点</param>
        /// <returns></returns>
        public static bool IsWeekday(this DateTime dateTime)
        {
            DayOfWeek[] weeks =
                {DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday};
            return weeks.Contains(dateTime.DayOfWeek);
        }

        #endregion

        #region ToUniqueString(获取时间相对唯一字符串)
        /// <summary>
        /// 获取时间相对唯一字符串
        /// </summary>
        /// <param name="dateTime">时间点</param>
        /// <param name="milsec">是否使用毫秒</param>
        /// <returns></returns>
        public static string ToUniqueString(this DateTime dateTime, bool milsec = false)
        {
            int sedonds = dateTime.Hour * 3600 + dateTime.Minute * 60 + dateTime.Second;
            string value = string.Format("{0}{1}{2}", dateTime.ToString("yy"), dateTime.DayOfWeek, sedonds);
            return milsec ? value + dateTime.ToString("fff") : value;
        }

        #endregion

        #region ToJsGetTime(将时间转换为Js时间格式)
        /// <summary>
        /// 将时间转换为Js时间格式（Date.getTiem()）
        /// </summary>
        /// <param name="dateTime">时间点</param>
        /// <returns></returns>
        public static string ToJsGetTime(this DateTime dateTime)
        {
            DateTime utc = dateTime.ToUniversalTime();
            return ((long)utc.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds).ToString();
        }

        #endregion

        #region SetTime(设置时间)
        /// <summary>
        /// 设置时间，设置时分秒
        /// </summary>
        /// <param name="date">时间</param>
        /// <param name="hours">小时</param>
        /// <param name="minutes">分钟</param>
        /// <param name="seconds">秒</param>
        /// <returns>返回设置后的时间</returns>
        public static DateTime SetTime(this DateTime date, int hours, int minutes, int seconds)
        {
            return date.SetTime(new TimeSpan(hours, minutes, seconds));
        }
        /// <summary>
        /// 设置时间，设置时分秒毫秒
        /// </summary>
        /// <param name="date">时间</param>
        /// <param name="hours">小时</param>
        /// <param name="minutes">分钟</param>
        /// <param name="seconds">秒</param>
        /// <param name="milliseconds">毫秒</param>
        /// <returns>返回设置后的时间</returns>
        public static DateTime SetTime(this DateTime date, int hours, int minutes, int seconds, int milliseconds)
        {
            return date.SetTime(new TimeSpan(0, hours, minutes, seconds, milliseconds));
        }
        /// <summary>
        /// 设置时间，设置时间间隔
        /// </summary>
        /// <param name="date">时间</param>
        /// <param name="time">时间间隔</param>
        /// <returns>返回设置后的时间</returns>
        public static DateTime SetTime(this DateTime date, TimeSpan time)
        {
            return date.Date.Add(time);
        }
        #endregion

        #region EndOfDay(设置指定时间为当天的结束时间)
        /// <summary>
        /// 设置指定时间为当天的结束时间。范例：yyyy-MM-dd 23:59:59.999
        /// </summary>
        /// <param name="date">指定时间</param>
        /// <returns>当天的结束时间</returns>
        public static DateTime EndOfDay(this DateTime date)
        {
            return date.SetTime(23, 59, 59, 999);
        }
        #endregion

        #region BeginOfDay(设置指定时间为当天的开始时间)
        /// <summary>
        /// 设置指定时间为当天的开始时间（凌晨）。范例：yyyy-MM-dd 00:00:00
        /// </summary>
        /// <param name="time">指定时间</param>
        /// <returns>当天的开始时间</returns>
        public static DateTime BeginOfDay(this DateTime time)
        {
            return time.SetTime(0, 0, 0, 0);
        }
        #endregion

        #region EndOfMonth(设置指定时间为当月的结束时间)

        /// <summary>
        /// 设置指定时间为当月的结束时间。范例：yyyy-MM-dd 23:59:59:999
        /// </summary>
        /// <param name="date">时间</param>
        /// <returns>当月的结束时间</returns>
        public static DateTime EndOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month), 23, 59, 59, 999);
        }

        #endregion

        #region BeginOfMonth(设置指定时间为当月的开始时间)

        /// <summary>
        /// 设置指定时间为当月的开始时间。范例：yyyy-MM-01 00:00:00.000
        /// </summary>
        /// <param name="date">时间</param>
        /// <returns>当月的开始时间</returns>
        public static DateTime BeginOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1, 0, 0, 0, 0);
        }

        #endregion

        #region GetFirstDayOfMonth(获取指定日期的月份第一天)
        /// <summary>
        /// 获取指定日期的月份第一天
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>月份第一天</returns>
        public static DateTime GetFirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }
        /// <summary>
        /// 获取指定日期的月份第一天，指定星期几
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="dayOfWeek">星期几</param>
        /// <returns>月份第一天</returns>
        public static DateTime GetFirstDayOfMonth(this DateTime date, DayOfWeek dayOfWeek)
        {
            var dt = date.GetFirstDayOfMonth();
            while (dt.DayOfWeek != dayOfWeek)
                dt = dt.AddDays(1);
            return dt;
        }
        #endregion

        #region GetLastDayOfMonth(获取指定日期的月份最后一天)
        /// <summary>
        /// 获取指定日期的月份最后一天
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>最后一天</returns>
        public static DateTime GetLastDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, GetCountDaysOfMonth(date));
        }
        /// <summary>
        /// 获取指定日期的月份最后一天，指定星期几
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="dayOfWeek">星期几</param>
        /// <returns>最后一天</returns>
        public static DateTime GetLastDayOfMonth(this DateTime date, DayOfWeek dayOfWeek)
        {
            var dt = date.GetLastDayOfMonth();
            while (dt.DayOfWeek != dayOfWeek)
                dt = dt.AddDays(-1);
            return dt;
        }
        #endregion

        #region GetCountDaysOfMonth(获取月总天数)
        /// <summary>
        /// 获取月总天数
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>月总天数</returns>
        public static int GetCountDaysOfMonth(this DateTime date)
        {
            var nextMonth = date.AddMonths(1);
            return new DateTime(nextMonth.Year, nextMonth.Month, 1).AddDays(-1).Day;
        }
        #endregion

        #region GetMillisecondsSince1970(获取当前毫秒数)
        /// <summary>
        /// 获取当前毫秒数，毫秒数=1970年1月1日-当前时间，UNIX
        /// </summary>
        /// <param name="datetime">当前时间</param>
        /// <returns>毫秒数</returns>
        public static long GetMillisecondsSince1970(this DateTime datetime)
        {
            var ts = datetime.Subtract(Date1970);
            return (long)ts.TotalMilliseconds;
        }
        #endregion

        #region CompareInterval(计算两个时间的间隔)

        /// <summary>
        /// 计算两个时间的间隔
        /// </summary>
        /// <param name="begin">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="dateFormat">间隔格式(y:年,M:月,d:天,h:小时,m:分钟,s:秒,fff:毫秒)</param>
        /// <returns></returns>
        public static long CompareInterval(this DateTime begin, DateTime end, string dateFormat)
        {
            long interval = begin.Ticks - end.Ticks;
            DateTime dt1;
            DateTime dt2;
            switch (dateFormat)
            {
                case "fff":
                    interval /= 10000;
                    break;
                case "s":
                    interval /= 10000000;
                    break;
                case "m":
                    interval /= 600000000;
                    break;
                case "h":
                    interval /= 36000000000;
                    break;
                case "d":
                    interval /= 864000000000;
                    break;
                case "M":
                    dt1 = (begin.CompareTo(end) >= 0) ? end : begin;
                    dt2 = (begin.CompareTo(end) >= 0) ? begin : end;
                    interval = -1;
                    while (dt2.CompareTo(dt1)>=0)
                    {
                        interval++;
                        dt1 = dt1.AddMonths(1);
                    }
                    break;
                case "y":
                    dt1 = (begin.CompareTo(end) >= 0) ? end : begin;
                    dt2 = (begin.CompareTo(end) >= 0) ? begin : end;
                    interval = -1;
                    while (dt2.CompareTo(dt1) >= 0)
                    {
                        interval++;
                        dt1 = dt1.AddMonths(1);
                    }

                    interval /= 12;
                    break;
            }

            return interval;
        }

        #endregion

        #region IsBetweenTime(判断当前时间是否在指定时间段内)

        /// <summary>
        /// 判断当前时间是否在指定时间段内，格式：hh:mm:ss
        /// </summary>
        /// <param name="currentTime">当前时间</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public static bool IsBetweenTime(this DateTime currentTime, DateTime beginTime, DateTime endTime)
        {
            var am = beginTime.TimeOfDay;
            var pm = endTime.TimeOfDay;

            var now = currentTime.TimeOfDay;
            if (pm < am)//截止时间小于开始时间，表示跨天
            {
                if (now <= pm || now >= am)
                {
                    return true;
                }
            }

            if (now >= am && now <= pm)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region IsBetweenDate(判断当前时间是否在指定日期时间段内)

        /// <summary>
        /// 判断当前时间是否在指定日期时间段内，格式：yyyy-MM-dd
        /// </summary>
        /// <param name="currentDate">当前日期</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public static bool IsBetweenDate(this DateTime currentDate, DateTime beginDate, DateTime endDate)
        {
            var begin = beginDate.Date;
            var end = endDate.Date;
            var now = currentDate.Date;

            return now >= begin && now <= end;
        }

        #endregion

        #region IsBetween(判断当前时间是否在指定时间范围内)

        /// <summary>
        /// 判断当前时间是否在指定时间范围内，格式：yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="current">当前时间</param>
        /// <param name="begin">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        public static bool IsBetween(this DateTime current, DateTime begin, DateTime end)
        {
            var ticks = current.Ticks;
            return ticks >= begin.Ticks && ticks <= end.Ticks;
        }

        #endregion

        #region IsValid(是否有效时间)

        /// <summary>
        /// 是否有效时间
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static bool IsValid(this DateTime value)
        {
            return (value >= MinDate) && (value <= MaxDate);
        }

        #endregion
    }
}
