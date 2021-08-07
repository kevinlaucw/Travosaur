namespace Travosaur.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.City",
                c => new
                    {
                        CityID = c.Int(nullable: false, identity: true),
                        CityName = c.String(nullable: false, maxLength: 100, unicode: false),
                        StateID = c.Int(nullable: false),
                        CountryID = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CityID)
                .ForeignKey("dbo.Country", t => t.CountryID, cascadeDelete: true)
                .ForeignKey("dbo.State", t => t.StateID, cascadeDelete: true)
                .Index(t => t.StateID)
                .Index(t => t.CountryID);
            
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        CountryID = c.Int(nullable: false, identity: true),
                        CountryName = c.String(nullable: false, maxLength: 100, unicode: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CountryID);
            
            CreateTable(
                "dbo.State",
                c => new
                    {
                        StateID = c.Int(nullable: false, identity: true),
                        StateName = c.String(nullable: false, maxLength: 100, unicode: false),
                        CountryID = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.StateID);
            
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        ContactId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Zip = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.ContactId);
            
            CreateTable(
                "dbo.Currency",
                c => new
                    {
                        CurrencyID = c.Int(nullable: false, identity: true),
                        CurrencyCode = c.String(maxLength: 3, fixedLength: true),
                        Name = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CurrencyID);
            
            CreateTable(
                "dbo.Image",
                c => new
                    {
                        ImageID = c.Int(nullable: false, identity: true),
                        ImageName = c.String(maxLength: 250),
                        ImageSize = c.Int(),
                        ImageData = c.Binary(),
                    })
                .PrimaryKey(t => t.ImageID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Subscriber",
                c => new
                    {
                        SubscriberID = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.SubscriberID);
            
            CreateTable(
                "dbo.Tour",
                c => new
                    {
                        TourID = c.Int(nullable: false, identity: true),
                        TourName = c.String(maxLength: 100),
                        ShortDescription = c.String(maxLength: 200),
                        Description = c.String(maxLength: 4000),
                        Highlight = c.String(maxLength: 2000),
                        Include = c.String(maxLength: 2000),
                        Exclude = c.String(maxLength: 2000),
                        DepartureInfo = c.String(maxLength: 2000),
                        TourCode = c.String(maxLength: 50, unicode: false),
                        TourTypeID = c.Int(),
                        CityID = c.Int(nullable: false),
                        Currency = c.String(maxLength: 10, unicode: false),
                        Price = c.Decimal(nullable: false, storeType: "money"),
                        StartDate = c.DateTime(storeType: "date"),
                        EndDate = c.DateTime(storeType: "date"),
                        DateCreated = c.DateTime(),
                        CreatedBy = c.String(maxLength: 100, unicode: false),
                        LastUpdated = c.DateTime(),
                        LastUpdatedBy = c.String(maxLength: 100, unicode: false),
                        DurationDay = c.Int(nullable: false),
                        DurationHour = c.Byte(nullable: false),
                        ImageData = c.Binary(),
                        RedirectURL = c.String(maxLength: 500),
                        Term = c.String(maxLength: 4000),
                        English = c.Boolean(nullable: false),
                        Chinese = c.Boolean(nullable: false),
                        Indian = c.Boolean(nullable: false),
                        Japanese = c.Boolean(nullable: false),
                        Korean = c.Boolean(nullable: false),
                        Active = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TourID)
                .ForeignKey("dbo.City", t => t.CityID, cascadeDelete: true)
                .Index(t => t.CityID);
            
            CreateTable(
                "dbo.vw_Tour",
                c => new
                    {
                        TourID = c.Int(nullable: false, identity: true),
                        TourName = c.String(),
                        ShortDescription = c.String(),
                        Description = c.String(),
                        Highlight = c.String(),
                        Include = c.String(),
                        Exclude = c.String(),
                        DepartureInfo = c.String(),
                        TourCode = c.String(),
                        TourTypeID = c.Int(),
                        CityID = c.Int(nullable: false),
                        CityName = c.String(),
                        StateID = c.Int(),
                        StateName = c.String(),
                        CountryID = c.Int(),
                        CountryName = c.String(),
                        Currency = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        DateCreated = c.DateTime(),
                        CreatedBy = c.String(),
                        CreatedByDisplayName = c.String(),
                        LastUpdated = c.DateTime(),
                        LastUpdatedBy = c.String(),
                        DurationDay = c.Int(nullable: false),
                        DurationHour = c.Byte(nullable: false),
                        ImageData = c.Binary(),
                        RedirectURL = c.String(),
                        Term = c.String(),
                        English = c.Boolean(nullable: false),
                        Chinese = c.Boolean(nullable: false),
                        Indian = c.Boolean(nullable: false),
                        Japanese = c.Boolean(nullable: false),
                        Korean = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TourID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        DisplayName = c.String(),
                        Subscribed = c.Boolean(nullable: false),
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
                .PrimaryKey(t => t.UserId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.Tour", "CityID", "dbo.City");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.City", "StateID", "dbo.State");
            DropForeignKey("dbo.City", "CountryID", "dbo.Country");
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.Tour", new[] { "CityID" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.City", new[] { "CountryID" });
            DropIndex("dbo.City", new[] { "StateID" });
            DropTable("dbo.UserLogins");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.vw_Tour");
            DropTable("dbo.Tour");
            DropTable("dbo.Subscriber");
            DropTable("dbo.UserRoles");
            DropTable("dbo.Roles");
            DropTable("dbo.Image");
            DropTable("dbo.Currency");
            DropTable("dbo.Contact");
            DropTable("dbo.State");
            DropTable("dbo.Country");
            DropTable("dbo.City");
        }
    }
}
