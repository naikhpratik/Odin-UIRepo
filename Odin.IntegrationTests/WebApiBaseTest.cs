using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using MyDwellworks.App_Start;
using Ninject.Web.WebApi;
using NUnit.Framework;
using Odin.Data.Persistence;

namespace Odin.IntegrationTests
{
    [TestFixture]
    public abstract class WebApiBaseTest : IDisposable
    {
        protected const string Url = "http://localhost/";
        protected HttpClient Client;
        protected ApplicationDbContext Context;
        private HttpServer _server;
        protected string ApiKey;

        [SetUp]
        public void SetUp()
        {
            Context = new ApplicationDbContext();
            ApiKey = "SeApiTokenKeyTest";

            NinjectWebCommon.Start();
            var config = new HttpConfiguration();
            var kernel = NinjectWebCommon.bootstrapper.Kernel;
            var resolver = new NinjectDependencyResolver(kernel);
            config.DependencyResolver = resolver;
            WebApiConfig.Register(config);
            _server = new HttpServer(config);
            Client = new HttpClient(_server);
        }

        [TearDown]
        public void TearDown()
        {
            Context.Dispose();            
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

        public void Dispose()
        {
            _server?.Dispose();
        }
    }
}
