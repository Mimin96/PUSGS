namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PricelistMigration : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PricelistPrices", newName: "PricePricelists");
            DropPrimaryKey("dbo.PricePricelists");
            AddPrimaryKey("dbo.PricePricelists", new[] { "Price_IdPrice", "Pricelist_IdPricelist" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.PricePricelists");
            AddPrimaryKey("dbo.PricePricelists", new[] { "Pricelist_IdPricelist", "Price_IdPrice" });
            RenameTable(name: "dbo.PricePricelists", newName: "PricelistPrices");
        }
    }
}
