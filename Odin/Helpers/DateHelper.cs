using System;
using System.Threading;

namespace Odin.Helpers
{
    public class DateHelper
    {
        public static string GetViewFormat(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return "";
            string ecks = phone.ToUpper().IndexOf("X") > -1 ? " X":" ";
            string value = new System.Text.RegularExpressions.Regex(@"\D").Replace(phone, string.Empty);
            if (value.Length > 10)
                value = value.TrimStart('1');
            if (value.Length == 7)
                return Convert.ToInt64(value).ToString("###'.'####");
            if (value.Length == 10)
                return Convert.ToInt64(value).ToString("0##'.'###'.'####");
            if (value.Length > 10)
                return Convert.ToInt64(value)
                    .ToString("0##'.'###'.'####" + ecks + new String('#', (value.Length - 10)));
            return value;            
        }
        public static string GetViewFormat(DateTime? date)
        {
            return date.HasValue ? string.Format(Thread.CurrentThread.CurrentCulture, "{0:dd-MMM-yyyy}", date) : String.Empty;
        }
        public static string GetViewFormat(decimal? amount)
        {
            return amount.HasValue ? string.Format(Thread.CurrentThread.CurrentCulture, "{0:c}", amount) : String.Empty;
        }
        public static string GetViewFormat(DateTime? date, bool isTime)
        {
            if (isTime)
                return date.HasValue ? string.Format(Thread.CurrentThread.CurrentCulture, "{0:t}", date) : String.Empty;
            else
                return date.HasValue ? string.Format(Thread.CurrentThread.CurrentCulture, "{0:dd-MMM-yyyy}", date) : String.Empty;
        }

        public static string GetViewHistoryFormat(DateTime? date)
        {
            return date.HasValue ? string.Format(Thread.CurrentThread.CurrentCulture, "{0:dd-MMM-yyyy}", date) + "at " + (date.HasValue ? string.Format(Thread.CurrentThread.CurrentCulture, "{0:t}", date) : String.Empty): String.Empty;
        }

    }
}