﻿using System;
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
    [Logging]
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
        public ActionResult Index(string ordering, int? page)
        {
            int pageNumber = (page ?? 0)*25;
            @ViewBag.Page = (page ?? 0);
            @ViewBag.active = "Date";
            @ViewBag.PreviousEnabled = true;
            @ViewBag.NextEnabled = true;
            List<QuestionListModel> models = new List<QuestionListModel>();
            var questions = _unitOfWork.QuestionRepository.GetList().OrderByDescending(x=> x.CreationDate).ToList();
            
            switch (ordering)
            {
                case "Votes":
                    questions = questions.OrderByDescending(x => x.Votes).ToList();
                    @ViewBag.active = "Votes";
                    break;
                case "Views":
                    questions = questions.OrderByDescending(x => x.Views).ToList();
                    @ViewBag.active = "Views";
                    break;
                case "Answers":
                    questions = questions.OrderByDescending(x => x.Answers.Count).ToList();
                    @ViewBag.active = "Answers";
                    break;
            }
            foreach (var item in questions)
            {

                var model = Mapper.Map<Question, QuestionListModel>(item);
                models.Add(model);
            }
            if (models.Count <= pageNumber+25)
                @ViewBag.NextEnabled = false;
            if (pageNumber == 0)
                @ViewBag.PreviousEnabled = false;
            if (pageNumber + 25 > models.Count)
                return View(models.GetRange(pageNumber, models.Count - pageNumber));
            return View(models.GetRange(pageNumber,25));
        }
        [AllowAnonymous]
        public ActionResult Details(Guid id)
        {
            Question question = _unitOfWork.QuestionRepository.GetById(id);
            if (question == null)
                return RedirectToAction("Index");
            question.Answers = question.Answers.OrderByDescending(c => c.Correct).ToList();
            if (!question.IsAnswered)
            {
                question.Answers = question.Answers.OrderByDescending(c => c.Votes).ToList();
                if (question.Answers.Count > 0 && question.Answers.ElementAt(0).Votes == 0)
                    question.Answers = question.Answers.OrderByDescending(c => c.CreationDate).ToList();
            }
                
            QuestionDetailsModel questionModel = Mapper.Map<Question,QuestionDetailsModel>(question);

            question.Views += 1;
            _unitOfWork.QuestionRepository.Update(question);
            _unitOfWork.Commit();

            var md = new MarkdownDeep.Markdown();
            md.ExtraMode = true;
            md.SafeMode = false;
            questionModel.Description = md.Transform(questionModel.Description);
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
            Guid idQuestion = Guid.Parse(TempData["id"].ToString());
            if (description.Length < 50)
            {
                TempData["Error"] = "Answer must be more than 50 characters";
                return RedirectToAction("Details", new { id = idQuestion });
            }
            string[] words = description.Split(' ');
            if (words.Count() < 5 || (words.Last() == "" && words.Count() == 5))
            {
                TempData["Error"] = "Answer must have at least 5 words";
                return RedirectToAction("Details", new { id = idQuestion });
                
            }
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
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult VoteQuestionUp(QuestionDetailsModel item)
        {
            item.Votes += 1;
            return RedirectToAction("VoteQuestion", item);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult VoteQuestionDown(QuestionDetailsModel item)
        {
            item.Votes += -1;
            return RedirectToAction("VoteQuestion", item);
        }
        [ValidateInput(false)]
        public ActionResult VoteQuestion(QuestionDetailsModel item)
        {
            Guid voterId = Guid.Parse(HttpContext.User.Identity.Name);
            var votes = _unitOfWork.VoteRepository.GetList();
            var voter = votes.FirstOrDefault(x => x.Voter.Id == voterId && x.Question != null &&  x.Question.Id == item.Id);
            if (voter != null)
                return RedirectToAction("Details", "Question", new { id = item.Id });
            Question question = Mapper.Map<QuestionDetailsModel, Question>(item);
            var account = _unitOfWork.AccountRepository.GetById(item.OwnerId);
            question.Owner = account;
            question.ModificationDate = DateTime.Now;
            _unitOfWork.QuestionRepository.Update(question);
            _unitOfWork.Commit();

            var voterAccount = _unitOfWork.AccountRepository.GetById(voterId);
            _unitOfWork.VoteRepository.Add(new Vote() { Question = question, Voter = voterAccount });
            _unitOfWork.Commit();

            Guid idQuestion = Guid.Parse(TempData["id"].ToString());
            return RedirectToAction("Details", new { id = idQuestion });
        }
        

        [ValidateInput(false)]
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

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult RemoveCorrectAnswer(AnswerModel itemModel)
        {
            Answer answer = Mapper.Map<AnswerModel, Answer>(itemModel);
            var question = _unitOfWork.QuestionRepository.GetById(itemModel.QuestionId);
            var account = _unitOfWork.AccountRepository.GetById(itemModel.OwnerId);
            answer.Question = question;
            answer.Owner = account;
            answer.Correct = false;
            question.IsAnswered = false;
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