namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTablePreferiti : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Preferiti",
                c => new
                    {
                        IdPreferito = c.Int(nullable: false, identity: true),
                        IdUtente = c.Int(nullable: false),
                        IdEvento = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdPreferito);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Preferiti");
        }
    }
}
