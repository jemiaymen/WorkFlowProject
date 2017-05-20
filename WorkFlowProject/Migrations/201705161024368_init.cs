namespace WorkFlowProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AchaFournisseurs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AchatID = c.Int(nullable: false),
                        FournisseurID = c.Int(nullable: false),
                        Price = c.Single(nullable: false),
                        Remise = c.Single(nullable: false),
                        Delais = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Achats", t => t.AchatID, cascadeDelete: true)
                .ForeignKey("dbo.Fournisseurs", t => t.FournisseurID, cascadeDelete: true)
                .Index(t => t.AchatID)
                .Index(t => t.FournisseurID);
            
            CreateTable(
                "dbo.Achats",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DepartmentID = c.Int(nullable: false),
                        Des = c.String(nullable: false, maxLength: 500),
                        Categ = c.String(nullable: false, maxLength: 500),
                        DtAcha = c.DateTime(nullable: false),
                        Creation = c.Boolean(nullable: false),
                        LieuLiv = c.String(nullable: false, maxLength: 500),
                        Imp = c.String(nullable: false, maxLength: 500),
                        Qte = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Departments", t => t.DepartmentID, cascadeDelete: true)
                .Index(t => t.DepartmentID);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Dep = c.String(nullable: false, maxLength: 100),
                        Budget = c.Single(nullable: false),
                        Depense = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Fournisseurs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nom_frn = c.String(nullable: false, maxLength: 100),
                        Adress_frn = c.String(nullable: false, maxLength: 500),
                        Mail_frn = c.String(nullable: false),
                        Tel_frn = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Avis",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AchatID = c.Int(nullable: false),
                        UserID = c.String(maxLength: 128),
                        Lbl = c.String(nullable: false, maxLength: 500),
                        Code = c.Int(nullable: false),
                        Accept = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Achats", t => t.AchatID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.AchatID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Login = c.String(),
                        Address = c.String(),
                        Datenaiss = c.DateTime(nullable: false),
                        Tel = c.String(),
                        SignatureUser = c.String(),
                        DepartmentID = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentID, cascadeDelete: true)
                .Index(t => t.DepartmentID)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Lbl = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Lbl, unique: true, name: "IndexLblCategory");
            
            CreateTable(
                "dbo.CategoryInFournisseurs",
                c => new
                    {
                        CategoryID = c.Int(nullable: false),
                        FournisseurID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CategoryID, t.FournisseurID })
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.Fournisseurs", t => t.FournisseurID, cascadeDelete: true)
                .Index(t => t.CategoryID)
                .Index(t => t.FournisseurID);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AchatID = c.Int(nullable: false),
                        Lbl = c.String(nullable: false, maxLength: 500),
                        Dt = c.DateTime(nullable: false),
                        Etat = c.Boolean(nullable: false),
                        Role = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Achats", t => t.AchatID, cascadeDelete: true)
                .Index(t => t.AchatID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Notifications", "AchatID", "dbo.Achats");
            DropForeignKey("dbo.CategoryInFournisseurs", "FournisseurID", "dbo.Fournisseurs");
            DropForeignKey("dbo.CategoryInFournisseurs", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.Avis", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "DepartmentID", "dbo.Departments");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Avis", "AchatID", "dbo.Achats");
            DropForeignKey("dbo.AchaFournisseurs", "FournisseurID", "dbo.Fournisseurs");
            DropForeignKey("dbo.AchaFournisseurs", "AchatID", "dbo.Achats");
            DropForeignKey("dbo.Achats", "DepartmentID", "dbo.Departments");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Notifications", new[] { "AchatID" });
            DropIndex("dbo.CategoryInFournisseurs", new[] { "FournisseurID" });
            DropIndex("dbo.CategoryInFournisseurs", new[] { "CategoryID" });
            DropIndex("dbo.Categories", "IndexLblCategory");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "DepartmentID" });
            DropIndex("dbo.Avis", new[] { "UserID" });
            DropIndex("dbo.Avis", new[] { "AchatID" });
            DropIndex("dbo.Achats", new[] { "DepartmentID" });
            DropIndex("dbo.AchaFournisseurs", new[] { "FournisseurID" });
            DropIndex("dbo.AchaFournisseurs", new[] { "AchatID" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Notifications");
            DropTable("dbo.CategoryInFournisseurs");
            DropTable("dbo.Categories");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Avis");
            DropTable("dbo.Fournisseurs");
            DropTable("dbo.Departments");
            DropTable("dbo.Achats");
            DropTable("dbo.AchaFournisseurs");
        }
    }
}
