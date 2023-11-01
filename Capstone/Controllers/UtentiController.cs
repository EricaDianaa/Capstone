using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Capstone.Models;
using DocumentFormat.OpenXml.EMMA;
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
            //var base64EncodedBytesPassword = System.Convert.FromBase64String(utenti.Password);
            //string password = System.Text.Encoding.UTF8.GetString(base64EncodedBytesPassword);
            //utenti.Password = password;
            //try
            //{
            //    using (var context = new ModelBContent())
            //    {
            //        var getUser = (from s in context.Utenti where s.Username == utenti.Username || s.Email == utenti.Username select s).FirstOrDefault();
            //        if (getUser != null)
            //        {
            //            var PasswordDecode = Hash.base64Decode(utenti.Password);

            //            ViewBag.ErrorMessage = "Username o password non validi";
            //            return View();
            //        }
            //        ViewBag.ErrorMessage = "Username o password non validi";
            //        return View();
            //    }
            //}
            //catch (Exception e)
            //{
            //    ViewBag.ErrorMessage = " Error!!! contact cms@info.in";
            //    return View();
            //}
            return View(utenti);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdUtente,Username,Ruolo,Password,Indirizzo,Email,Telefono,IsAzienda,CodiceFiscale,PartitaIva")] Utenti utenti)
        {

            Utenti u = db.Utenti.Where(m => m.IdUtente == utenti.IdUtente).FirstOrDefault();
            utenti.Username = u.Username;

            if (!User.IsInRole("Admin"))
            {
                utenti.Ruolo = u.Ruolo;
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
                return RedirectToAction("Account", "Home");
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
