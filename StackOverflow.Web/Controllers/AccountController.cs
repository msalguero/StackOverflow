using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using StackOverflow.Domain.Entities;
using StackOverflow.Web.Models;

namespace StackOverflow.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMappingEngine _mappingEngine;

        public AccountController(IMappingEngine mappingEngine)
        {
            _mappingEngine = mappingEngine;
        }

        public ActionResult Register()
        {
            return View(new AccountRegisterModel());
        }

        [HttpPost]
        public ActionResult Register(AccountRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                Account newAccount = _mappingEngine.Map<AccountRegisterModel, Account>(model);

                return RedirectToAction("Login");
            }
            return View(model);
        }

        public ActionResult Login()
        {
            return View(new AccountLoginModel());
        }

        [HttpPost]
        public ActionResult Login(AccountLoginModel model)
        {
            if (ModelState.IsValid)
            {
                Account newAccount = _mappingEngine.Map<AccountLoginModel, Account>(model);
                FormsAuthentication.SetAuthCookie(newAccount.Id.ToString(), false);

                return RedirectToAction("Index", "Question");
            }

            return View(new AccountLoginModel());
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Question");
        }

        public ActionResult ForgotPassword()
        {
            return View(new ForgotPasswordModel());
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordModel model)
        {
            @ViewBag.Message = "El correo fue enviado";
            return View(model);
        }

        public ActionResult Profile(Guid id)
        {
            return View(new AccountProfileModel());
        }

        public ActionResult ChangePassword()
        {
            return View(new ChangePasswordModel());
        }
    }
}
