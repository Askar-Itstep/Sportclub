namespace Sportclub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtractUserIntoTable : DbMigration
    {
        public override void Up()
        {
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
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Administrations", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Clients", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Coaches", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Administrations", "UserId");
            CreateIndex("dbo.Clients", "UserId");
            CreateIndex("dbo.Coaches", "UserId");
            AddForeignKey("dbo.Administrations", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Coaches", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Clients", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            DropColumn("dbo.Administrations", "FullName");
            DropColumn("dbo.Administrations", "BirthDay");
            DropColumn("dbo.Administrations", "Phone");
            DropColumn("dbo.Administrations", "Email");
            DropColumn("dbo.Administrations", "_Gender");
            DropColumn("dbo.Administrations", "Login");
            DropColumn("dbo.Administrations", "Password");
            DropColumn("dbo.Clients", "FullName");
            DropColumn("dbo.Clients", "BirthDay");
            DropColumn("dbo.Clients", "Phone");
            DropColumn("dbo.Clients", "Email");
            DropColumn("dbo.Clients", "Password");
            DropColumn("dbo.Coaches", "FullName");
            DropColumn("dbo.Coaches", "BirthDay");
            DropColumn("dbo.Coaches", "Phone");
            DropColumn("dbo.Coaches", "Email");
            DropColumn("dbo.Coaches", "_Gender");
            DropColumn("dbo.Coaches", "Login");
            DropColumn("dbo.Coaches", "Password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Coaches", "Password", c => c.String());
            AddColumn("dbo.Coaches", "Login", c => c.String());
            AddColumn("dbo.Coaches", "_Gender", c => c.Int(nullable: false));
            AddColumn("dbo.Coaches", "Email", c => c.String());
            AddColumn("dbo.Coaches", "Phone", c => c.String());
            AddColumn("dbo.Coaches", "BirthDay", c => c.DateTime(nullable: false));
            AddColumn("dbo.Coaches", "FullName", c => c.String());
            AddColumn("dbo.Clients", "Password", c => c.String());
            AddColumn("dbo.Clients", "Email", c => c.String());
            AddColumn("dbo.Clients", "Phone", c => c.String());
            AddColumn("dbo.Clients", "BirthDay", c => c.DateTime(nullable: false));
            AddColumn("dbo.Clients", "FullName", c => c.String());
            AddColumn("dbo.Administrations", "Password", c => c.String());
            AddColumn("dbo.Administrations", "Login", c => c.String());
            AddColumn("dbo.Administrations", "_Gender", c => c.Int(nullable: false));
            AddColumn("dbo.Administrations", "Email", c => c.String());
            AddColumn("dbo.Administrations", "Phone", c => c.String());
            AddColumn("dbo.Administrations", "BirthDay", c => c.DateTime(nullable: false));
            AddColumn("dbo.Administrations", "FullName", c => c.String());
            DropForeignKey("dbo.Clients", "UserId", "dbo.Users");
            DropForeignKey("dbo.Coaches", "UserId", "dbo.Users");
            DropForeignKey("dbo.Administrations", "UserId", "dbo.Users");
            DropIndex("dbo.Coaches", new[] { "UserId" });
            DropIndex("dbo.Clients", new[] { "UserId" });
            DropIndex("dbo.Administrations", new[] { "UserId" });
            DropColumn("dbo.Coaches", "UserId");
            DropColumn("dbo.Clients", "UserId");
            DropColumn("dbo.Administrations", "UserId");
            DropTable("dbo.Users");
        }
    }
}
