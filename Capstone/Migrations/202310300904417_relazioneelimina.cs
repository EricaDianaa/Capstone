namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class relazioneelimina : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Eventi", "IdUtente", "dbo.Utenti");
            DropIndex("dbo.Eventi", new[] { "IdUtente" });
          
        }
        
        public override void Down()
        {
            AddColumn("dbo.Eventi", "IdUtente", c => c.Int(nullable: false));
            CreateIndex("dbo.Eventi", "IdUtente");
            AddForeignKey("dbo.Eventi", "IdUtente", "dbo.Utenti", "IdUtente", cascadeDelete: true);
        }
    }
}
