using Odin.Data.Core.Models;

namespace Odin.PropBotWebJob.Interfaces
{
    public interface IBotHelper
    {
        IBot GetBot(string rawUrl);
        Photo SaveImageToStore(string imgUrl);
    }
}