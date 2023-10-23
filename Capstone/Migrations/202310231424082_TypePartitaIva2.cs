namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TypePartitaIva2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Utenti", "PartitaIva", c => c.String(maxLength: 11));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Utenti", "PartitaIva", c => c.String());
        }
    }
}
