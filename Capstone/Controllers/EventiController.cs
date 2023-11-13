using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
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
            if (User.IsInRole("Azienda"))
            {
                if (Session["Utente"] != null)
                {
                    int idUtente = (int)Session["Utente"];
                    var eventi = db.Eventi.Include(e => e.Categorie).Include(e => e.Recensioni).Where(m => m.IdUtente == idUtente);
                    return View(eventi.ToList());
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }

            }
            else
            {
              var eventi = db.Eventi.Include(e => e.Categorie).Include(e=>e.Recensioni);
              return View(eventi.ToList());
            }
            
        }
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eventi eventi = db.Eventi.Find(id);
            if (Session["Utente"] != null)
            {
                int IdUtente = (int)Session["Utente"];
                List<Recensioni> r = db.Recensioni.Where(m => m.IdEvento == id && m.IdUtente == IdUtente).ToList();
                ViewBag.Recensioni = r;
                List<Recensioni> re = db.Recensioni.Where(m => m.IdEvento == id && m.IdUtente != IdUtente).ToList();
                ViewBag.RecensioniUtente = re;
                ViewBag.CountRecensione = db.Recensioni.Where(m=> m.IdEvento == id).Count();

                //Media Recensioni
                if (ViewBag.CountRecensione != 0)
                {
                 ViewBag.MediaRecensione = db.Recensioni.Where(m => m.IdEvento == id).Average(m => m.Voto);
                 ViewBag.MediaRecensione = Math.Round(ViewBag.MediaRecensione,2);
                } 
                //Percentuale Recensioni
                //Totale recensioni evento
                var Recensioni = db.Recensioni.Where(m => m.IdEvento == id).Count();
                //Numero recensioni stelle
                int star5 = db.Recensioni.Where(m => m.IdEvento == id&&m.Voto==5).Count();
                int star4 = db.Recensioni.Where(m => m.IdEvento == id && m.Voto == 4).Count();
                int star3 = db.Recensioni.Where(m => m.IdEvento == id && m.Voto == 3).Count();
                int star2 = db.Recensioni.Where(m => m.IdEvento == id && m.Voto == 2).Count();
                int star1 = db.Recensioni.Where(m => m.IdEvento == id && m.Voto == 1).Count();
                //Calcolo percentuale
                ViewBag.Tot5st = ((double)star5 / Recensioni) * 100;
                ViewBag.Tot4st = ((double)star4 / Recensioni) * 100;
                ViewBag.Tot3st = ((double)star3 / Recensioni) * 100;
                ViewBag.Tot2st = ((double)star2 / Recensioni) * 100;
                ViewBag.Tot1st = ((double)star1 / Recensioni) * 100;
                //Arrotondo il numero
                ViewBag.Tot5st =  Math.Round(ViewBag.Tot5st);
                ViewBag.Tot4st= Math.Round(ViewBag.Tot4st);
                ViewBag.Tot3st= Math.Round(ViewBag.Tot3st);
                ViewBag.Tot2st= Math.Round(ViewBag.Tot2st);
                ViewBag.Tot1st= Math.Round(ViewBag.Tot1st);
                //Preferiti(per popolare il cuore all'avvio della pagina)
                Preferiti p = db.Preferiti.Where(m => m.IdEvento == id && m.IdUtente == IdUtente).FirstOrDefault();
                ViewBag.Preferiti = p;
            }

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
        public ActionResult Create([Bind(Include = "IdEvento,NomeEvento,Descrizione,Prezzo,Luogo,Indirizzo,FotoCopertina,Foto1,Foto2,Foto3,Foto4,DataEvento,IdCategoria,DataDa")] Eventi eventi, HttpPostedFileBase FotoCopertina, HttpPostedFileBase Foto1, HttpPostedFileBase Foto2, HttpPostedFileBase Foto3, HttpPostedFileBase Foto4)
        {
            if (ModelState.IsValid)
            {
                if (Session["Utente"] != null)
                {
                    eventi.IdUtente = (int)Session["Utente"];
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

                        if (Foto2 != null && Foto2.ContentLength > 0)
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
                        return RedirectToAction("Index", "Eventi");
                    }
                    else
                    {
                        ViewBag.Errore = "Il campo Foto copertina è obbligatorio";
                        ViewBag.IdCategoria = new SelectList(db.Categorie, "IdCategoria", "NomeCategoria", eventi.IdCategoria);
                        return View();
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }

            ViewBag.IdCategoria = new SelectList(db.Categorie, "IdCategoria", "NomeCategoria", eventi.IdCategoria);
            return View(eventi);
        }
        public ActionResult Edit(int? id)
        {
            CultureInfo.CurrentCulture = new CultureInfo("it-IT");
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
        public ActionResult Edit([Bind(Include = "IdEvento,NomeEvento,Descrizione,Prezzo,Luogo,Indirizzo,FotoCopertinaEdit,Foto1,Foto2,Foto3,Foto4,DataEvento,IdCategoria,DataDa")] Eventi eventi, HttpPostedFileBase FotoCopertina, HttpPostedFileBase Foto1, HttpPostedFileBase Foto2, HttpPostedFileBase Foto3, HttpPostedFileBase Foto4)
        {
            CultureInfo.CurrentCulture = new CultureInfo("it-IT");
            if (ModelState.IsValid)
            {
                if (Session["Utente"] != null)
                {
                    eventi.IdUtente = (int)Session["Utente"];
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
                   
                    Eventi e =db.Eventi.Where(m=>m.IdEvento==eventi.IdEvento).FirstOrDefault();
                    if (eventi.DataDa == null)
                    {
                        eventi.DataDa = e.DataDa;
                    }
                    if (eventi.DataEvento == null)
                    {
                        eventi.DataEvento = e.DataEvento;
                    }
                    var local = db.Set<Eventi>().Local.FirstOrDefault(f => f.IdEvento == eventi.IdEvento);
                    if (local != null)
                    {
                        db.Entry(local).State = EntityState.Detached;
                    }
                    db.Entry(eventi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Eventi");
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
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
            //Se listaOrdini ha ordini correlati all'evento che non sono ancora terminati(la data dell'evento non è ancora scaduta)è impossibile eliminare l'elemento
            List<ListaOrdini> listaOrdini = db.ListaOrdini.Where(m => m.IdEvento == id && m.Eventi.DataEvento >= DateTime.Today).ToList();
            if (listaOrdini.Count >= 1)
            {
                TempData["ErroreElimina"] = "Errore";
                ViewBag.Errore = "Impossibile eliminare l'elemento perchè ci sono ordini non ancora conclusi";
                return RedirectToAction("index","Eventi");
            }//Altrimenti elimina
            else
            {//prima elimina tutti la lista degli ordini
                List<ListaOrdini> l = db.ListaOrdini.Where(m => m.IdEvento == id && m.Eventi.DataEvento <= DateTime.Today).ToList();
                foreach (ListaOrdini ordine in l)
                {
                    db.ListaOrdini.Remove(ordine);
                    db.SaveChanges();
                }//poi le recensioni
                List<Recensioni> r = db.Recensioni.Where(m => m.IdEvento == id).ToList();
                if (r.Count >= 1)
                {
                    foreach (Recensioni recensioni in r)
                    {
                        db.Recensioni.Remove(recensioni);
                        db.SaveChanges();
                    }
                }
                //poi i preferiti
                Preferiti preferiti = db.Preferiti.Where(m => m.IdEvento == id).FirstOrDefault();
                if (preferiti != null)
                {
                    db.Preferiti.Remove(preferiti);
                    db.SaveChanges();
                }
                //infine l'evento
                Eventi eventi = db.Eventi.Find(id);
                db.Eventi.Remove(eventi);
                db.SaveChanges();
                TempData["Elimina"] = "ElimnaSuccesso";
                return RedirectToAction("Index", "Eventi");

            }

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
