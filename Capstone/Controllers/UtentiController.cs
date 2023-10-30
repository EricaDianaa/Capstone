using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Capstone.Models;
using Microsoft.Ajax.Utilities;

namespace Capstone.Controllers
{
    public class UtentiController : Controller
    {
        private ModelBContent db = new ModelBContent();

        [Authorize(Roles ="Admin,Azienda")]
        public ActionResult Index()
        {
            if (User.IsInRole("Azienda"))
            {
                if (Session["Utente"] != null)
                {
                    //se l'utente è un azienda deve vedere solo gli utenti correlati agli eventi da lui creati
                    int id = (int)Session["Utente"];
                    List<Utenti> UtentiList = new List<Utenti>();
                    //selezione gli eventi
                    List<Eventi> eventi = db.Eventi.Where(m => m.IdUtente == id).ToList();
                    
                    //seleziono la lista collegata agli ordini
                    List<ListaOrdini> li = new List<ListaOrdini>();
                    foreach (Eventi e in eventi)
                    {
                        List<ListaOrdini> lista = db.ListaOrdini.Where(m => m.IdEvento == e.IdEvento).ToList();
                        li.AddRange(lista);
                        
                    }
                    //Seleziono gli ordini
                    List<Ordini> ordini = new List<Ordini>();
                    foreach (ListaOrdini list in li)
                    {
                        List<Ordini> lista = db.Ordini.Where(m => m.IdOrdini == list.IdOrdine).ToList();
                        ordini.AddRange(lista);
                       
                    }
                    List<Utenti> utenti = new List<Utenti>();
                    foreach (Ordini or in ordini)
                    {
                      List< Utenti> u = db.Utenti.Where(m => m.IdUtente == or.IdUtente).ToList();
                      utenti.AddRange(u);
                    }
                    List<Recensioni> recensioni = new List<Recensioni>();
                    //Seleziono le recensioni
                    foreach (Eventi item in eventi)
                    {
                        List<Recensioni> rece = db.Recensioni.Where(m => m.IdEvento == item.IdEvento).ToList();
                        recensioni.AddRange(rece);

                    }
                    foreach (Recensioni item in recensioni)
                    {
                        List<Utenti> u = db.Utenti.Where(m => m.IdUtente == item.IdUtente).ToList();
                        utenti.AddRange(u);
                    }

                    return View(utenti.ToList());
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }

            }
            else
            {
                return View(db.Utenti.ToList());
            }

        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utenti utenti = db.Utenti.Find(id);
            if (utenti == null)
            {
                return HttpNotFound();
            }
            return View(utenti);
        }

        [Authorize(Roles ="User,Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utenti utenti = db.Utenti.Find(id);
            if (utenti == null)
            {
                return HttpNotFound();
            }
            return View(utenti);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdUtente,Username,Ruolo,Password,Indirizzo,Email,Telefono,IsAzienda,CodiceFiscale,PartitaIva")] Utenti utenti)
        {
            Utenti u =db.Utenti.Where(m=>m.IdUtente==utenti.IdUtente).FirstOrDefault();
              utenti.Username= u.Username;
           
            if (!User.IsInRole("Admin"))
            {
                 utenti.Ruolo=u.Ruolo;
            }
            var local = db.Set<Utenti>().Local.FirstOrDefault(f => f.IdUtente == u.IdUtente);
            if (local != null)
            {
                db.Entry(local).State = EntityState.Detached;
            }
            db.Entry(utenti).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

        }
        [Authorize(Roles ="Admin")]
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Utenti utenti = db.Utenti.Find(id);
        //    if (utenti == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(utenti);
        //}

       
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Utenti utenti = db.Utenti.Find(id);
        //    db.Utenti.Remove(utenti);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
