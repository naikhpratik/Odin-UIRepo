using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Odin.Interfaces;

namespace Odin.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        private readonly string SeTokenKey = "SeApiToken";

        public string GetSeApiToken()
        {
            return WebConfigurationManager.AppSettings[SeTokenKey];
        }
    }
}