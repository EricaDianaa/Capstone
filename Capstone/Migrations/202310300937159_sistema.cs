namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sistema : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Eventi", "IdUtente", "dbo.Utenti");
            DropIndex("dbo.Eventi", new[] { "IdUtente" });
            RenameColumn(table: "dbo.Eventi", name: "IdUtente", newName: "IdUtente");
            AlterColumn("dbo.Eventi", "IdUtente", c => c.Int(nullable: false));
            CreateIndex("dbo.Eventi", "IdUtente");
            AddForeignKey("dbo.Eventi", "IdUtente", "dbo.Utenti", "IdUtente", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Eventi", "IdUtente", "dbo.Utenti");
            DropIndex("dbo.Eventi", new[] { "IdUtente" });
            AlterColumn("dbo.Eventi", "IdUtente", c => c.Int());
            RenameColumn(table: "dbo.Eventi", name: "IdUtente", newName: "IdUtente");
            CreateIndex("dbo.Eventi", "IdUtente");
            AddForeignKey("dbo.Eventi", "IdUtente", "dbo.Utenti", "IdUtente");
        }
    }
}
