namespace Capstone.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Eventi")]
    public partial class Eventi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Eventi()
        {
            ListaOrdini = new HashSet<ListaOrdini>();
            Recensioni = new HashSet<Recensioni>();
        }

        [Key]
        public int IdEvento { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Nome evento")]
        public string NomeEvento { get; set; }

        [Required]
        public string Descrizione { get; set; }

        [Column(TypeName = "money")]
        public decimal Prezzo { get; set; }

        [Required]
        [StringLength(100)]
        public string Luogo { get; set; }

        [Required]
        [StringLength(100)]
        public string Indirizzo { get; set; }


        [StringLength(50,MinimumLength =1,ErrorMessage ="Il campo � obbligatorio")]
        [Display(Name = "Foto copertina")]
        public string FotoCopertina { get; set; }


        [StringLength(50)]
        [Display(Name = "Immagine aggiuntiva 1")]
        public string Foto1 { get; set; }

        [StringLength(50)]
        [Display(Name = "Immagine aggiuntiva 2")]
        public string Foto2 { get; set; }

        [StringLength(50)]
        [Display(Name = "Immagine aggiuntiva 3")]
        public string Foto3 { get; set; }

        [StringLength(50)]
        [Display(Name = "Immagine aggiuntiva 4")]
        public string Foto4 { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Data evento")]
        public DateTime DataEvento { get; set; }

        public int IdCategoria { get; set; }

        public virtual Categorie Categorie { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ListaOrdini> ListaOrdini { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recensioni> Recensioni { get; set; }
    }
}
