using Odin.Data.Core.Models;
using System.Security.Principal;

namespace Odin.Extensions
{
    public static class UserExtensions
    {
        public static string GetUserRole(this IPrincipal User)
        {
            if (User.IsInRole(UserRoles.Admin))
            {
                return UserRoles.Admin;
            }

            if (User.IsInRole(UserRoles.Consultant))
            {
                return UserRoles.Consultant;
            }

            if (User.IsInRole(UserRoles.GlobalSupplyChain))
            {
                return UserRoles.GlobalSupplyChain;
            }

            if (User.IsInRole(UserRoles.ProgramManager))
            {
                return UserRoles.ProgramManager;
            }

            if (User.IsInRole(UserRoles.Transferee))
            {
                return UserRoles.Transferee;
            }

            return null;
        }
    }
}