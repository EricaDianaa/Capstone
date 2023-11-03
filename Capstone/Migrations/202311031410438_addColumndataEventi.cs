namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addColumndataEventi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Eventi", "DataDa", c => c.DateTime(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Eventi", "DataDa");
        }
    }
}
