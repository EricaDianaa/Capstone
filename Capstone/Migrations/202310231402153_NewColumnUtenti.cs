namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewColumnUtenti : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Utenti", "IsAzienda", c => c.Boolean(nullable: false));
            AddColumn("dbo.Utenti", "CodiceFiscale", c => c.String(maxLength: 16));
            AddColumn("dbo.Utenti", "PartitaIva", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Utenti", "PartitaIva");
            DropColumn("dbo.Utenti", "CodiceFiscale");
            DropColumn("dbo.Utenti", "IsAzienda");
        }
    }
}
