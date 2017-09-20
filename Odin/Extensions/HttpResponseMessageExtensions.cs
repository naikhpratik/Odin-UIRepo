using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Odin.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<T> ReadContentAsyncSafe<T>(this HttpResponseMessage message) where T : class
        {
            try
            {
                return await message.Content.ReadAsAsync<T>();
            }
            catch
            {
                return null;
            }
        }
    }
}