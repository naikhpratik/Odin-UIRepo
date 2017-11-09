using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Testing;
using MyDwellworks.App_Start;
using Ninject.Web.WebApi;
using NUnit.Framework;
using Odin.Data.Core.Models;
using Odin.Data.Persistence;
using Odin.Helpers;

namespace Odin.IntegrationTests
{
    [TestFixture]
    public abstract class WebApiBaseTest
    {
        protected const string Url = "http://localhost/";
        protected TestServer Server;
        protected ApplicationDbContext Context;
        protected string ApiKey;
        protected Transferee transferee;
        protected Consultant dsc;
        protected Manager pm;

        [SetUp]
        public void SetUp()
        {
            Context = new ApplicationDbContext();
            Server = TestServer.Create<Startup>();

            ApiKey = "SeApiTokenKeyTest";

            transferee = Context.Transferees.First(u => u.UserName.Equals("odinee@dwellworks.com"));
            dsc = Context.Consultants.First(u => u.UserName.Equals("odinconsultant@dwellworks.com"));
            pm = Context.Managers.First(u => u.UserName.Equals("odinpm@dwellworks.com"));
        }

        [TearDown]
        public void TearDown()
        {
            Context.Dispose();
            Server.Dispose();
        }

        protected string MakeUri(string uri)
        {
            return $"{Url}{uri}";
        }

        protected HttpRequestMessage CreateRequest(string url, string mthv, HttpMethod method)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(Url + url)
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mthv));
            request.Method = method;

            return request;
        }

        protected HttpRequestMessage CreateRequest<T>(string url, string mthv, HttpMethod method, T content) where T : class
        {
            var request = CreateRequest(url, mthv, method);
            request.Content = new ObjectContent<T>(content, new JsonMediaTypeFormatter());
            
            return request;
        }
    }
}
