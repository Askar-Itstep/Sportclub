namespace Sportclub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixOne : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Administrations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        BirthDay = c.DateTime(nullable: false),
                        Phone = c.String(),
                        Email = c.String(),
                        _Gender = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Login = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        BirthDay = c.DateTime(nullable: false),
                        Phone = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        GraphicId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GraphTranings", t => t.GraphicId)
                .Index(t => t.GraphicId);
            
            CreateTable(
                "dbo.GraphTranings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CoacheId = c.Int(),
                        DayOfWeek = c.Int(nullable: false),
                        TimeBegin = c.DateTime(nullable: false),
                        TimeEnd = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Coaches", t => t.CoacheId)
                .Index(t => t.CoacheId);
            
            CreateTable(
                "dbo.Coaches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        BirthDay = c.DateTime(nullable: false),
                        Phone = c.String(),
                        Email = c.String(),
                        _Gender = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        SpecializationId = c.Int(nullable: false),
                        TimeWork = c.Double(nullable: false),
                        Login = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Specializations", t => t.SpecializationId, cascadeDelete: true)
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clients", "GraphicId", "dbo.GraphTranings");
            DropForeignKey("dbo.GraphTranings", "CoacheId", "dbo.Coaches");
            DropForeignKey("dbo.Coaches", "SpecializationId", "dbo.Specializations");
            DropIndex("dbo.Coaches", new[] { "SpecializationId" });
            DropIndex("dbo.GraphTranings", new[] { "CoacheId" });
            DropIndex("dbo.Clients", new[] { "GraphicId" });
            DropTable("dbo.Gyms");
            DropTable("dbo.Specializations");
            DropTable("dbo.Coaches");
            DropTable("dbo.GraphTranings");
            DropTable("dbo.Clients");
            DropTable("dbo.Administrations");
        }
    }
}
