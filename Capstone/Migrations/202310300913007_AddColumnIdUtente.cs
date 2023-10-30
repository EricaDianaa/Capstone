namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnIdUtente : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Eventi", "IdUtenti", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Eventi", "IdUtenti");
        }
    }
}
