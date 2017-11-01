using System;

namespace Odin.Helpers
{
    public class DateHelper
    {
        public static string GetViewFormat(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString("MM/dd/yy") : String.Empty;
        }
        public static string GetViewFormat(DateTime? date, bool isTime)
        {
            if (isTime)
                return date.HasValue ? date.Value.ToString("t") : String.Empty;
            else
                return date.HasValue ? date.Value.ToString("MM/dd/yy") : String.Empty;
        }
    }
}