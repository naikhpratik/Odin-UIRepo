using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Odin.IntegrationTests.Helpers
{
    internal class HttpPostedFileBaseTesting : HttpPostedFileBase
    {
        private Stream stream;
        private string contentType;
        private string fileName;

        public HttpPostedFileBaseTesting(Stream stream, string contentType, string fileName)
        {
            this.stream = stream;
            this.contentType = contentType;
            this.fileName = fileName;
        }

        public override int ContentLength => (int)stream.Length;

        public override string ContentType => contentType;

        public override string FileName => fileName;

        public override Stream InputStream => stream;
    }
}
