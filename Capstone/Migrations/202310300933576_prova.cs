namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class prova : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Eventi", "IdUtente", "dbo.Utenti");
            DropIndex("dbo.Eventi", new[] { "IdUtente" });
            RenameColumn(table: "dbo.Eventi", name: "IdUtente", newName: "IdUtente");
            AlterColumn("dbo.Eventi", "IdUtente", c => c.Int());
            CreateIndex("dbo.Eventi", "IdUtente");
            AddForeignKey("dbo.Eventi", "IdUtente", "dbo.Utenti", "IdUtente");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Eventi", "IdUtente", "dbo.Utenti");
            DropIndex("dbo.Eventi", new[] { "IdUtente" });
            AlterColumn("dbo.Eventi", "IdUtente", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Eventi", name: "IdUtente", newName: "IdUtente");
            CreateIndex("dbo.Eventi", "IdUtente");
            AddForeignKey("dbo.Eventi", "IdUtente", "dbo.Utenti", "IdUtente", cascadeDelete: true);
        }
    }
}
