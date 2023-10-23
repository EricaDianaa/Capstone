namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class utentiTypePartitaIva : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Utenti", "PartitaIva", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Utenti", "PartitaIva", c => c.Boolean(nullable: false));
        }
    }
}
