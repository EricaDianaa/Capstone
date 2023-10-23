namespace Capstone.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ListaOrdini")]
    public partial class ListaOrdini
    {
        [Key]
        public int IdListaOrdine { get; set; }

        public int Quantità { get; set; }

        public int IdEvento { get; set; }

        public int IdOrdine { get; set; }

        public virtual Eventi Eventi { get; set; }

        public virtual Ordini Ordini { get; set; }
    }
}
