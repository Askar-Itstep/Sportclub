namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AltNavigatPropGymsId : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.GraphTranings", name: "GymId", newName: "GymsId");
            RenameIndex(table: "dbo.GraphTranings", name: "IX_GymId", newName: "IX_GymsId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.GraphTranings", name: "IX_GymsId", newName: "IX_GymId");
            RenameColumn(table: "dbo.GraphTranings", name: "GymsId", newName: "GymId");
        }
    }
}
