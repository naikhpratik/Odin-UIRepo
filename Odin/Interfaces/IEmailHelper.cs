using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Interfaces
{
    public interface IEmailHelper
    {
        string SendEmail_SG(string to, string subject, string content, string contentType);
        //string SendEmail_SG_Text(string to, string subject, string content);
    }
}
