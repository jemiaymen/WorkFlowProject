namespace WorkFlowProject.Migrations
{
    
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<WorkFlowProject.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(WorkFlowProject.Models.ApplicationDbContext context)
        {
            context.Roles.AddOrUpdate(
                 r => r.Name,
                 new IdentityRole { Name = "Responsable Achat" },
                 new IdentityRole { Name = "Demandeur" },
                 new IdentityRole { Name = "Direction Administrative et Financière" },
                 new IdentityRole { Name = "Direction Audit" },
                 new IdentityRole { Name = "Direction Generale" }
                 );
        }
    }
}
