namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnUtenti : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Utenti", "VCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Utenti", "VCode");
        }
    }
}
