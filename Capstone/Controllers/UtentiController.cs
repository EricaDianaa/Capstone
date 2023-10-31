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

        [Authorize(Roles ="Admin")]
        public ActionResult Index()
        {
            return View(db.Utenti.ToList());
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
            if (User.IsInRole("Admin") || User.IsInRole("Azienda"))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Account","Home");
            }
        }
        [Authorize(Roles ="Admin")]
        
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
