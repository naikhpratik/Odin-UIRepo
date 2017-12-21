using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Odin.Data.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int? SeContactUid { get; set; }

        public ICollection<UserNotification> UserNotifications { get; set; }


        public ApplicationUser()
        {
            UserNotifications = new Collection<UserNotification>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("FullName", this.FullName));
            return userIdentity;
        }

        public string FullName => $"{FirstName} {LastName}";

        public void Notify(Notification notification)
        {
            UserNotifications.Add(new UserNotification(this, notification));
        }

    }
}