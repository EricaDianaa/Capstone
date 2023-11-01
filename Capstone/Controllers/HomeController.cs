using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            DateTime date = DateTime.Today;
            if(User.IsInRole("Azienda"))
            { 
                if (Session["Utente"] != null)
                {
                    int utente = (int)Session["Utente"];
                    List<Eventi> eventi1 = db.Eventi.Where(m => m.DataEvento >= date && m.IdUtente == utente).ToList();

                    ViewBag.Title = "Home Page";
                    return View(eventi1);
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }

            }
            else
            {
            List<Eventi> eventi= db.Eventi.Where(m=>m.DataEvento >= date).ToList();
            ViewBag.Title = "Home Page";
            return View(eventi);
            }
           
            
        }

        //Autentificazione
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Username, Password,Indirizzo,Email,Telefono,Ruolo,PartitaIva,CodiceFiscale")] Utenti u, bool IsAzienda, string Username, string CodiceFiscale, string Email)
        {
            if (ModelState.IsValid) {
                //Validazione Username/CodiceFiscale/Email 
                Utenti utente = db.Utenti.FirstOrDefault(m => m.Username == Username);
                Utenti utente1 = db.Utenti.FirstOrDefault(m => m.CodiceFiscale == CodiceFiscale);
                Utenti utente2 = db.Utenti.FirstOrDefault(m => m.Email == Email);
                //Messaggio di errore in caso Username sia già presenete nel database
                if (utente1 != null)
                {
                    ViewBag.Username = "Username non disponibile";

                }
                //Messaggio di errore in caso Email sia già presenete nel database
                if (utente2 != null)
                {
                    ViewBag.Email = "Email non disponibile";

                }
                //Messaggio di errore in caso CodiceFiscale sia già presenete nel database
                if (utente1 != null)
                {
                    ViewBag.CodiceFiscale = "Codice fiscale non disponibile";

                }
                try
                {
                    //Cripto la password 
                    using (var context = new ModelBContent())
                    {
                        var chkUser = (from s in context.Utenti where s.Username == u.Username || s.Email == u.Email select s).FirstOrDefault();
                        if (chkUser == null)
                        {
                            var keyNew = Hash.GeneratePassword(10);
                            var password = Hash.EncodePassword(u.Password, keyNew);
                           
                            //Se l'utente non è un admin assegno il ruolo User o Azienda
                            if (!User.IsInRole("Admin") || !User.IsInRole("Azienda"))
                            {
                                // Se è un azienda assegna ruolo come Azienda 
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
                            }
                            //Sicurezza password
                            if (!Regex.IsMatch(u.Password, @"\d"))
                            {
                                ViewBag.ErroreNumerico = "la password deve contenere un carattere numerico";

                            }
                            
                            if (!Regex.IsMatch(u.Password, @"[@#$%^&+=]"))
                            {
                                ViewBag.ErroreCarattereSpeciale = "la password deve contenere un carattere speciale";

                            }
                            if (!Regex.IsMatch(u.Password, @"[A-Z]"))
                            {
                                ViewBag.ErroreletteraMaiuscola = "la password deve contenere una lettera maiuscola";

                            }
                            if (Regex.IsMatch(u.Password, @"\d") && Regex.IsMatch(u.Password, @"[@#$%^&+=]") && Regex.IsMatch(u.Password, @"[A-Z]"))
                            {

                                if (utente == null && utente1 == null && utente2 == null)
                                {
                                    u.Password = password;
                                    u.VCode = keyNew;
                                    db.Utenti.Add(u);
                                    db.SaveChanges();
                                    ModelState.Clear();
                                    return RedirectToAction("Login", "Home");

                                }
                            }
                            

                            //Se Username/Email/CodiceFiscale non sono presenti nel database salvo l'utente

                            //Altrimenti rimando la pagin messaggi errore
                            else
                            {
                                return View();
                            }
                           
                        }
                        ViewBag.ErrorMessage = "Username gia esistente";
                        return View();
                    }
                }
                catch (Exception e)
                {
                    ViewBag.ErrorMessage = "Some exception occured" + e;
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
        public ActionResult Login()
        {

            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "IdUtente,UsernameLogin, Password")] Utenti u)
        {
            try
            {
                using (var context = new ModelBContent())
                {
                    var getUser = (from s in context.Utenti where s.Username == u.UsernameLogin || s.Email == u.UsernameLogin select s).FirstOrDefault();
                    if (getUser != null)
                    {
                        var hashCode = getUser.VCode; 
                        var encodingPasswordString = Hash.EncodePassword(u.Password, hashCode);   
                        var query = (from s in context.Utenti where (s.Username == u.UsernameLogin || s.Email == u.UsernameLogin) && s.Password.Equals(encodingPasswordString) select s).FirstOrDefault();
                        if (query != null)
                        {
                            FormsAuthentication.SetAuthCookie(u.UsernameLogin, false);
                            Session["Utente"] = query.IdUtente;
                            return RedirectToAction("Index", "Home");
                        }
                        ViewBag.ErrorMessage = "Username o password non validi";
                        return View();
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
        public ActionResult Logout()
        {
            Session["Cliente"] = null;
            FormsAuthentication.SignOut();
            Session["Utente"] = null;
            return RedirectToAction("Index", "Home");
        }

        //filtri
        [HttpPost]
        public JsonResult Filtri(string NomeCategoria,decimal Prezzo,DateTime DataEvento)
        {
            //data
            string data = Convert.ToString(DataEvento);
            if (data != "01/01/0001 00:00:00")
            {
                if (NomeCategoria == "Dal")
                {
                    List<Eventi> ListEventi = new List<Eventi>();
                    List<Eventi> eventi = db.Eventi.Where(m => m.DataEvento >= DataEvento&&m.DataEvento >= DateTime.Today).ToList();
                    foreach (Eventi e in eventi)
                    {
                        ListEventi.Add(new Eventi { IdEvento = e.IdEvento, NomeEvento = e.NomeEvento, DataE = e.DataEvento.ToShortDateString(), Descrizione = e.Descrizione, FotoCopertina = e.FotoCopertina, Prezzo = e.Prezzo, Indirizzo = e.Indirizzo, Luogo = e.Luogo });
                    }
                    return Json(ListEventi);
                }
                else
                {
                    List<Eventi> ListEventi = new List<Eventi>();
                    List<Eventi> eventi = db.Eventi.Where(m => m.DataEvento == DataEvento && m.DataEvento >= DateTime.Today).ToList();
                    foreach (Eventi e in eventi)
                    {
                        ListEventi.Add(new Eventi { IdEvento = e.IdEvento, NomeEvento = e.NomeEvento, DataE = e.DataEvento.ToShortDateString(), Descrizione = e.Descrizione, FotoCopertina = e.FotoCopertina, Prezzo = e.Prezzo, Indirizzo = e.Indirizzo, Luogo = e.Luogo });
                    }
                    return Json(ListEventi);
                }

            }
            //Rimuovi filtri(mostra tutta la lisegli eventi comoleta)
            else if(Prezzo==-1)
            {
                List<Eventi> ListEventi = new List<Eventi>();
                DateTime date = DateTime.Today;
                List<Eventi> eventi = db.Eventi.Where(m => m.DataEvento >= date && m.DataEvento >= DateTime.Today).ToList();
                foreach (Eventi e in eventi)
                {
                    ListEventi.Add(new Eventi { IdEvento = e.IdEvento, NomeEvento = e.NomeEvento, DataE = e.DataEvento.ToShortDateString(), Descrizione = e.Descrizione, FotoCopertina = e.FotoCopertina, Prezzo = e.Prezzo, Indirizzo = e.Indirizzo, Luogo = e.Luogo });
                }
                return Json(ListEventi);

            }
            //Per categoria
            else if (NomeCategoria != ""&&NomeCategoria!="Da" && NomeCategoria != "Dal")
            {
                Categorie c = db.Categorie.FirstOrDefault(m => m.NomeCategoria == NomeCategoria);
                List<Eventi> eventi = db.Eventi.Where(m => m.IdCategoria == c.IdCategoria&&m.DataEvento>= DateTime.Today).ToList();
                List<Eventi> ListEventi = new List<Eventi>();
                foreach (Eventi e in eventi)
                {
                    ListEventi.Add(new Eventi { IdEvento = e.IdEvento, NomeEvento = e.NomeEvento, DataE = e.DataEvento.ToShortDateString(), Descrizione = e.Descrizione, FotoCopertina = e.FotoCopertina, Prezzo = e.Prezzo, Indirizzo = e.Indirizzo, Luogo = e.Luogo });
                }
                return Json(ListEventi);
            }
            //Per prezzo(gratis)
            else if(Prezzo<=0)
            {
                List<Eventi> ListEventi = new List<Eventi>();
                List<Eventi> eventi = db.Eventi.Where(m => m.Prezzo <= Prezzo && m.DataEvento >= DateTime.Today).ToList();
                foreach (Eventi e in eventi)
                {
                    ListEventi.Add(new Eventi { IdEvento = e.IdEvento, NomeEvento = e.NomeEvento, DataE = e.DataEvento.ToShortDateString(), Descrizione = e.Descrizione, FotoCopertina = e.FotoCopertina, Prezzo = e.Prezzo,Indirizzo=e.Indirizzo,Luogo=e.Luogo });
                }
                return Json(ListEventi);
            }
            //(pagamento)
            else if (Prezzo >= 0)
            {
                if (NomeCategoria == "Da")
                {
                    List<Eventi> ListEventi = new List<Eventi>();
                    List<Eventi> eventi = db.Eventi.Where(m => m.Prezzo >= Prezzo && m.DataEvento >= DateTime.Today).ToList();
                    foreach (Eventi e in eventi)
                    {
                        ListEventi.Add(new Eventi { IdEvento = e.IdEvento, NomeEvento = e.NomeEvento, DataE = e.DataEvento.ToShortDateString(), Descrizione = e.Descrizione, FotoCopertina = e.FotoCopertina, Prezzo = e.Prezzo, Indirizzo = e.Indirizzo, Luogo = e.Luogo });
                    }
                    return Json(ListEventi);
                }
                else
                {
                List<Eventi> ListEventi = new List<Eventi>();
                List<Eventi> eventi = db.Eventi.Where(m => m.Prezzo == Prezzo && m.DataEvento >= DateTime.Today).ToList();
                foreach (Eventi e in eventi)
                {
                    ListEventi.Add(new Eventi { IdEvento = e.IdEvento, NomeEvento = e.NomeEvento, DataE = e.DataEvento.ToShortDateString(), Descrizione = e.Descrizione, FotoCopertina = e.FotoCopertina, Prezzo = e.Prezzo, Indirizzo = e.Indirizzo, Luogo = e.Luogo });
                }
                return Json(ListEventi);
                }
                
            }
            else
            {
                return Json(null);
            }
       
            
        }

        public ActionResult Account()
        {
            if (Session["Utente"] != null)
            {
                int id = (int)Session["Utente"];
                Utenti u = db.Utenti.FirstOrDefault(m => m.IdUtente == id);
                return View(u);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
               
        }


    }
}
