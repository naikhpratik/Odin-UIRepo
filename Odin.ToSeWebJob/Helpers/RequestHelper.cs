using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Odin.ToSeWebJob.Helpers
{
    public static class RequestHelper
    {
        public static HttpRequestMessage CreateBaldrRequest(string token, string url, string jsonData)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post
            };
            request.Headers.Add("Token", token);
            request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            return request;
        }
    }
}
