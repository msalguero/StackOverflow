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
        // GET: Question
        [AllowAnonymous]
        public ActionResult Index()
        {
            List<QuestionListModel> models = new List<QuestionListModel>();
            var context = new StackOverflowContext();
            var query = from q in context.Questions
                        orderby q.CreationDate
                        select q;
         
            foreach (var item in query)
            {
                var model = Mapper.Map<Question, QuestionListModel>(item);
                models.Add(model);
            } 
            return View(models);
        }
        [AllowAnonymous]
        public ActionResult Details(Guid id)
        {
            var context = new StackOverflowContext();
            Question question = context.Questions.FirstOrDefault(x => x.Id == id);
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
            var context = new StackOverflowContext();
            Guid idQuestion = Guid.Parse( TempData["id"].ToString());
            var question = context.Questions.FirstOrDefault(q => q.Id == idQuestion);
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];         
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            Guid creatorId = Guid.Parse(ticket.Name);
            var account = context.Accounts.FirstOrDefault(a => a.Id == creatorId);
            if (account == null || question == null)
            {
                return RedirectToAction("Index");
            }
            var answer = new Answer
            {
                Description = description, CreationDate = DateTime.Now,
                Owner = account, Question = question
            };

            context.Answers.Add(answer);
            context.SaveChanges();
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
            var context = new StackOverflowContext();
            Question question = Mapper.Map<QuestionDetailsModel, Question>(item);
            var account = context.Accounts.FirstOrDefault(a => a.Id == item.OwnerId);
            question.Owner = account;
            question.ModificationDate = DateTime.Now;
            context.Entry(question).State = EntityState.Modified;
            context.SaveChanges();
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
            var context = new StackOverflowContext();
            var question = context.Questions.FirstOrDefault(q => q.Id == itemModel.QuestionId);
            var account = context.Accounts.FirstOrDefault(a => a.Id == itemModel.OwnerId);
            answer.Question = question;
            answer.Owner = account;
            context.Entry(answer).State = EntityState.Modified;
            context.SaveChanges();

            Guid idQuestion = Guid.Parse(TempData["id"].ToString());
            return RedirectToAction("Details", new { id = idQuestion });
        }

        [HttpPost]
        public ActionResult SelectCorrectAnswer(AnswerModel itemModel)
        {
            Answer answer = Mapper.Map<AnswerModel, Answer>(itemModel);
            var context = new StackOverflowContext();
            var question = context.Questions.FirstOrDefault(q => q.Id == itemModel.QuestionId);
            var account = context.Accounts.FirstOrDefault(a => a.Id == itemModel.OwnerId);
            answer.Question = question;
            answer.Owner = account;
            answer.Correct = true;
            question.IsAnswered = true;
            context.Entry(answer).State = EntityState.Modified;
            context.SaveChanges();

            Guid idQuestion = Guid.Parse(TempData["id"].ToString());
            return RedirectToAction("Details", new { id = idQuestion });
        }

        [HttpPost]
        public ActionResult Ask(AskQuestionModel model)
        {
            if (ModelState.IsValid)
            {
                var context = new StackOverflowContext();
                Question question = Mapper.Map<AskQuestionModel, Question>(model);
                
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    Guid creatorId = Guid.Parse(ticket.Name);
                    question.Owner = context.Accounts
                    .FirstOrDefault(x => x.Id == creatorId);
                    question.ModificationDate = question.CreationDate =
                        DateTime.Now;
                }
                
                context.Questions.Add(question);
                context.SaveChanges();
                return RedirectToAction("Details", new { id = question.Id });
            }
            return View(model);
        }
    }
}