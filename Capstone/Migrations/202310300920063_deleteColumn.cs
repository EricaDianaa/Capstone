namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteColumn : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Eventi", "IdUtenti");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Eventi", "IdUtenti", c => c.Int(nullable: false));
        }
    }
}
