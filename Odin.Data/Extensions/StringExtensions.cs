using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Data.Extensions
{
    public static class StringExtensions
    {
        public static bool NullContains(this string str, string containsStr)
        {
            if (String.IsNullOrEmpty(str))
                return false;

            return str.Contains(containsStr);
        }
    }
}
