using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Odin.Data.Core.Models;

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
            else if (User.IsInRole(UserRoles.Consultant))
            {
                return UserRoles.Consultant;
            }
            if (User.IsInRole(UserRoles.GlobalSupplyChain))
            {
                return UserRoles.GlobalSupplyChain;
            }
            else if (User.IsInRole(UserRoles.ProgramManager))
            {
                return UserRoles.ProgramManager;
            }
            else
            {
                return UserRoles.Transferee;
            }
        }
    }
}