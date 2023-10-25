namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteRequiredFotoCopertina : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Eventi", "FotoCopertina", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Eventi", "FotoCopertina", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
