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
    public class ListaOrdiniController : Controller
    {
        private ModelBContent db = new ModelBContent();


        public ActionResult Index()
        {
            var listaOrdini = db.ListaOrdini.Include(l => l.Eventi).Include(l => l.Ordini);
            return View(listaOrdini.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListaOrdini listaOrdini = db.ListaOrdini.Find(id);
            if (listaOrdini == null)
            {
                return HttpNotFound();
            }
            return View(listaOrdini);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListaOrdini listaOrdini = db.ListaOrdini.Find(id);
            if (listaOrdini == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdEvento = new SelectList(db.Eventi, "IdEvento", "NomeEvento", listaOrdini.IdEvento);
            ViewBag.IdOrdine = new SelectList(db.Ordini, "IdOrdini", "IdOrdini", listaOrdini.IdOrdine);
            return View(listaOrdini);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdListaOrdine,Quantità,IdEvento,IdOrdine")] ListaOrdini listaOrdini)
        {
            if (ModelState.IsValid)
            {
                ListaOrdini li = db.ListaOrdini.FirstOrDefault(m => m.IdListaOrdine == listaOrdini.IdListaOrdine);
                listaOrdini.IdOrdine = li.IdOrdine;
                listaOrdini.IdEvento = li.IdEvento;

                var local = db.Set<ListaOrdini>().Local.FirstOrDefault(f => f.IdListaOrdine == listaOrdini.IdListaOrdine);
                if (local != null)
                {
                    db.Entry(local).State = EntityState.Detached;
                }
                db.Entry(listaOrdini).State = EntityState.Modified;
                     db.SaveChanges();
            }
            ViewBag.IdEvento = new SelectList(db.Eventi, "IdEvento", "NomeEvento", listaOrdini.IdEvento);
            ViewBag.IdOrdine = new SelectList(db.Ordini, "IdOrdini", "IdOrdini", listaOrdini.IdOrdine);
            return View(listaOrdini);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListaOrdini listaOrdini = db.ListaOrdini.Find(id);
            if (listaOrdini == null)
            {
                return HttpNotFound();
            }
            return View(listaOrdini);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ListaOrdini listaOrdini = db.ListaOrdini.Find(id);
            db.ListaOrdini.Remove(listaOrdini);
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
