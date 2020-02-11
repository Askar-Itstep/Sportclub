namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddManyToManyClientsGraphics : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Clients", "GraphicId", "dbo.GraphTranings");
            DropIndex("dbo.Clients", new[] { "GraphicId" });
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
            
            DropColumn("dbo.Clients", "GraphicId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clients", "GraphicId", c => c.Int());
            DropForeignKey("dbo.GraphTraningClients", "Clients_Id", "dbo.Clients");
            DropForeignKey("dbo.GraphTraningClients", "GraphTraning_Id", "dbo.GraphTranings");
            DropIndex("dbo.GraphTraningClients", new[] { "Clients_Id" });
            DropIndex("dbo.GraphTraningClients", new[] { "GraphTraning_Id" });
            DropTable("dbo.GraphTraningClients");
            CreateIndex("dbo.Clients", "GraphicId");
            AddForeignKey("dbo.Clients", "GraphicId", "dbo.GraphTranings", "Id");
        }
    }
}
