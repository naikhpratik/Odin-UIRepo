using System.Collections.Generic;
using Odin.Data.Core.Models;

namespace Odin.PropBotWebJob.Interfaces
{
    public interface IBot
    {
        Property Bot();
        IEnumerable<string> BotImages();
    }
}
