using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Capstone.Controllers
{[Authorize(Roles ="Admin, Azienda")]
    public class AdminController : Controller
    {
        ModelBContent db=new ModelBContent();
        public ActionResult Index()
        {
            ViewBag.IdCategoria = new SelectList(db.Categorie, "IdCategoria", "NomeCategoria");
            return View();
        }
        //Queri per Admin e Aziende
        public JsonResult TotaleGuadagni()
        {
            //Seleziono gli eventi
            List<Eventi> eventi = new List<Eventi>();
            //Se è un azienda
            if (User.IsInRole("Azienda"))
            {
                if (Session["Utente"] != null)
                {
                    //Seleziono gli eventi collegati all'azienda
                    int idUtente = (int)Session["Utente"];
                    eventi = db.Eventi.Where(m => m.IdUtente == idUtente).ToList();
                }
                else
                {
                     RedirectToAction("Login", "Home");
                }

            }
            else
            {
                //Seleziono tutti gli eventi
                eventi = db.Eventi.Select(m => m).ToList();
            }

            List<decimal> Totale = new List<decimal>();
            foreach (Eventi e in eventi)
            {   
                //Seleziono la lista collegata agli ordini
                List<ListaOrdini> lista = db.ListaOrdini.Where(m => m.IdEvento == e.IdEvento).ToList();
                //Per ogni lista ordine faccio la somma del costo totale
                foreach (ListaOrdini list in lista)
                {
                    decimal i = list.Quantità * e.Prezzo;
                    Totale.Add(i);
                }
            }
            decimal tot = Totale.Sum();
            return Json(tot,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TotaleGuadagniData(DateTime DataEvento)
        {
            //Seleziono gli eventi
            List<Eventi> eventi=new List<Eventi>();
            //Se è un azienda
            if (User.IsInRole("Azienda"))
            {
                if (Session["Utente"] != null)
                {
                    //Seleziono gli eventi collegati all'azienda
                    int idUtente = (int)Session["Utente"];
                    eventi = db.Eventi.Where(m => m.IdUtente == idUtente).ToList();
                }
                else
                {
                    RedirectToAction("Login", "Home");
                }
            }
            else
            {
              //Seleziono tutti gli eventi
               eventi = db.Eventi.Select(m => m).ToList();
            }

            List<decimal> Totale = new List<decimal>();
            foreach (Eventi e in eventi)
            {
                //Seleziono la lista collegata agli ordini
                List<ListaOrdini> lista = db.ListaOrdini.Where(m => m.IdEvento == e.IdEvento).ToList();
              
                //Seleziono gli ordini collegati a dataEvento
                foreach (ListaOrdini list in lista)
                {
                    List<Ordini> o=db.Ordini.Where(m=>m.IdOrdini==list.IdOrdine&&m.Data==DataEvento).ToList();
                   
                    foreach (Ordini or in o)
                    {
                        //Riseleziono la lista collegata agli ordini in quella data con l'IdOrdine
                        List<ListaOrdini> li = db.ListaOrdini.Where(m => m.IdEvento == e.IdEvento&&m.IdOrdine==or.IdOrdini).ToList();
                        //Per ogni lista ordine faccio la somma del costo totale
                        foreach (ListaOrdini l in li)
                        {
                           decimal i = l.Quantità * e.Prezzo;
                           Totale.Add(i);
                        }
                    }
                }
                
            }
            decimal tot = Totale.Sum();
            return Json(tot);
        }

        [HttpPost]
        public JsonResult TotaleGuadagniDataCategoria(DateTime DataEvento,int IdCategoria)
        {
            //Seleziono gli eventi
            List<Eventi> eventi = new List<Eventi>();
            //Se è un azienda
            if (User.IsInRole("Azienda"))
            {
                if (Session["Utente"] != null)
                {
                    //Seleziono gli eventi collegati all'azienda
                    int idUtente = (int)Session["Utente"];
                    eventi = db.Eventi.Where(m => m.IdUtente == idUtente && m.IdCategoria == IdCategoria).ToList();
                }
                else
                {
                    RedirectToAction("Login", "Home");
                }
            }
            else
            {
                 //Seleziono tutti gli eventi
                 eventi = db.Eventi.Where(m => m.IdCategoria==IdCategoria).ToList();
            }
           

            List<decimal> Totale = new List<decimal>();
            foreach (Eventi e in eventi)
            {
                //Seleziono la lista collegata agli ordini
                List<ListaOrdini> lista = db.ListaOrdini.Where(m => m.IdEvento == e.IdEvento).ToList();

                //Seleziono gli ordini collegati a dataEvento
                foreach (ListaOrdini list in lista)
                {
                    List<Ordini> o = db.Ordini.Where(m => m.IdOrdini == list.IdOrdine && m.Data == DataEvento).ToList();

                    foreach (Ordini or in o)
                    {
                        //Riseleziono la lista collegata agli ordini in quella data con l'IdOrdine
                        List<ListaOrdini> li = db.ListaOrdini.Where(m => m.IdEvento == e.IdEvento && m.IdOrdine == or.IdOrdini).ToList();
                        //Per ogni lista ordine faccio la somma del costo totale
                        foreach (ListaOrdini l in li)
                        {
                            decimal i = l.Quantità * e.Prezzo;
                            Totale.Add(i);
                        }
                    }
                }

            }
            decimal tot = Totale.Sum();
            return Json(tot);
        }
        public JsonResult PostPopolari()
        {
            //selezione tutti gli eventi
            List<Eventi> eventi=new List<Eventi>();
            //Per admin
            if (User.IsInRole("Admin"))
            {
                 eventi = db.Eventi.ToList();
            }
            //per azienda
            if (User.IsInRole("Azienda"))
            {
                if (Session["Utente"] != null)
                {
                    //Seleziono gli eventi collegati all'azienda
                    int idUtente = (int)Session["Utente"];
                    eventi = db.Eventi.Where(m => m.IdUtente == idUtente).ToList();
                }
                else
                {
                    RedirectToAction("Login", "Home");
                }

            }


            List<ListaOrdini> Lista = new List<ListaOrdini>();
            List<PostPopolari> PostPopolari = new List<PostPopolari>();
            List<PostPopolari> list = new List<PostPopolari>();
            
            foreach (Eventi e in eventi)
            {
                //Seleziono la lista degli articoli degli ordini
                var order = db.ListaOrdini.Where(m => m.IdEvento == e.IdEvento).ToList();
                Lista.AddRange(order);
            }
            foreach (ListaOrdini li in Lista)
            {
                //Per ogni articolo collegato a un ordine lo assegno in una lista(PostPopolari)
                var order = db.Ordini.Where(m => m.IdOrdini == li.IdOrdine).Count();
                var idevento = li.IdEvento;
                //Completo tutte le proprietà
                PostPopolari p = new PostPopolari();
                p.IdEvento = idevento;
                p.Ordini = order;
                p.Prezzo = li.Eventi.Prezzo;
                p.Quantità = li.Quantità;
                p.NomeEvento = li.Eventi.NomeEvento;
                //calcolo il prezzo
                var Quantitàperordini = Convert.ToDouble(p.Quantità) * Convert.ToDouble(p.Prezzo);
                p.Totale = Quantitàperordini * p.Ordini;
                if (PostPopolari.Count == 0)
                {  
                    PostPopolari.Add(p);
                    //Sommo gli ordini con stesso IdEvento e li riassegno in un altra lista
                    var sum = PostPopolari.Where(m => m.IdEvento == idevento).Sum(m => m.Ordini);

                    if (sum != 0)
                    {
                        
                        PostPopolari p1 = new PostPopolari();
                        p1.IdEvento = idevento;
                        p1.Ordini = sum;
                        p1.Prezzo = li.Eventi.Prezzo;
                        var sumquantità =PostPopolari.Where(m => m.IdEvento == idevento).Sum(m => m.Quantità);
                        p1.Quantità = sumquantità;
                        var Quantitàperordin= Convert.ToDouble(p1.Quantità) * Convert.ToDouble(p1.Prezzo);
                        p1.Totale = Quantitàperordin * p1.Ordini;
                        p1.NomeEvento = li.Eventi.NomeEvento;
                        list.Add(p1);
                    } 
                }

                foreach (var item in PostPopolari)
                {
                    if (p != item)
                    {
                        var c = list.Where(m => m.IdEvento == item.IdEvento).ToList();
                        //Popolo la lista con gli ordini per ogni evento

                        PostPopolari.Add(p);
                        //Somma delle quantità con lo stesso idEvento
                        var sum = PostPopolari.Where(m => m.IdEvento == idevento).Sum(m => m.Ordini);

                        if (sum != 0)
                        {
                            PostPopolari p1 = new PostPopolari();
                            p1.IdEvento = idevento;
                            p1.Ordini = sum;
                            p1.Prezzo = li.Eventi.Prezzo;
                            var sumquantità = PostPopolari.Where(m => m.IdEvento == idevento).Sum(m => m.Quantità);
                            p1.Quantità = sumquantità;
                            double QuantitàOrdine= Convert.ToDouble(p1.Quantità) * Convert.ToDouble(p1.Prezzo);
                            p1.Totale = QuantitàOrdine;
                            p1.NomeEvento = li.Eventi.NomeEvento;
                            list.Add(p1);

                            if (sum > 1)
                            {
                                //Rimuovo gli elementi doppi
                                PostPopolari removeitem = list.FirstOrDefault(m => m.IdEvento == p1.IdEvento && m.Ordini >= 1);
                                list.Remove(removeitem);

                            }
                            break;
                        }
                    }
                }
            }
            return Json(list, JsonRequestBehavior.AllowGet);

        }
    }

}