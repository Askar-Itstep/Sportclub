namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewFirst : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Administrations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        BirthDay = c.DateTime(nullable: false),
                        Phone = c.String(),
                        Email = c.String(),
                        Gender = c.Int(nullable: false),
                        Login = c.String(),
                        Password = c.String(),
                        RoleId = c.Int(nullable: false),
                        Token = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.GraphTranings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CoacheId = c.Int(),
                        GymsId = c.Int(),
                        DayOfWeek = c.Int(nullable: false),
                        TimeBegin = c.DateTime(nullable: false),
                        TimeEnd = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Coaches", t => t.CoacheId)
                .ForeignKey("dbo.Gyms", t => t.GymsId)
                .Index(t => t.CoacheId)
                .Index(t => t.GymsId);
            
            CreateTable(
                "dbo.Coaches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        SpecializationId = c.Int(nullable: false),
                        TimeWork = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Specializations", t => t.SpecializationId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.SpecializationId);
            
            CreateTable(
                "dbo.Specializations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Gyms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GymName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GraphTraningClients",
                c => new
                    {
                        GraphTraning_Id = c.Int(nullable: false),
                        Clients_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GraphTraning_Id, t.Clients_Id })
                .ForeignKey("dbo.GraphTranings", t => t.GraphTraning_Id, cascadeDelete: true)
                .ForeignKey("dbo.Clients", t => t.Clients_Id, cascadeDelete: true)
                .Index(t => t.GraphTraning_Id)
                .Index(t => t.Clients_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clients", "UserId", "dbo.Users");
            DropForeignKey("dbo.GraphTranings", "GymsId", "dbo.Gyms");
            DropForeignKey("dbo.GraphTranings", "CoacheId", "dbo.Coaches");
            DropForeignKey("dbo.Coaches", "UserId", "dbo.Users");
            DropForeignKey("dbo.Coaches", "SpecializationId", "dbo.Specializations");
            DropForeignKey("dbo.GraphTraningClients", "Clients_Id", "dbo.Clients");
            DropForeignKey("dbo.GraphTraningClients", "GraphTraning_Id", "dbo.GraphTranings");
            DropForeignKey("dbo.Administrations", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropIndex("dbo.GraphTraningClients", new[] { "Clients_Id" });
            DropIndex("dbo.GraphTraningClients", new[] { "GraphTraning_Id" });
            DropIndex("dbo.Coaches", new[] { "SpecializationId" });
            DropIndex("dbo.Coaches", new[] { "UserId" });
            DropIndex("dbo.GraphTranings", new[] { "GymsId" });
            DropIndex("dbo.GraphTranings", new[] { "CoacheId" });
            DropIndex("dbo.Clients", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.Administrations", new[] { "UserId" });
            DropTable("dbo.GraphTraningClients");
            DropTable("dbo.Gyms");
            DropTable("dbo.Specializations");
            DropTable("dbo.Coaches");
            DropTable("dbo.GraphTranings");
            DropTable("dbo.Clients");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.Administrations");
        }
    }
}
