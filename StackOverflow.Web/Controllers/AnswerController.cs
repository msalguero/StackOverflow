using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackOverflow.Data;
using StackOverflow.Domain.Entities;

namespace StackOverflow.Web.Controllers
{
    public class AnswerController : Controller
    {
        // GET: Answer
        //[HttpPost]
        //public ActionResult CreateAnswer(string description)
        //{
        //    var context = new StackOverflowContext();
        //    var question = context.Questions.FirstOrDefault(question => question.Id = ViewBag.CurrentQuestion);
        //    var model = new Answer { Description = description, CreationDate = DateTime.Now };


        //    return RedirectToAction("Index");
        //    //return RedirectToAction("Details", new {id = model.QuestionId});
        //}
    }
}