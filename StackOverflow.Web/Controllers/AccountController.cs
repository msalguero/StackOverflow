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
    
    [Logging]
    public class AccountController : Controller
    {
        private readonly IMappingEngine _mappingEngine;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IMappingEngine mappingEngine)
        {
            _mappingEngine = mappingEngine;
            _unitOfWork = new UnitOfWork();
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
                _unitOfWork.AccountRepository.Add(newAccount);
                _unitOfWork.Commit();
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
                Account account = _unitOfWork.AccountRepository.GetWithFilter(x => x.Email == model.Email && x.Password == model.Password);
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
            Account account = _unitOfWork.AccountRepository.GetWithFilter(x => x.Email == model.Email);
            if (account == null)
                return View(new ForgotPasswordModel());
            IEmailSender email = new EmailSender();
            
            email.SendEmail(model.Email, "Account/ChangePassword/"+account.Id.ToString());
            return View(model);
        }

        public ActionResult Profile(Guid id)
        {
            Account account = _unitOfWork.AccountRepository.GetById(id);
            if (account != null)
            {
                _unitOfWork.AccountRepository.Load(account, "Questions");
                _unitOfWork.AccountRepository.Load(account, "Answers");
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
                
            return View(new ChangePasswordModel());
        }
    }
}
