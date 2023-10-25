using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Capstone.Controllers
{
    public class ValidateController : Controller
    {
        ModelBContent db = new ModelBContent();
        //Validazioni email e Username 

        public ActionResult IsEmailValid(string Email)
        {
            bool isValid = db.Utenti.All(x => x.Email != Email); ;
            return Json(isValid, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsNameValid(string Username)
        {
       bool isValid = db.Utenti.All(x => x.Username != Username); ;
            return Json(isValid, JsonRequestBehavior.AllowGet);
        }


    }
}