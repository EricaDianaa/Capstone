namespace Capstone.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Recensioni")]
    public partial class Recensioni
    {
        [Key]
        public int IdRecensione { get; set; }

        public int IdUtente { get; set; }

        public int Voto { get; set; }

        [Required]
        public string Descrizione { get; set; }

        public int IdEvento { get; set; }

        public virtual Eventi Eventi { get; set; }

        public virtual Utenti Utenti { get; set; }
    }
}
