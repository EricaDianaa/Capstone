namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelazioneEventiUtenti : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Eventi", "IdUtente", c => c.Int());
            CreateIndex("dbo.Eventi", "IdUtente");
            AddForeignKey("dbo.Eventi", "IdUtente", "dbo.Utenti", "IdUtente");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Eventi", "IdUtente", "dbo.Utenti");
            DropIndex("dbo.Eventi", new[] { "IdUtente" });
            DropColumn("dbo.Eventi", "IdUtente");
        }
    }
}
