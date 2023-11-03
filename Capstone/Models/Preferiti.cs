
namespace Capstone.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Preferiti")]
    public partial class Preferiti
    {
        [Key]
        public int IdPreferito { get; set; }
        public int IdUtente { get; set; }
        public int IdEvento { get; set; }
        public virtual Utenti Utenti { get; set; }
    }
}
