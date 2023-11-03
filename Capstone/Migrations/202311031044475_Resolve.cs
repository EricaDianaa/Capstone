namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Resolve : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Preferiti", "IdEvento", "dbo.Eventi");
            DropIndex("dbo.Preferiti", new[] { "IdEvento" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Preferiti", "IdEvento");
            AddForeignKey("dbo.Preferiti", "IdEvento", "dbo.Eventi", "IdEvento", cascadeDelete: true);
        }
    }
}
