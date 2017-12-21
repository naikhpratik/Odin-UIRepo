namespace Odin.PropBotWebJob.Extensions
{
    public static class StringExtensions
    {

        public static string StringBetween(this string str, string startStr, string endStr)
        {
            var start = str.IndexOf(startStr) + (startStr.Length);
            var sub = str.Substring(start);
            var end = sub.IndexOf(endStr);
            return sub.Substring(0, end);
        }

        public static string CleanNumeric(this string str)
        {
            return str.Trim().Replace("\"", "").Replace(",", "").Replace("$","");
        }

        public static string StringEndingAt(this string str, string endStr)
        {
            int end = str.IndexOf(endStr);
            return str.Substring(0,end + endStr.Length);
        }

        public static string CleanText(this string str)
        {
            return str.Trim().Replace("\n", "").Replace("<br />", "").Replace("<br/>", "").Replace("<br>", "").Replace("\r", "").Replace("\t", "").Trim();
        }

        public static string LowerReplace(this string str, string oldChars, string newChars)
        {
            var lowerStr = str.ToLower();
            return lowerStr.Replace(oldChars, newChars);
        }
    }
}
