namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNavigatPropGymId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GraphTranings", "GymId", c => c.Int());
            CreateIndex("dbo.GraphTranings", "GymId");
            AddForeignKey("dbo.GraphTranings", "GymId", "dbo.Gyms", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GraphTranings", "GymId", "dbo.Gyms");
            DropIndex("dbo.GraphTranings", new[] { "GymId" });
            DropColumn("dbo.GraphTranings", "GymId");
        }
    }
}
