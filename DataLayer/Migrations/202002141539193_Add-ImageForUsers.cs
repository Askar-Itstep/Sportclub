namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImageForUsers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Filename = c.String(),
                        ImageData = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "ImageId", c => c.Int(nullable: true));
            CreateIndex("dbo.Users", "ImageId");
            AddForeignKey("dbo.Users", "ImageId", "dbo.Images", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "ImageId", "dbo.Images");
            DropIndex("dbo.Users", new[] { "ImageId" });
            DropColumn("dbo.Users", "ImageId");
            DropTable("dbo.Images");
        }
    }
}
