using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackOverflow.Web.Models;

namespace StackOverflow.Web.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Register()
        {
            return View(new AccountRegisterModel());
        }
    }
}
