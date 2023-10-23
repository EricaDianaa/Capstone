namespace Capstone.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ordini")]
    public partial class Ordini
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ordini()
        {
            ListaOrdini = new HashSet<ListaOrdini>();
        }

        [Key]
        public int IdOrdini { get; set; }

        [Column(TypeName = "date")]
        public DateTime Data { get; set; }

        public int IdUtente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ListaOrdini> ListaOrdini { get; set; }

        public virtual Utenti Utenti { get; set; }
    }
}
