using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public class EventiOrdini
    {
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


        [StringLength(50, MinimumLength = 1, ErrorMessage = "Il campo è obbligatorio")]
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
        public DateTime DataDa { get; set; }
        public int IdCategoria { get; set; }
        public int NomeCategoria { get; set; }
        public int NomeUtente { get; set; }
        public int IdListaOrdine { get; set; }

        public int Quantità { get; set; }

        public int IdOrdine { get; set; }
    }
}