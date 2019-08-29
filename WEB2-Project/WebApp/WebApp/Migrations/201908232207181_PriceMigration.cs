namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PriceMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Prices",
                c => new
                    {
                        IdPrice = c.Int(nullable: false, identity: true),
                        Value = c.Double(nullable: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdPrice);
            
            CreateTable(
                "dbo.Pricelists",
                c => new
                    {
                        IdPricelist = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        InUse = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdPricelist);
            
            CreateTable(
                "dbo.PricelistPrices",
                c => new
                    {
                        Pricelist_IdPricelist = c.Int(nullable: false),
                        Price_IdPrice = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Pricelist_IdPricelist, t.Price_IdPrice })
                .ForeignKey("dbo.Pricelists", t => t.Pricelist_IdPricelist, cascadeDelete: true)
                .ForeignKey("dbo.Prices", t => t.Price_IdPrice, cascadeDelete: true)
                .Index(t => t.Pricelist_IdPricelist)
                .Index(t => t.Price_IdPrice);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PricelistPrices", "Price_IdPrice", "dbo.Prices");
            DropForeignKey("dbo.PricelistPrices", "Pricelist_IdPricelist", "dbo.Pricelists");
            DropIndex("dbo.PricelistPrices", new[] { "Price_IdPrice" });
            DropIndex("dbo.PricelistPrices", new[] { "Pricelist_IdPricelist" });
            DropTable("dbo.PricelistPrices");
            DropTable("dbo.Pricelists");
            DropTable("dbo.Prices");
        }
    }
}
