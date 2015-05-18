namespace Lab2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Firstname = c.String(),
                        Lastname = c.String(),
                        Account_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IdentityUsers", t => t.Account_Id)
                .Index(t => t.Account_Id);
            
            CreateTable(
                "dbo.IdentityUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
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
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IdentityUsers", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.IdentityUsers", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.IdentityUsers", t => t.IdentityUser_Id)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.Memories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Date = c.DateTime(nullable: false),
                        Position_X = c.Double(nullable: false),
                        Position_Y = c.Double(nullable: false),
                        Vacation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vacations", t => t.Vacation_Id)
                .Index(t => t.Vacation_Id);
            
            CreateTable(
                "dbo.Media",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Type = c.String(),
                        Url = c.String(),
                        Width = c.Int(),
                        Height = c.Int(),
                        Duration = c.Double(),
                        Container = c.String(),
                        Codec = c.String(),
                        Bitrate = c.Double(),
                        Channels = c.Int(),
                        SamplingRate = c.Double(),
                        Duration1 = c.Double(),
                        Container1 = c.String(),
                        VideoCodec = c.String(),
                        VideoBitrate = c.Double(),
                        Width1 = c.Int(),
                        Height1 = c.Int(),
                        FrameRate = c.Double(),
                        AudioCodec = c.String(),
                        AudioBitrate = c.Double(),
                        Channels1 = c.Int(),
                        SamplingRate1 = c.Double(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Memories", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.SearchTags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tag = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Vacations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Place = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.UserFriends",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        FriendId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.FriendId })
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.Users", t => t.FriendId)
                .Index(t => t.UserId)
                .Index(t => t.FriendId);
            
            CreateTable(
                "dbo.SearchTagMemories",
                c => new
                    {
                        SearchTag_Id = c.Int(nullable: false),
                        Memory_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SearchTag_Id, t.Memory_Id })
                .ForeignKey("dbo.SearchTags", t => t.SearchTag_Id, cascadeDelete: true)
                .ForeignKey("dbo.Memories", t => t.Memory_Id, cascadeDelete: true)
                .Index(t => t.SearchTag_Id)
                .Index(t => t.Memory_Id);
            
            CreateTable(
                "dbo.MemoryUsers",
                c => new
                    {
                        Memory_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Memory_Id, t.User_Id })
                .ForeignKey("dbo.Memories", t => t.Memory_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Memory_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        VacationUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IdentityUsers", t => t.Id)
                .ForeignKey("dbo.Users", t => t.VacationUser_Id)
                .Index(t => t.Id)
                .Index(t => t.VacationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "VacationUser_Id", "dbo.Users");
            DropForeignKey("dbo.AspNetUsers", "Id", "dbo.IdentityUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Vacations", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Memories", "Vacation_Id", "dbo.Vacations");
            DropForeignKey("dbo.MemoryUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.MemoryUsers", "Memory_Id", "dbo.Memories");
            DropForeignKey("dbo.SearchTagMemories", "Memory_Id", "dbo.Memories");
            DropForeignKey("dbo.SearchTagMemories", "SearchTag_Id", "dbo.SearchTags");
            DropForeignKey("dbo.Media", "Id", "dbo.Memories");
            DropForeignKey("dbo.UserFriends", "FriendId", "dbo.Users");
            DropForeignKey("dbo.UserFriends", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "Account_Id", "dbo.IdentityUsers");
            DropForeignKey("dbo.AspNetUserRoles", "IdentityUser_Id", "dbo.IdentityUsers");
            DropForeignKey("dbo.AspNetUserLogins", "IdentityUser_Id", "dbo.IdentityUsers");
            DropForeignKey("dbo.AspNetUserClaims", "IdentityUser_Id", "dbo.IdentityUsers");
            DropIndex("dbo.AspNetUsers", new[] { "VacationUser_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Id" });
            DropIndex("dbo.MemoryUsers", new[] { "User_Id" });
            DropIndex("dbo.MemoryUsers", new[] { "Memory_Id" });
            DropIndex("dbo.SearchTagMemories", new[] { "Memory_Id" });
            DropIndex("dbo.SearchTagMemories", new[] { "SearchTag_Id" });
            DropIndex("dbo.UserFriends", new[] { "FriendId" });
            DropIndex("dbo.UserFriends", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Vacations", new[] { "User_Id" });
            DropIndex("dbo.Media", new[] { "Id" });
            DropIndex("dbo.Memories", new[] { "Vacation_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "IdentityUser_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "IdentityUser_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "IdentityUser_Id" });
            DropIndex("dbo.IdentityUsers", "UserNameIndex");
            DropIndex("dbo.Users", new[] { "Account_Id" });
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.MemoryUsers");
            DropTable("dbo.SearchTagMemories");
            DropTable("dbo.UserFriends");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Vacations");
            DropTable("dbo.SearchTags");
            DropTable("dbo.Media");
            DropTable("dbo.Memories");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.IdentityUsers");
            DropTable("dbo.Users");
        }
    }
}
