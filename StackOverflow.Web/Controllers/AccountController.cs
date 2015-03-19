using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using MvcReCaptcha;
using StackOverflow.Data;
using StackOverflow.Domain.Entities;
using StackOverflow.Domain.Services;
using StackOverflow.Web.Models;

namespace StackOverflow.Web.Controllers
{
    
    [Logging]
    public class AccountController : Controller
    {
        private readonly IMappingEngine _mappingEngine;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _email;
        private int loginAttempts;

        public AccountController(IMappingEngine mappingEngine)
        {
            _mappingEngine = mappingEngine;
            _unitOfWork = new UnitOfWork();
            _email = new MailgunSender();
            loginAttempts = 0;
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
                if (_unitOfWork.AccountRepository.GetWithFilter(x => x.Email == model.Email) != null)
                {
                    return View(new AccountRegisterModel());
                }
                Account newAccount = _mappingEngine.Map<AccountRegisterModel, Account>(model);
                _unitOfWork.AccountRepository.Add(newAccount);
                _unitOfWork.Commit();

                var hostName = HttpContext.Request.Url.Host;
                if (hostName == "localhost")
                    hostName = Request.Url.GetLeftPart(UriPartial.Authority);
                _email.SendEmail(model.Email, "To validate the account go to: " + hostName + "/Account/ValidateAccount/" + newAccount.Id.ToString());
                return RedirectToAction("Login");
            }
            model.Password = "";
            model.ConfirmPassword = "";
            return View(model);
        }

        public ActionResult Login()
        {
            if (loginAttempts > 3)
            {
                @ViewBag.Captcha = true;
                loginAttempts = 0;
            }
                
            return View(new AccountLoginModel());
        }

        [HttpPost]
        [CaptchaValidator]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Login(AccountLoginModel model, bool captchaValid)
        {
            if (!captchaValid)
            {
                ModelState.AddModelError("_FORM", "You did not type the verification word correctly. Please try again.");
            }
            if (ModelState.IsValid)
            {
                Account account = _unitOfWork.AccountRepository.GetWithFilter(x => x.Email == model.Email);
                if (account != null)
                {
                    if (account.Password == model.Password)
                    {
                        if (!account.IsActive)
                        {
                            ViewBag.Message = "Please validate the account";
                            return View(new AccountLoginModel());
                        }
                        FormsAuthentication.SetAuthCookie(account.Id.ToString(), false);

                        return RedirectToAction("Index", "Question");
                    }
                    _email.SendEmail(account.Email, "There was a failed attempt to enter your account.");
                    loginAttempts++;
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
            Account account = _unitOfWork.AccountRepository.GetWithFilter(x => x.Email == model.Email);
            if (account == null)
            {
                @ViewBag.Message = "The email does not exist";
                return View(new ForgotPasswordModel());
            }
                
            var hostName = HttpContext.Request.Url.Host;
            if (hostName == "localhost")
                hostName = Request.Url.GetLeftPart(UriPartial.Authority);

            @ViewBag.Message = "The email has been sent with further instructions";
            _email.SendEmail(model.Email, hostName+"/Account/ChangePassword/"+account.Id.ToString());
            return View(model);
        }

        public ActionResult Profile(Guid id)
        {
            Account account = _unitOfWork.AccountRepository.GetById(id);
            if (account != null)
            {
                _unitOfWork.AccountRepository.Load(account, "Questions");
                _unitOfWork.AccountRepository.Load(account, "Answers");
                account.ProfileViews += 1;
                account.LastProfileViewDate = DateTime.Now;
                _unitOfWork.AccountRepository.Update(account);
                _unitOfWork.Commit();
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
            Account account = _unitOfWork.AccountRepository.GetById(model.Id);
            if (model.Password == model.ConfirmPassword && account != null)
            {
                account.Password = model.Password;
                _unitOfWork.AccountRepository.Update(account);
                _unitOfWork.Commit();
            }
            ViewBag.Message = "Your password has been updated";
            return RedirectToAction("Login");
        }

        public ActionResult ValidateAccount(Guid id)
        {
            Account account = _unitOfWork.AccountRepository.GetById(id);

            account.IsActive = true;
            _unitOfWork.AccountRepository.Update(account);
            _unitOfWork.Commit();
            return RedirectToAction("Login");
        }
    }
}
