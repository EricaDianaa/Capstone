using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Capstone.Models
{
    public partial class ModelBContent : DbContext
    {
        public ModelBContent()
            : base("name=ModelBContent")
        {
        }

        public virtual DbSet<Categorie> Categorie { get; set; }
        public virtual DbSet<Eventi> Eventi { get; set; }
        public virtual DbSet<ListaOrdini> ListaOrdini { get; set; }
        public virtual DbSet<Ordini> Ordini { get; set; }
        public virtual DbSet<Recensioni> Recensioni { get; set; }
        public virtual DbSet<Utenti> Utenti { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categorie>()
                .HasMany(e => e.Eventi)
                .WithRequired(e => e.Categorie)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Eventi>()
                .Property(e => e.Prezzo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Eventi>()
                .HasMany(e => e.ListaOrdini)
                .WithRequired(e => e.Eventi)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Eventi>()
                .HasMany(e => e.Recensioni)
                .WithRequired(e => e.Eventi)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ordini>()
                .HasMany(e => e.ListaOrdini)
                .WithRequired(e => e.Ordini)
                .HasForeignKey(e => e.IdOrdine)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Utenti>()
                .HasMany(e => e.Ordini)
                .WithRequired(e => e.Utenti)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Utenti>()
                .HasMany(e => e.Recensioni)
                .WithRequired(e => e.Utenti)
                .WillCascadeOnDelete(false);
        }
    }
}
