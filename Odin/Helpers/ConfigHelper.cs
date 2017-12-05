using Odin.Interfaces;
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
        public string GetDWOdinTeamEmailFrom()
        {
            return WebConfigurationManager.AppSettings[DWOdinTeamEmailFrom];
        }
    }
}