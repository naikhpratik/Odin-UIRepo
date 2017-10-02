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
        private readonly string SendGridAPIKey = "SendGridAPIKey";

        public string GetSeApiToken()
        {
            return WebConfigurationManager.AppSettings[SeTokenKey];
        }
        public string GetSendGridAPIKey()
        {
            return WebConfigurationManager.AppSettings[SendGridAPIKey];
        }
    }
}