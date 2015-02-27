using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using StackOverflow.Data;
using StackOverflow.Domain.Entities;
using StackOverflow.Domain.Services;
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
            if (ModelState.IsValid && model.ConfirmPassword == model.Password)
            {
                Account newAccount = _mappingEngine.Map<AccountRegisterModel, Account>(model);
                var context = new StackOverflowContext();
                context.Accounts.Add(newAccount);
                context.SaveChanges();
                return RedirectToAction("Login");
            }
            model.Password = "";
            model.ConfirmPassword = "";
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
                var context = new StackOverflowContext();
                Account account = context.Accounts.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);
                if (account != null)
                {
                    FormsAuthentication.SetAuthCookie(account.Id.ToString(), false);

                    return RedirectToAction("Index", "Question");
                }
            }
            ViewBag.Message = "Email or Password invalid";
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
            var context = new StackOverflowContext();
            Account account = context.Accounts.FirstOrDefault(x => x.Email == model.Email);
            if (account == null)
                return View(new ForgotPasswordModel());
            IEmailSender email = new EmailSender();
            
            email.SendEmail(model.Email, "Account/ChangePassword/"+account.Id.ToString());
            return View(model);
        }

        public ActionResult Profile(Guid id)
        {
            var context = new StackOverflowContext();
            Account account = context.Accounts.FirstOrDefault(x => x.Id == id);
            if (account != null)
            {
                context.Entry(account).Collection(p => p.Questions).Load();
                context.Entry(account).Collection(p => p.Answers).Load();
                var model = _mappingEngine.Map<Account, AccountProfileModel>(account);
                
                return View(model);
            }  
            
            return RedirectToAction("Index", "Question");
        }

        public ActionResult ChangePassword(Guid id)
        {
            var model = new ChangePasswordModel() {Id = id};
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            var context = new StackOverflowContext();
            Account account = context.Accounts.FirstOrDefault(x => x.Id == model.Id);
            if (model.Password == model.ConfirmPassword && account != null)
            {
                account.Password = model.Password;
                context.Entry(account).State = EntityState.Modified;
                context.SaveChanges();
            }
                
            return View(new ChangePasswordModel());
        }
    }
}
