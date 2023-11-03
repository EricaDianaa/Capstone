namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelazioniPreferiti : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Preferiti", "IdUtente");
            AddForeignKey("dbo.Preferiti", "IdUtente", "dbo.Utenti", "IdUtente", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Preferiti", "IdUtente", "dbo.Utenti");
            DropIndex("dbo.Preferiti", new[] { "IdUtente" });
        }
    }
}
