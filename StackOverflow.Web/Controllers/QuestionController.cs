using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            QuestionListModel model = new QuestionListModel();
            model.OwnerName = "juan";
            model.Title = "Generating Maven GAE Endpoints library and importing it into Android Studio";
            model.Votes = 2;
            model.CreationDate = DateTime.Now;
            model.OwnerId = Guid.NewGuid();
            model.QuestionId = Guid.NewGuid();
            QuestionListModel model2= new QuestionListModel();
            model2.OwnerName = "juan";
            model2.Title = "Title test";
            model2.Votes = 13;
            model2.CreationDate = DateTime.Now;
            model2.OwnerId = Guid.NewGuid();
            model2.QuestionId = Guid.NewGuid();

            models.Add(model);
            models.Add(model2);
            return View(models);
        }
        [AllowAnonymous]
        public ActionResult Details(Guid id)
        {
            QuestionDetailsModel question = new QuestionDetailsModel();
            question.CreationDate = DateTime.Now;
            question.OwnerName = "Juan";
            question.Title = "Generating Maven GAE Endpoints library and importing it into Android Studio";
            question.Votes = 11;
            question.Description =
                "I have a Maven Google App Engine Endpoints API in Java. I compiled the JAR by following these instructions:" +
                "I recently transitioned to Android Studios from Eclipse. I'm trying to import the JAR into Android Studio. I added this line to build.gradle:" +
                "What other dependencies do I have to add to build.gradle? I can't seem to find a list anywhere";
            return View(question);
        }

        public ActionResult Ask()
        {
            return View(new AskQuestionModel());
        }

        [HttpPost]
        public ActionResult Ask(AskQuestionModel model)
        {
            return View(new AskQuestionModel());
        }
    }
}