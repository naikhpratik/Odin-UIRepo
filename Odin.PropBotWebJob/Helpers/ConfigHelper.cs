using System;
using System.Web.Configuration;

namespace Odin.PropBotWebJob
{
    public static class ConfigHelper
    {
        private const string SendGridAPIKey = "SendGridAPIKey";
        private const string EmailErrorTo = "EmailErrorTo";
        private const string EmailErrorFrom = "EmailErrorFrom";
        private const string MaxDequeueCount = "MaxDequeueCount";

        public static string GetSendGridAPIKey()
        {
            return WebConfigurationManager.AppSettings[SendGridAPIKey];
        }

        public static string[] GetEmailErrorTo()
        {
            return WebConfigurationManager.AppSettings[EmailErrorTo].Split(';');
        }

        public static string GetEmailErrorFrom()
        {
            return WebConfigurationManager.AppSettings[EmailErrorFrom];
        }

        public static int GetMaxDequeueCount()
        {
            return Int32.Parse(WebConfigurationManager.AppSettings[MaxDequeueCount]);
        }

    }
}
