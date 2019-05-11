namespace JTicket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConsolidatedTicket : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tickets", "TicketDescriptionId", "dbo.TicketDescriptions");
            DropIndex("dbo.Tickets", new[] { "TicketDescriptionId" });
            AddColumn("dbo.Tickets", "Description", c => c.String());
            AddColumn("dbo.Tickets", "Comments", c => c.String());
            AddColumn("dbo.Tickets", "isOpen", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tickets", "severity", c => c.Int(nullable: false));
            AddColumn("dbo.Tickets", "creationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Tickets", "lastModified", c => c.DateTime(nullable: false));
            AddColumn("dbo.Tickets", "Title", c => c.String());
            DropColumn("dbo.Tickets", "TicketDescriptionId");
            DropTable("dbo.TicketDescriptions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TicketDescriptions",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Description = c.String(),
                        Comments = c.String(),
                        isOpen = c.Boolean(nullable: false),
                        severity = c.Int(nullable: false),
                        creationDate = c.DateTime(nullable: false),
                        lastModified = c.DateTime(nullable: false),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Tickets", "TicketDescriptionId", c => c.Byte(nullable: false));
            DropColumn("dbo.Tickets", "Title");
            DropColumn("dbo.Tickets", "lastModified");
            DropColumn("dbo.Tickets", "creationDate");
            DropColumn("dbo.Tickets", "severity");
            DropColumn("dbo.Tickets", "isOpen");
            DropColumn("dbo.Tickets", "Comments");
            DropColumn("dbo.Tickets", "Description");
            CreateIndex("dbo.Tickets", "TicketDescriptionId");
            AddForeignKey("dbo.Tickets", "TicketDescriptionId", "dbo.TicketDescriptions", "Id", cascadeDelete: true);
        }
    }
}
