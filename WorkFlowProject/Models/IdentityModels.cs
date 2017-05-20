using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkFlowProject.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public async Task<ClaimsIdentity>GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            return userIdentity;
        }

        public string Login { get; set; }
        public string Address { get; set; }
        public DateTime Datenaiss { get; set; }
        public string Tel { get; set; }
        public string SignatureUser { get; set; }

        [ForeignKey("Department")]
        public int DepartmentID { get; set; }


        public virtual Department Department { get; set; }
    }

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



        public System.Data.Entity.DbSet<Fournisseur> Fournisseurs { get; set; }

        public System.Data.Entity.DbSet<Category> Categories { get; set; }

        public System.Data.Entity.DbSet<CategoryInFournisseur> CategoryInFournisseur { get; set; }

        public System.Data.Entity.DbSet<Department> Departments { get; set; }

        public System.Data.Entity.DbSet<Achat> Achats { get; set; }

        public System.Data.Entity.DbSet<Notification> Notifications { get; set; }

        public System.Data.Entity.DbSet<Avis> Avis { get; set; }

        public System.Data.Entity.DbSet<AchaFournisseur> AchaFournisseurs { get; set; }

    }
}