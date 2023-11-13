using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public class PostPopolari
    {
        public int IdEvento { get; set; }
        public int Ordini { get; set; }
        public decimal Prezzo { get; set; }
        public int Quantità  { get; set; }
        public double Totale { get; set; }
    }
}