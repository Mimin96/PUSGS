namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LocationMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        IdLocation = c.Int(nullable: false, identity: true),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.IdLocation);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Locations");
        }
    }
}
