namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletedata : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Eventi", "DataDa");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Eventi", "DataDa", c => c.DateTime(nullable: false));
        }
    }
}
