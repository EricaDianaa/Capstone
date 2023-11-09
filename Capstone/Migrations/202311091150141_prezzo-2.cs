namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class prezzo2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Eventi", "Prezzo", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Eventi", "Prezzo", c => c.Decimal(nullable: false, precision: 19, scale: 2));
        }
    }
}
