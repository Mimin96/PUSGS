namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TicketMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        IdTicket = c.Int(nullable: false, identity: true),
                        From = c.DateTime(nullable: false),
                        To = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                        Passenger = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdTicket);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tickets");
        }
    }
}
