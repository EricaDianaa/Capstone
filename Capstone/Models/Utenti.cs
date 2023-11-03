namespace Capstone.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;
    using System.Web.UI.WebControls;

    [Table("Utenti")]
    public partial class Utenti
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Utenti()
        {
            Ordini = new HashSet<Ordini>();
            Recensioni = new HashSet<Recensioni>();
            Eventi=new HashSet<Eventi>();
            Preferiti = new HashSet<Preferiti>();
        }

        [Key]
        public int IdUtente { get; set; }

        [Required]
        [StringLength(50)]
        //[Remote("IsNameValid", "Validate", ErrorMessage ="Username non disponibile")]
        public string Username { get; set; }

        [NotMapped]

        public string UsernameLogin { get; set; }

        [StringLength(50)]
        public string Ruolo { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4,  ErrorMessage = "La password deve contenere minimo 4 caratteri")]
        
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string Indirizzo { get; set; }

        [Required]
        [StringLength(50)]
        //[Remote("IsEmailValid","Validate", ErrorMessage = "Email non disponibile")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email non valida")]
        public string Email { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Il campo deve contenere 10 caratteri")]
        public string Telefono { get; set; }
        [Display(Name ="Sei un Azienda?")]
        public bool IsAzienda { get; set; }

        [StringLength(16,MinimumLength =16 ,ErrorMessage ="Il codice fiscale deve contenere 16 caratteri")]

        [Display(Name = "Codice fiscale")]

        public string CodiceFiscale { get; set; }
        [StringLength(11, MinimumLength = 11, ErrorMessage = "La partita iva deve contenere 11 caratteri")]
        [Display(Name = "Partita iva")]
        public string PartitaIva { get; set; }

        public string VCode {get; set;}
      

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ordini> Ordini { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recensioni> Recensioni { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Eventi> Eventi { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Preferiti> Preferiti { get; set; }

    }
}
