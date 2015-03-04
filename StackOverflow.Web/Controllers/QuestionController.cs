using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using StackOverflow.Data;
using StackOverflow.Domain.Entities;
using StackOverflow.Web.Models;

namespace StackOverflow.Web.Controllers
{
    [Authorize]
    public class QuestionController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public QuestionController()
        {
            _unitOfWork = new UnitOfWork();
        }
        // GET: Question
        [AllowAnonymous]
        public ActionResult Index()
        {
            List<QuestionListModel> models = new List<QuestionListModel>();
            var questions = _unitOfWork.QuestionRepository.GetList();
            foreach (var item in questions)
            {
                var model = Mapper.Map<Question, QuestionListModel>(item);
                models.Add(model);
            }
            return View(models);
        }
        [AllowAnonymous]
        public ActionResult Details(Guid id)
        {
            Question question = _unitOfWork.QuestionRepository.GetById(id);
            if (question == null)
                return RedirectToAction("Index");
            question.Answers = question.Answers.OrderByDescending(c => c.Correct).ToList();
            QuestionDetailsModel questionModel = Mapper.Map<Question,QuestionDetailsModel>(question);
           
            return View(questionModel);
        }
        //[AllowAnonymous]
        //public PartialViewResult AnswerIndex(ICollection<AnswerModel> models)
        //{
        //    return PartialView("AnswerIndex", models);
        //}

        public ActionResult Ask()
        {
            return View(new AskQuestionModel());
        }

        [HttpPost]
        public ActionResult CreateAnswer(string description)
        {
            Guid idQuestion = Guid.Parse( TempData["id"].ToString());
            var question = _unitOfWork.QuestionRepository.GetById(idQuestion);
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];         
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            Guid creatorId = Guid.Parse(ticket.Name);
            var account = _unitOfWork.AccountRepository.GetById(creatorId);
            if (account == null || question == null)
            {
                return RedirectToAction("Index");
            }
            var answer = new Answer
            {
                Description = description, CreationDate = DateTime.Now,
                Owner = account, Question = question
            };
            _unitOfWork.AnswerRepository.Add(answer);
            _unitOfWork.Commit();
            return RedirectToAction("Details", new{id = question.Id});
            //return RedirectToAction("Details", new {id = model.QuestionId});
        }
        [HttpPost]
        public ActionResult VoteQuestionUp(QuestionDetailsModel item)
        {
            item.Votes += 1;
            return RedirectToAction("VoteQuestion", item);
        }

        [HttpPost]
        public ActionResult VoteQuestionDown(QuestionDetailsModel item)
        {
            item.Votes += -1;
            return RedirectToAction("VoteQuestion", item);
        }
        public ActionResult VoteQuestion(QuestionDetailsModel item)
        {
            Question question = Mapper.Map<QuestionDetailsModel, Question>(item);
            var account = _unitOfWork.AccountRepository.GetById(item.OwnerId);
            question.Owner = account;
            question.ModificationDate = DateTime.Now;
            _unitOfWork.QuestionRepository.Update(question);
            _unitOfWork.Commit();
            Guid idQuestion = Guid.Parse(TempData["id"].ToString());
            return RedirectToAction("Details", new { id = idQuestion });
        }
        [HttpPost]
        public ActionResult VoteAnswerUp(AnswerModel item)
        {
            item.Votes += 1;
            return RedirectToAction("VoteAnswer", item);
        }

        [HttpPost]
        public ActionResult VoteAnswerDown(AnswerModel item)
        {
            item.Votes += -1;
            return RedirectToAction("VoteAnswer", item);
        }

        public ActionResult VoteAnswer(AnswerModel itemModel)
        {
            Answer answer = Mapper.Map<AnswerModel, Answer>(itemModel);
           
            var question = _unitOfWork.QuestionRepository.GetById(itemModel.QuestionId);
            var account = _unitOfWork.AccountRepository.GetById(itemModel.OwnerId);
            answer.Question = question;
            answer.Owner = account;
            
            Guid voterId = Guid.Parse(HttpContext.User.Identity.Name);
            
            _unitOfWork.AnswerRepository.Update(answer);
            _unitOfWork.Commit();

            Guid idQuestion = Guid.Parse(TempData["id"].ToString());
            return RedirectToAction("Details", new { id = idQuestion });
        }

        [HttpPost]
        public ActionResult SelectCorrectAnswer(AnswerModel itemModel)
        {
            Answer answer = Mapper.Map<AnswerModel, Answer>(itemModel);
            var question = _unitOfWork.QuestionRepository.GetById(itemModel.QuestionId);
            var account = _unitOfWork.AccountRepository.GetById(itemModel.OwnerId);
            answer.Question = question;
            answer.Owner = account;
            answer.Correct = true;
            question.IsAnswered = true;
            _unitOfWork.AnswerRepository.Update(answer);
            _unitOfWork.Commit();

            Guid idQuestion = Guid.Parse(TempData["id"].ToString());
            return RedirectToAction("Details", new { id = idQuestion });
        }

        [HttpPost]
        public ActionResult Ask(AskQuestionModel model)
        {
            if (ModelState.IsValid)
            {
                Question question = Mapper.Map<AskQuestionModel, Question>(model);
                
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    Guid creatorId = Guid.Parse(ticket.Name);
                    question.Owner = _unitOfWork.AccountRepository.GetById(creatorId);
                    question.ModificationDate = question.CreationDate =
                        DateTime.Now;
                }
                
                _unitOfWork.QuestionRepository.Add(question);
                _unitOfWork.Commit();
                return RedirectToAction("Details", new { id = question.Id });
            }
            return View(model);
        }
    }
}