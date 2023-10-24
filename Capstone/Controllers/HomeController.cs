using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Capstone.Controllers
{
    public class HomeController : Controller
    {
        ModelBContent db = new ModelBContent();
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        //Autentificazione
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Username, Password,Indirizzo,Email,Telefono,Ruolo,PartitaIva,CodiceFiscale")] Utenti u,bool IsAzienda)
        {
            
            if (ModelState.IsValid)
            {   // Se è un azienda assegna ruolo come Azienda 
                if (IsAzienda == true)
                {
                   u.Ruolo = "Azienda";
                    u.IsAzienda = IsAzienda;
                }
                //Altrimenti come User
                else
                {
                    u.Ruolo = "User";
                    u.IsAzienda = IsAzienda;
                }
            //Creazione dell 'utente nel DB
            Utenti user = db.Utenti.Add(u);
            db.SaveChanges();
            return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
           ;
        }
        public ActionResult Login()
        {

            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "IdUtente,Username, Password")] Utenti u)
        {   
            //Selezione dell'utente ricevuto all'interno del DB per verificare l'autentificazione
            Utenti users = db.Utenti.FirstOrDefault(m => m.Username == u.Username & m.Password == u.Password);
            if (users != null)
            {
                FormsAuthentication.SetAuthCookie(u.Username, false);
                return RedirectToAction("Index");
            }

            return View();

        }
        public ActionResult Logout()
        {
            Session["Cliente"] = null;
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

     

    }
}
