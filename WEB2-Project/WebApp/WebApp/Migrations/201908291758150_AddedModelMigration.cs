namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedModelMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        IdLocation = c.Int(nullable: false, identity: true),
                        Address = c.String(),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.IdLocation);
            
            CreateTable(
                "dbo.Pricelists",
                c => new
                    {
                        IdPriceList = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        InUse = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdPriceList);
            
            CreateTable(
                "dbo.Prices",
                c => new
                    {
                        IdPrice = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Value = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.IdPrice);
            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        IdSchadule = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Day = c.Int(nullable: false),
                        DepartureTime = c.String(),
                        Line_IdLine = c.Int(),
                        Line_IdLine1 = c.Int(),
                    })
                .PrimaryKey(t => t.IdSchadule)
                .ForeignKey("dbo.Lines", t => t.Line_IdLine)
                .ForeignKey("dbo.Lines", t => t.Line_IdLine1)
                .Index(t => t.Line_IdLine)
                .Index(t => t.Line_IdLine1);
            
            CreateTable(
                "dbo.Lines",
                c => new
                    {
                        IdLine = c.Int(nullable: false, identity: true),
                        Number = c.String(),
                        RouteType = c.Int(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Schedule_IdSchadule = c.Int(),
                    })
                .PrimaryKey(t => t.IdLine)
                .ForeignKey("dbo.Schedules", t => t.Schedule_IdSchadule)
                .Index(t => t.Schedule_IdSchadule);
            
            CreateTable(
                "dbo.Stations",
                c => new
                    {
                        IdStation = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.IdStation);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        IdTicket = c.Int(nullable: false, identity: true),
                        From = c.DateTime(nullable: false),
                        To = c.DateTime(nullable: false),
                        Passenger = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.IdTicket);
            
            CreateTable(
                "dbo.PricePricelists",
                c => new
                    {
                        Price_IdPrice = c.Int(nullable: false),
                        Pricelist_IdPriceList = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Price_IdPrice, t.Pricelist_IdPriceList })
                .ForeignKey("dbo.Prices", t => t.Price_IdPrice, cascadeDelete: true)
                .ForeignKey("dbo.Pricelists", t => t.Pricelist_IdPriceList, cascadeDelete: true)
                .Index(t => t.Price_IdPrice)
                .Index(t => t.Pricelist_IdPriceList);
            
            CreateTable(
                "dbo.StationLines",
                c => new
                    {
                        Station_IdStation = c.Int(nullable: false),
                        Line_IdLine = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Station_IdStation, t.Line_IdLine })
                .ForeignKey("dbo.Stations", t => t.Station_IdStation, cascadeDelete: true)
                .ForeignKey("dbo.Lines", t => t.Line_IdLine, cascadeDelete: true)
                .Index(t => t.Station_IdStation)
                .Index(t => t.Line_IdLine);
            
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
            AddColumn("dbo.AspNetUsers", "Password", c => c.String());
            AddColumn("dbo.AspNetUsers", "BirthdayDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "Address", c => c.String());
            AddColumn("dbo.AspNetUsers", "Picture", c => c.String());
            AddColumn("dbo.AspNetUsers", "PassengerType", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "State", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Lines", "Schedule_IdSchadule", "dbo.Schedules");
            DropForeignKey("dbo.Schedules", "Line_IdLine1", "dbo.Lines");
            DropForeignKey("dbo.StationLines", "Line_IdLine", "dbo.Lines");
            DropForeignKey("dbo.StationLines", "Station_IdStation", "dbo.Stations");
            DropForeignKey("dbo.Schedules", "Line_IdLine", "dbo.Lines");
            DropForeignKey("dbo.PricePricelists", "Pricelist_IdPriceList", "dbo.Pricelists");
            DropForeignKey("dbo.PricePricelists", "Price_IdPrice", "dbo.Prices");
            DropIndex("dbo.StationLines", new[] { "Line_IdLine" });
            DropIndex("dbo.StationLines", new[] { "Station_IdStation" });
            DropIndex("dbo.PricePricelists", new[] { "Pricelist_IdPriceList" });
            DropIndex("dbo.PricePricelists", new[] { "Price_IdPrice" });
            DropIndex("dbo.Lines", new[] { "Schedule_IdSchadule" });
            DropIndex("dbo.Schedules", new[] { "Line_IdLine1" });
            DropIndex("dbo.Schedules", new[] { "Line_IdLine" });
            DropColumn("dbo.AspNetUsers", "State");
            DropColumn("dbo.AspNetUsers", "PassengerType");
            DropColumn("dbo.AspNetUsers", "Picture");
            DropColumn("dbo.AspNetUsers", "Address");
            DropColumn("dbo.AspNetUsers", "BirthdayDate");
            DropColumn("dbo.AspNetUsers", "Password");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "Name");
            DropTable("dbo.StationLines");
            DropTable("dbo.PricePricelists");
            DropTable("dbo.Tickets");
            DropTable("dbo.Stations");
            DropTable("dbo.Lines");
            DropTable("dbo.Schedules");
            DropTable("dbo.Prices");
            DropTable("dbo.Pricelists");
            DropTable("dbo.Locations");
        }
    }
}
