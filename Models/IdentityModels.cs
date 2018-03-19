using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CMS.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Beacon> Beacons { get; set; }

        public DbSet<Content> Contents { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Conference> Conferences { get; set; }

        public DbSet<BlobContent> BlobContents { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<URL> GroupURIs { get; set; }

        public DbSet<ApiKey> ApiKeySet { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<Log> Log { get; set; }
    }
}