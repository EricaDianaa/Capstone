namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Eventi", "DataDa", c => c.DateTime());
            AlterColumn("dbo.Eventi", "DataEvento", c => c.DateTime(storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Eventi", "DataEvento", c => c.DateTime(nullable: false, storeType: "date"));
            DropColumn("dbo.Eventi", "DataDa");
        }
    }
}
