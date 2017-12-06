using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.ToSeWebJob
{
    public static class ConfigHelper
    {
        private static readonly string SeApiTokenKey = "SeApiToken";
        private static readonly string BaldrApiUrl = "BaldrApiUrl";

        public static string GetSeApiToken()
        {
            return ConfigurationManager.AppSettings[SeApiTokenKey];
        }

        public static string GetBaldrApiUrl()
        {
            return ConfigurationManager.AppSettings[BaldrApiUrl];
        }
    }
}
