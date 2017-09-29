using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Interfaces
{
    public interface IConfigHelper
    {
        // Token for getting updates from se
        string GetSeApiToken();
        string GetSendGridAPIKey();
    }
}
