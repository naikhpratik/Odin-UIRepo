using System;

namespace Odin.Helpers
{
    public class DateHelper
    {
        public static string GetViewFormat(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString("MM/dd/yy") : String.Empty;
        }
    }
}