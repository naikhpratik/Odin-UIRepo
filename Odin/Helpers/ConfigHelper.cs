using Odin.Interfaces;
using System;
using System.Configuration;
using System.Web.Configuration;

namespace Odin.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        private readonly string SeTokenKey = "SeApiToken";
        private readonly string SendGridAPIKey = "SendGridAPIKey";
        private readonly string DWOdinTeamEmailFrom = "DWOdinTeamEmailFrom";
        
        public string GetSeApiToken()
        {
            return WebConfigurationManager.AppSettings[SeTokenKey];
        }
        public string GetSendGridAPIKey()
        {
            return WebConfigurationManager.AppSettings[SendGridAPIKey];
        }
public static int GetWebServerPort()
        {
            return 55555;
            //return ConfigurationManager.AppSettings[WebServerPortKey].ParseInt(2020);
        }
        public string GetDWOdinTeamEmailFrom()
        {
            return WebConfigurationManager.AppSettings[DWOdinTeamEmailFrom];
        }
    }
}