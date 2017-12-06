using Odin.Interfaces;
using System;

namespace Odin.Helpers
{
    public class BookMarkletHelper : IBookMarkletHelper
    {
        public const string TRULIA = "www.trulia.com";
        public const string APARTMENTS = "www.apartments.com";
        public const string REALTOR = "www.realtor.com";

        public bool IsValidDomain(string domain)
        {
            return domain == REALTOR || domain == APARTMENTS || domain == TRULIA;
        }

        public bool IsValidUrl(string url)
        {
            if (!String.IsNullOrEmpty(url))
            {
                var uri = new Uri(url);
                return IsValidDomain(uri.Host);
            }
            return false;
        }
    }
}