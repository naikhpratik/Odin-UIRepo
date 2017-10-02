using System.Threading.Tasks;


namespace Odin.Interfaces
{
    public interface IAccountHelper
    {
        Task<string> SendEmailConfirmationTokenAsync(string userId); // where T : ApplicationUser;
    }
}
