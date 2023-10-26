using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Capstone.Models;

namespace Capstone.Controllers
{[Authorize(Roles ="Admin , Azienda")]
    public class EventiController : Controller
    {
        private ModelBContent db = new ModelBContent();
        
       

        // GET: Eventi
        public ActionResult Index()
        {
            var eventi = db.Eventi.Include(e => e.Categorie).Include(e=>e.Recensioni);
            return View(eventi.ToList());
        }
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eventi eventi = db.Eventi.Find(id);
            if (eventi == null)
            {
                return HttpNotFound();
            }
            return View(eventi);
        }
        public ActionResult Create()
        {
            ViewBag.IdCategoria = new SelectList(db.Categorie, "IdCategoria", "NomeCategoria");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdEvento,NomeEvento,Descrizione,Prezzo,Luogo,Indirizzo,FotoCopertina,Foto1,Foto2,Foto3,Foto4,DataEvento,IdCategoria")] Eventi eventi, HttpPostedFileBase FotoCopertina, HttpPostedFileBase Foto1, HttpPostedFileBase Foto2, HttpPostedFileBase Foto3, HttpPostedFileBase Foto4)
        {
            if (ModelState.IsValid)
            {
                if (FotoCopertina != null)
                {

                
                //Salvataggio foto
                if (FotoCopertina != null && FotoCopertina.ContentLength > 0)
                {
                    string FotoCopertinaFile = FotoCopertina.FileName;
                    string FotoCopertinaPath = Path.Combine(Server.MapPath("~/Content/Img"), FotoCopertinaFile);
                    FotoCopertina.SaveAs(FotoCopertinaPath);
                    eventi.FotoCopertina = FotoCopertinaFile;
                }

                if (Foto1 != null && Foto1.ContentLength > 0)
                {
                    string Foto1File = Foto1.FileName;
                    string Foto1Path = Path.Combine(Server.MapPath("~/Content/Img"), Foto1File);
                    Foto1.SaveAs(Foto1Path);
                    eventi.Foto1 = Foto1File;
                }
              
                if (Foto2 != null&&Foto2.ContentLength > 0)
                {
                    string Foto2File = Foto2.FileName;
                    string Foto2Path = Path.Combine(Server.MapPath("~/Content/Img"), Foto2File);
                    Foto2.SaveAs(Foto2Path);
                    eventi.Foto2 = Foto2File;
                }
                
              

                if (Foto3 != null && Foto3.ContentLength > 0)
                {
                    string Foto3File = Foto3.FileName;
                    string Foto3Path = Path.Combine(Server.MapPath("~/Content/Img"), Foto3File);
                    Foto3.SaveAs(Foto3Path);
                    eventi.Foto3 = Foto3File;
                }

                if (Foto4 != null && Foto4.ContentLength > 0)
                {
                    string Foto4File = Foto4.FileName;
                    string Foto4Path = Path.Combine(Server.MapPath("~/Content/Img"), Foto4File);
                    Foto4.SaveAs(Foto4Path);
                    eventi.Foto4 = Foto4File;
                }
                db.Eventi.Add(eventi);
                db.SaveChanges();
                return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Errore = "Il campo Foto copertina è obbligatorio";
                    ViewBag.IdCategoria = new SelectList(db.Categorie, "IdCategoria", "NomeCategoria", eventi.IdCategoria);
                    return View();
                }
            }

            ViewBag.IdCategoria = new SelectList(db.Categorie, "IdCategoria", "NomeCategoria", eventi.IdCategoria);
            return View(eventi);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eventi eventi = db.Eventi.Find(id);
            if (eventi.FotoCopertina != null)
            {
                TempData["FotoCopertina"] = eventi.FotoCopertina;
            }
            else
            {
                TempData["FotoCopertina"] = "";
            }
            if (eventi.Foto1 != null)
            {
                TempData["Foto1"] = eventi.Foto1;
            }
            else
            {
                TempData["Foto1"] = "";
            }
            if (eventi.Foto2 != null)
            {
                TempData["Foto2"] = eventi.Foto2;
            }
            else
            {
                TempData["Foto2"] = "";
            }
            if (eventi.Foto3 != null)
            {
                TempData["Foto3"] = eventi.Foto3;
            }
            else
            {
                TempData["Foto3"] = "";
            }
            if (eventi.Foto4 != null)
            {
                TempData["Foto4"] = eventi.Foto4;
            }
            else
            {
                TempData["Foto4"] = "";
            }
            if (eventi == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCategoria = new SelectList(db.Categorie, "IdCategoria", "NomeCategoria", eventi.IdCategoria);
            return View(eventi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdEvento,NomeEvento,Descrizione,Prezzo,Luogo,Indirizzo,FotoCopertinaEdit,Foto1,Foto2,Foto3,Foto4,DataEvento,IdCategoria")] Eventi eventi, HttpPostedFileBase FotoCopertina, HttpPostedFileBase Foto1, HttpPostedFileBase Foto2, HttpPostedFileBase Foto3, HttpPostedFileBase Foto4)
        {
            if (ModelState.IsValid)
            {
                if (FotoCopertina != null && FotoCopertina.ContentLength > 0)
                {
                    string FotoCopertinaFile = FotoCopertina.FileName;
                    string FotoCopertinaPath = Path.Combine(Server.MapPath("~/Content/Img"), FotoCopertinaFile);
                    FotoCopertina.SaveAs(FotoCopertinaPath);
                    eventi.FotoCopertina = FotoCopertinaFile;
                }
                else
                {
                    eventi.FotoCopertina= TempData["FotoCopertina"].ToString();
                }

                if (Foto1 != null && Foto1.ContentLength > 0)
                {
                    string Foto1File = Foto1.FileName;
                    string Foto1Path = Path.Combine(Server.MapPath("~/Content/Img"), Foto1File);
                    Foto1.SaveAs(Foto1Path);
                    eventi.Foto1 = Foto1File;
                }
                else
                {
                    eventi.Foto1 = TempData["Foto1"].ToString();
                }

                if (Foto2 != null && Foto2.ContentLength > 0)
                {
                    string Foto2File = Foto2.FileName;
                    string Foto2Path = Path.Combine(Server.MapPath("~/Content/Img"), Foto2File);
                    Foto2.SaveAs(Foto2Path);
                    eventi.Foto2 = Foto2File;
                }
                else
                {
                    eventi.Foto2 = TempData["Foto2"].ToString();
                }



                if (Foto3 != null && Foto3.ContentLength > 0)
                {
                    string Foto3File = Foto3.FileName;
                    string Foto3Path = Path.Combine(Server.MapPath("~/Content/Img"), Foto3File);
                    Foto3.SaveAs(Foto3Path);
                    eventi.Foto3 = Foto3File;
                }
                else
                {
                    eventi.Foto3 = TempData["Foto3"].ToString();
                }

                if (Foto4 != null && Foto4.ContentLength > 0)
                {
                    string Foto4File = Foto4.FileName;
                    string Foto4Path = Path.Combine(Server.MapPath("~/Content/Img"), Foto4File);
                    Foto4.SaveAs(Foto4Path);
                    eventi.Foto4 = Foto4File;
                }
                else
                {
                    eventi.Foto4 = TempData["Foto4"].ToString();
                }

                db.Entry(eventi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCategoria = new SelectList(db.Categorie, "IdCategoria", "NomeCategoria", eventi.IdCategoria);
            return View(eventi);
        }

 
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eventi eventi = db.Eventi.Find(id);
            if (eventi == null)
            {
                return HttpNotFound();
            }
            return View(eventi);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Eventi eventi = db.Eventi.Find(id);
            db.Eventi.Remove(eventi);
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
