namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categorie",
                c => new
                    {
                        IdCategoria = c.Int(nullable: false, identity: true),
                        NomeCategoria = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.IdCategoria);
            
            CreateTable(
                "dbo.Eventi",
                c => new
                    {
                        IdEvento = c.Int(nullable: false, identity: true),
                        NomeEvento = c.String(nullable: false, maxLength: 50),
                        Descrizione = c.String(nullable: false),
                        Prezzo = c.Decimal(nullable: false, storeType: "money"),
                        Luogo = c.String(nullable: false, maxLength: 100),
                        Indirizzo = c.String(nullable: false, maxLength: 100),
                        FotoCopertina = c.String(maxLength: 50),
                        Foto1 = c.String(maxLength: 50),
                        Foto2 = c.String(maxLength: 50),
                        Foto3 = c.String(maxLength: 50),
                        Foto4 = c.String(maxLength: 50),
                        DataEvento = c.DateTime(nullable: false, storeType: "date"),
                        IdCategoria = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdEvento)
                .ForeignKey("dbo.Categorie", t => t.IdCategoria)
                .Index(t => t.IdCategoria);
            
            CreateTable(
                "dbo.ListaOrdini",
                c => new
                    {
                        IdListaOrdine = c.Int(nullable: false, identity: true),
                        Quantità = c.Int(nullable: false),
                        IdEvento = c.Int(nullable: false),
                        IdOrdine = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdListaOrdine)
                .ForeignKey("dbo.Ordini", t => t.IdOrdine)
                .ForeignKey("dbo.Eventi", t => t.IdEvento)
                .Index(t => t.IdEvento)
                .Index(t => t.IdOrdine);
            
            CreateTable(
                "dbo.Ordini",
                c => new
                    {
                        IdOrdini = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false, storeType: "date"),
                        IdUtente = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdOrdini)
                .ForeignKey("dbo.Utenti", t => t.IdUtente)
                .Index(t => t.IdUtente);
            
            CreateTable(
                "dbo.Utenti",
                c => new
                    {
                        IdUtente = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 50),
                        Ruolo = c.String(maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 50),
                        Indirizzo = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 50),
                        Telefono = c.String(nullable: false, maxLength: 10),
                        IsAzienda = c.Boolean(nullable: false),
                        CodiceFiscale = c.String(maxLength: 16),
                        PartitaIva = c.String(maxLength: 11),
                    })
                .PrimaryKey(t => t.IdUtente);
            
            CreateTable(
                "dbo.Recensioni",
                c => new
                    {
                        IdRecensione = c.Int(nullable: false, identity: true),
                        IdUtente = c.Int(nullable: false),
                        Voto = c.Int(nullable: false),
                        Descrizione = c.String(nullable: false),
                        IdEvento = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdRecensione)
                .ForeignKey("dbo.Utenti", t => t.IdUtente)
                .ForeignKey("dbo.Eventi", t => t.IdEvento)
                .Index(t => t.IdUtente)
                .Index(t => t.IdEvento);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Eventi", "IdCategoria", "dbo.Categorie");
            DropForeignKey("dbo.Recensioni", "IdEvento", "dbo.Eventi");
            DropForeignKey("dbo.ListaOrdini", "IdEvento", "dbo.Eventi");
            DropForeignKey("dbo.Recensioni", "IdUtente", "dbo.Utenti");
            DropForeignKey("dbo.Ordini", "IdUtente", "dbo.Utenti");
            DropForeignKey("dbo.ListaOrdini", "IdOrdine", "dbo.Ordini");
            DropIndex("dbo.Recensioni", new[] { "IdEvento" });
            DropIndex("dbo.Recensioni", new[] { "IdUtente" });
            DropIndex("dbo.Ordini", new[] { "IdUtente" });
            DropIndex("dbo.ListaOrdini", new[] { "IdOrdine" });
            DropIndex("dbo.ListaOrdini", new[] { "IdEvento" });
            DropIndex("dbo.Eventi", new[] { "IdCategoria" });
            DropTable("dbo.Recensioni");
            DropTable("dbo.Utenti");
            DropTable("dbo.Ordini");
            DropTable("dbo.ListaOrdini");
            DropTable("dbo.Eventi");
            DropTable("dbo.Categorie");
        }
    }
}
