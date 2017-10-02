using System.ComponentModel.DataAnnotations;
using Odin.Data.Core.Models;

namespace Odin.Data.Validations
{
    public class HasManagerRoleAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value.Equals(UserRoles.GlobalSupplyChain) || value.Equals(UserRoles.ProgramManager);
        }
    }
}