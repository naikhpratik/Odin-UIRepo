using System;
using System.Threading;

namespace Odin.Helpers
{
    public class DateHelper
    {
        public static string GetViewFormat(string phone)
        {
            return string.Format("{0:###'.'###'.'####}", double.Parse(phone));
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