using System.Threading.Tasks;


namespace Odin.Interfaces
{
    public interface IAccountHelper
    {
        Task<string> SendEmailResetTokenAsync(string userId);
        Task<string> SendEmailCreateTokenAsync(string userId);
    }
}
