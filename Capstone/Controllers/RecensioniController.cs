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
        [Authorize(Roles ="Admin, Azienda")]
        public ActionResult Index()
        {

           if (User.IsInRole("Admin"))
            {
                //Se l'utente è un admin potra vedere tutti gli eventi
                var recensioni = db.Recensioni.Include(r => r.Eventi).Include(r => r.Utenti);
                return View(recensioni.ToList());
            }
            else if (User.IsInRole("Azienda"))
            {
                if (Session["Utente"] != null)
                {
                    //se l'utente è un azienda deve vedere solo gli ordini correlati agli eventi da lui creati
                    int id = (int)Session["Utente"];
                    //selezione gli eventi
                    List<Eventi> eventi = db.Eventi.Where(m => m.IdUtente == id).ToList();
                    List<Recensioni> li = new List<Recensioni>();
                    //seleziono la lista collegata agli ordini
                    foreach (Eventi e in eventi)
                    {
                        List<Recensioni> lista = db.Recensioni.Where(m => m.IdEvento == e.IdEvento).ToList();
                        li.AddRange(lista);
                    }

                    return View(li.ToList());
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
           
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
        [Authorize(Roles = "User")]
        public ActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdRecensione,IdUtente,Voto,Descrizione,IdEvento")] Recensioni recensioni, int? rating)
        {
            if (ModelState.IsValid)
            {
                if (Session["Utente"] != null)
                {
                    recensioni.IdEvento = Convert.ToInt32(Request.QueryString["id"]);
                    recensioni.IdUtente = (int)Session["Utente"];
                   
                    if (rating != null)
                    {
                        recensioni.Voto = (int)rating;
                        db.Recensioni.Add(recensioni);
                        db.SaveChanges();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.ErroreStelle = "Il campo è obbligatorio";
                        return View();
                    }

                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }


            }


            return View(recensioni);
        }

        [Authorize(Roles = "User")]
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
        public ActionResult Edit([Bind(Include = "IdRecensione,IdUtente,Voto,Descrizione,IdEvento")] Recensioni recensioni, int? rating)
        {  
            if (ModelState.IsValid)
            {
                if (Session["Utente"]!=null&&recensioni.IdUtente == (int)Session["Utente"])
                {
                    if (rating != null)
                    {
                        recensioni.Voto = (int)rating;
                    }
                    else
                    {
                        Recensioni r = db.Recensioni.FirstOrDefault(m => m.IdRecensione == recensioni.IdRecensione);
                        recensioni.Voto = r.Voto;
                    }
                    var local = db.Set<Recensioni>().Local.FirstOrDefault(f => f.IdRecensione == recensioni.IdRecensione);
                    if (local != null)
                    {
                        db.Entry(local).State = EntityState.Detached;
                    }
                    db.Entry(recensioni).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }

            }
           
            return View(recensioni);
        }
        [Authorize(Roles = "Azienda,Admin")]
        public ActionResult Delete(int? id)
        {
            Recensioni recensioni = db.Recensioni.Find(id);
            db.Recensioni.Remove(recensioni);
            db.SaveChanges();
            return RedirectToAction("Index", "Recensioni");
 
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
