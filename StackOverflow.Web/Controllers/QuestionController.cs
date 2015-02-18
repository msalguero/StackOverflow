using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackOverflow.Web.Models;

namespace StackOverflow.Web.Controllers
{
    public class QuestionController : Controller
    {
        // GET: Question
        public ActionResult Index()
        {
            List<QuestionListModel> models = new List<QuestionListModel>();
            QuestionListModel model = new QuestionListModel();
            model.OwnerName = "juan";
            model.Title = "Title test";
            model.Votes = 2;
            model.CreationDate = DateTime.Now;
            model.OwnerId = Guid.NewGuid();
            model.QuestionId = Guid.NewGuid();
            QuestionListModel model2= new QuestionListModel();
            model2.OwnerName = "juan";
            model2.Title = "Title test";
            model2.Votes = 2;
            model2.CreationDate = DateTime.Now;
            model2.OwnerId = Guid.NewGuid();
            model2.QuestionId = Guid.NewGuid();

            models.Add(model);
            models.Add(model2);
            return View(models);
        }

        public ActionResult Details(Guid id)
        {
            return View(new QuestionDetailsModel());
        }

        public ActionResult Ask()
        {
            return View(new AskQuestionModel());
        }
    }
}