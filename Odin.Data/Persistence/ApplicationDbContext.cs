using Microsoft.AspNet.Identity.EntityFramework;
using MyDwellworks.Data.Core.Models;

namespace MyDwellworks.Data.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}