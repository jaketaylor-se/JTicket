namespace JTicket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTicketDescription : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Tickets");
            AlterColumn("dbo.Tickets", "Id", c => c.Byte(nullable: false));
            AlterColumn("dbo.Tickets", "TicketDescriptionId", c => c.Byte(nullable: false));
            AddPrimaryKey("dbo.Tickets", "Id");
            CreateIndex("dbo.Tickets", "TicketDescriptionId");
            AddForeignKey("dbo.Tickets", "TicketDescriptionId", "dbo.TicketDescriptions", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "TicketDescriptionId", "dbo.TicketDescriptions");
            DropIndex("dbo.Tickets", new[] { "TicketDescriptionId" });
            DropPrimaryKey("dbo.Tickets");
            AlterColumn("dbo.Tickets", "TicketDescriptionId", c => c.Int(nullable: false));
            AlterColumn("dbo.Tickets", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Tickets", "Id");
        }
    }
}
