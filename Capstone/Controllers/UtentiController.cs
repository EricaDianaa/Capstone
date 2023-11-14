using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
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
            utenti.Password = u.Password;     
            if (!User.IsInRole("Admin"))
            {
            utenti.IsAzienda = u.IsAzienda;
            utenti.CodiceFiscale = u.CodiceFiscale;
            utenti.Ruolo = u.Ruolo;
            }
            var local = db.Set<Utenti>().Local.FirstOrDefault(f => f.IdUtente == u.IdUtente);
            if (local != null)
            {
                db.Entry(local).State = EntityState.Detached;
            }
            db.Entry(utenti).State = EntityState.Modified;
            db.SaveChanges();
            TempData["SuccessEdit"] = "La modifica è avvenuta con successo";
            if (User.IsInRole("Admin") || User.IsInRole("Azienda"))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Account", "Home");
            }
        }

        [AllowAnonymous]
        public ActionResult EditPassword()
        {
          
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPassword([Bind(Include = "IdUtente,Username,Ruolo,Password,Indirizzo,Email,Telefono,IsAzienda,CodiceFiscale,PartitaIva")] string NewPassword,string OldPassword)
        {
           
            int id = (int)Session["Utente"];
            if (id != 0)
            {
 try
            {
                using (var context = new ModelBContent())
                {
                    Utenti u = db.Utenti.Where(m => m.IdUtente == id).FirstOrDefault();
                    var hashCode = u.VCode;
                    var encodingPasswordString = Hash.EncodePassword(OldPassword, hashCode);
                    var query = (from s in context.Utenti where (s.Username == u.Username || s.Email == u.Username) && s.Password.Equals(encodingPasswordString) select s).FirstOrDefault();
                    if (query != null)
                    {
                    var keyNew = Hash.GeneratePassword(10);
                    

                        if (!Regex.IsMatch(NewPassword, @"\d"))
                        {
                            ViewBag.ErroreNumerico = "la password deve contenere un carattere numerico";

                        }

                        if (!Regex.IsMatch(NewPassword, @"[@#$%^&+=]"))
                        {
                            ViewBag.ErroreCarattereSpeciale = "la password deve contenere un carattere speciale";

                        }
                        if (!Regex.IsMatch(NewPassword, @"[A-Z]"))
                        {
                            ViewBag.ErroreletteraMaiuscola = "la password deve contenere una lettera maiuscola";

                        }
                        if (Regex.IsMatch(NewPassword, @"\d") && Regex.IsMatch(NewPassword, @"[@#$%^&+=]") && Regex.IsMatch(NewPassword, @"[A-Z]"))
                        {
                            var password = Hash.EncodePassword(NewPassword, keyNew);
                            u.Password = password;
                            u.VCode = keyNew;
                            db.Entry(u).State = EntityState.Modified;
                            db.SaveChanges();
                            ModelState.Clear();
                             TempData["Success"] = "La password è stata modificata con successo";
                            return RedirectToAction("Account", "home");


                        }
                    }


                    ViewBag.ErrorMessage = "Username o password non validi";
                    return View();
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = " Error!!! contact cms@info.in";
                return View();
            }
            }
            else
            {
                return View();
            }

        }

        public ActionResult RecuperoPassword()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperoPassword([Bind(Include = "IdUtente,Username,Ruolo,Password,Indirizzo,Email,Telefono,IsAzienda,CodiceFiscale,PartitaIva")] string NewPassword, string CodiceFiscale,string Username)
        {

            try
            {
                using (var context = new ModelBContent())
                {
                    Utenti u = db.Utenti.Where(m => m.CodiceFiscale == CodiceFiscale&&m.Username==Username).FirstOrDefault();
                 
                  
                    if (u != null)
                    {
                        var keyNew = Hash.GeneratePassword(10);
                        if (!Regex.IsMatch(NewPassword, @"\d"))
                        {
                            ViewBag.ErroreNumerico = "la password deve contenere un carattere numerico";

                        }

                        if (!Regex.IsMatch(NewPassword, @"[@#$%^&+=]"))
                        {
                            ViewBag.ErroreCarattereSpeciale = "la password deve contenere un carattere speciale";

                        }
                        if (!Regex.IsMatch(NewPassword, @"[A-Z]"))
                        {
                            ViewBag.ErroreletteraMaiuscola = "la password deve contenere una lettera maiuscola";

                        }
                        if (Regex.IsMatch(NewPassword, @"\d") && Regex.IsMatch(NewPassword, @"[@#$%^&+=]") && Regex.IsMatch(NewPassword, @"[A-Z]"))
                        {
                            var password = Hash.EncodePassword(NewPassword, keyNew);
                            u.Password = password;
                            u.VCode = keyNew;
                            db.Entry(u).State = EntityState.Modified;
                            db.SaveChanges();
                            ModelState.Clear();
                            return RedirectToAction("index", "Home");


                        }
                    }
                    ViewBag.ErrorMessage = "Username o codice fiscale non validi";
                    return View();
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = " Error!!! contact cms@info.in";
                return View();
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
