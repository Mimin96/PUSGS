namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SchaduleLinesMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        IdSchedule = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Day = c.Int(nullable: false),
                        DepartureTime = c.String(),
                    })
                .PrimaryKey(t => t.IdSchedule);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Schedules");
        }
    }
}
