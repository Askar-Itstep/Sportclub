namespace Sportclub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixRole : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Users", "RoleId");
            AddForeignKey("dbo.Users", "RoleId", "dbo.Roles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropIndex("dbo.Users", new[] { "RoleId" });
        }
    }
}
