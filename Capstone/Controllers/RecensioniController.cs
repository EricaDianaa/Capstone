using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Capstone.Models;

namespace Capstone.Controllers
{
    public class RecensioniController : Controller
    {
        private ModelBContent db = new ModelBContent();
        public ActionResult Index()
        {
            var recensioni = db.Recensioni.Include(r => r.Eventi).Include(r => r.Utenti);
            return View(recensioni.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recensioni recensioni = db.Recensioni.Find(id);
            if (recensioni == null)
            {
                return HttpNotFound();
            }
            return View(recensioni);
        }
        public ActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdRecensione,IdUtente,Voto,Descrizione,IdEvento")] Recensioni recensioni, int rating)
        {
            if (ModelState.IsValid)
            {
                recensioni.IdEvento =Convert.ToInt32(Request.QueryString["id"]);
                recensioni.IdUtente = (int)Session["Utente"];
                recensioni.Voto = rating;
                db.Recensioni.Add(recensioni);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

          
            return View(recensioni);
        }

        public ActionResult CreateRecensioni()
        {

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRecensioni([Bind(Include = "IdRecensione,IdUtente,Voto,Descrizione,IdEvento")] Recensioni recensioni)
        {
            if (ModelState.IsValid)
            {
                recensioni.IdEvento = Convert.ToInt32(Request.QueryString["id"]);
                recensioni.IdUtente = (int)Session["Utente"];
                db.Recensioni.Add(recensioni);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View(recensioni);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recensioni recensioni = db.Recensioni.Find(id);
            if (recensioni == null)
            {
                return HttpNotFound();
            }
           
            return View(recensioni);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdRecensione,IdUtente,Voto,Descrizione,IdEvento")] Recensioni recensioni, int rating)
        {
            if (ModelState.IsValid)
            {
                if (recensioni.IdUtente == (int)Session["Utente"])
                {
                    recensioni.Voto = rating;
                    db.Entry(recensioni).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
           
            return View(recensioni);
        }

        public ActionResult Delete(int? id)
        {
             Recensioni recensioni = db.Recensioni.Find(id);
            db.Recensioni.Remove(recensioni);
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
    }
}
