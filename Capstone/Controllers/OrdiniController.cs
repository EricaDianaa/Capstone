using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Capstone.Models;

namespace Capstone.Controllers
{
    [Authorize(Roles ="User")]
    public class OrdiniController : Controller
    {
        private ModelBContent db = new ModelBContent();

        public List<EventiOrdini> ListOrdini = new List<EventiOrdini>();
        [Authorize(Roles ="Admin, Azienda")]
        public ActionResult Index()
        {
            var ordini = db.Ordini.Include(o => o.Utenti);
            return View(ordini.ToList());
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordini ordini = db.Ordini.Find(id);
            if (ordini == null)
            {
                return HttpNotFound();
            }
            return View(ordini);
        }

        //Crea Ordine
        public ActionResult Create()
        {
            ViewBag.IdUtente = new SelectList(db.Utenti, "IdUtente", "Username");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdOrdini,Data,IdUtente")] Ordini ordini)
        {
            //Se l'utente non ha eseguito il login non può effettuare un ordine quindi lo rendirizzo in pagina Login
            if (Session["Utente"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
            if (ModelState.IsValid)
                {   //Prendo i prodotti della sessione carrello
                    Ordini ordine = new Ordini();
                    List<EventiOrdini> prodotto = new List<EventiOrdini>();
                    prodotto = (List<EventiOrdini>)Session["Carello"];
                    Session["Cart1"] = Session["Carello"];
                   //Assegno i valori all'ordine
                    ordini.Data = DateTime.Now;
                    ordini.IdUtente=(int)Session["Utente"];
                    //Per ogni elemento del carrello aggiungo nel database ListaOrdini legata alla tabella Ordini
                    foreach (EventiOrdini p in prodotto)
                    {
                        ordini.ListaOrdini.Add(new ListaOrdini
                        {
                            Quantità = p.Quantità,
                            IdEvento = p.IdEvento,
                        });
                    };
                    Session["IdOrdine"] = ordini.IdOrdini;
                    //Creo l'ordine e salvo nel db
                    ordine = db.Ordini.Add(ordini);
                    db.Ordini.Add(ordini);
                    db.SaveChanges();
                    //Svuoto il carrello alla fine dell'ordine
                    Session["Carello"]=null;
                return RedirectToAction("Index","Home");
            }
            }
    

            ViewBag.IdUtente = new SelectList(db.Utenti, "IdUtente", "Username", ordini.IdUtente);
            return View(ordini);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordini ordini = db.Ordini.Find(id);
            if (ordini == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdUtente = new SelectList(db.Utenti, "IdUtente", "Username", ordini.IdUtente);
            return View(ordini);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdOrdini,Data,IdUtente")] Ordini ordini)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordini).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdUtente = new SelectList(db.Utenti, "IdUtente", "Username", ordini.IdUtente);
            return View(ordini);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordini ordini = db.Ordini.Find(id);
            if (ordini == null)
            {
                return HttpNotFound();
            }
            return View(ordini);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ordini ordini = db.Ordini.Find(id);
            db.Ordini.Remove(ordini);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //Aggiungi al carrello
        public ActionResult AggiungiOrdine(int quantità, int IdEvento)
        {

            Eventi e = db.Eventi.FirstOrDefault(m => m.IdEvento == IdEvento);
            //Selezione del prodotto a un nuovo modello contenente Evento e Ordini
            EventiOrdini prod = new EventiOrdini();
            prod.IdEvento = IdEvento;
            prod.Quantità = quantità;
            prod.NomeEvento = e.NomeEvento;
            prod.FotoCopertina = e.FotoCopertina;
            prod.Foto1 = e.Foto1;
            prod.Foto2 = e.Foto2;
            prod.Foto3 = e.Foto3; 
            prod.Foto4= e.Foto4;
            prod.Prezzo = e.Prezzo;
            prod.DataEvento = e.DataEvento;
            prod.Luogo = e.Luogo;
            prod.Indirizzo = e.Indirizzo;
            ListOrdini.Add(prod);
          
           Eventi evento = db.Eventi.FirstOrDefault(m => m.IdEvento == IdEvento);
             //se la session è vuota(non esiste)creo la session e aggiungo il prodotto 
            if (Session["Carello"] == null)
            {
                Session["Carello"] = ListOrdini;
            }//altrimenti la session esiste già e aggiungo il nuovo evento
            else
            {
               
                List<EventiOrdini> prodott = new List<EventiOrdini>();
                prodott = (List<EventiOrdini>)Session["Carello"];
               
               EventiOrdini prodotto = prodott.Where(m => m.IdEvento == prod.IdEvento).FirstOrDefault();
                //se l'evento è gia presente nel carello somma solo la quantità
                if (prodotto != null && prod.IdEvento == prodotto.IdEvento)
                {
                    prod.Quantità = prodotto.Quantità + quantità;
                    prodott.Add(prod);
                    prodott.Remove(prodott.FirstOrDefault(m => m.IdEvento == IdEvento));

                }//Altrimenti aggiungi l'evento
                else
                {
                    prodott.Add(prod);

                }
                Session["Carello"] = prodott;
            };

            return View();
        }

        //Carrello
        public ActionResult Cart()
        {
            List<EventiOrdini> prodotto = new List<EventiOrdini>();
            prodotto = (List<EventiOrdini>)Session["Carello"];
            List<decimal> TotaleOrdine = new List<decimal>();
            if (Session["Carello"] != null)
            {
                //Somma totale carello
                foreach (EventiOrdini p in prodotto)
                    {
                        decimal q = Convert.ToDecimal(p.Quantità);
                        decimal prezzo = Convert.ToDecimal(p.Prezzo);
                        decimal tot = q *= prezzo;
                        TotaleOrdine.Add(tot);
                    }
                ViewBag.totale = TotaleOrdine.Sum();
                Session["TotOrdine"] = TotaleOrdine.Sum();
                return View(prodotto);
            }
            else
            {
                return View();
            }
        }
        //Elimina dal carrello
        public ActionResult CartRemove()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CartRemove(EventiOrdini p)
        {

            List<EventiOrdini> prodotto = new List<EventiOrdini>();
            prodotto = (List<EventiOrdini>)Session["Carello"];
            string id = Request.QueryString["Id"];
       
            p.IdEvento = Convert.ToInt16(id);
           var prod= prodotto.Where(m => m.IdEvento==p.IdEvento).FirstOrDefault();
            //Se il prodotto ha più di una quantità elimino solo dalla quantità
            if (prod.Quantità>1)
            {
              
                List<EventiOrdini> list = new List<EventiOrdini>();
                prod.Quantità = prod.Quantità - 1;
                list.Add(prod);
                Session["Carello"] = prod;
            }
            //altrimenti elimino il prodotto
            else
            {
                prodotto.Remove(prodotto.FirstOrDefault(m => m.IdEvento == p.IdEvento));
            }
            Session["Carello"] = prodotto;

            return RedirectToAction("Cart", "Ordini");
        }
        //Svuota carrello
        public ActionResult CartRemoveAll(EventiOrdini p)
        {
            //Azione per svuotare l'intero carello
            Session["Carello"] = null;
            return RedirectToAction("Cart", "Ordini");
        }

        public ActionResult RiepilogoOrdine()
        {
            int idUtente = (int)Session["Utente"];
            Utenti u = db.Utenti.Where(m => m.IdUtente==idUtente).FirstOrDefault();
            List<Ordini> e = db.Ordini.Where(m => m.IdUtente == idUtente).ToList();
            List<EventiOrdini> prodotto = new List<EventiOrdini>();
            prodotto = (List<EventiOrdini>)Session["Carello"];
            ViewBag.totale = Session["TotOrdine"];
            ViewBag.IdUtente = idUtente;
            ViewBag.Nome = u.Username;
            ViewBag.Indirizzo = u.Indirizzo;
            ViewBag.Telefono = u.Telefono;
            return PartialView(prodotto);
            
        }

        public ActionResult OrdiniEffettuati()
        {
            int idUtente =(int) Session["Utente"];
            Utenti u = db.Utenti.FirstOrDefault(m => m.IdUtente == idUtente);
            ViewBag.NomeUtente=u.Username;
            List< Ordini > e = db.Ordini.Where(m => m.IdUtente == idUtente).ToList();
            return View(e);
        }
    }
}