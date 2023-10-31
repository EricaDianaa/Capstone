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

        public JsonResult TotaleGuadagni()
        {
            List<Eventi> eventi = new List<Eventi>();
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

            List<Eventi> eventi=new List<Eventi>();
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
            List<Eventi> eventi = new List<Eventi>();
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
    }
}