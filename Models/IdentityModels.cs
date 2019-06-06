/*
 *  Domain Model for the Identity Framework.
 *  This is where the Application's DB Context is 
 *  declared, along with the DB Sets.
 */


using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using JTicket.Models;

namespace JTicket.Models
{
    // You can add profile data for the user by adding more properties 
    // to your ApplicationUser class, please visit
    // https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined 
            // in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, 
                DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    /// <summary>
    /// Class 
    /// <c>ApplicationDbContext</c> 
    /// The Application's DB Context. Gateway to database.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Ticket> Tickets { get; set; }    // Ticket table DbSet


        /// <summary>
        /// Method 
        /// <c>ApplicationDbContext</c> 
        /// Constructor for the ApplicationDbContext class.
        /// </summary>
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {}


        /// <summary>
        /// Method 
        /// <c>Create</c> 
        /// Static method for instantiating new ApplicationDbContext objects.
        /// </summary>
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
