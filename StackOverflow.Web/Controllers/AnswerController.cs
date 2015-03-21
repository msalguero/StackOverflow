using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackOverflow.Data;
using StackOverflow.Domain.Entities;
using StackOverflow.Web.Models;

namespace StackOverflow.Web.Controllers
{
    public class AnswerController : Controller
    {
        // GET: Answer
        public ActionResult AnswerIndex(IEnumerable<AnswerModel> models)
        {
            var md = new MarkdownDeep.Markdown();
            md.ExtraMode = true;
            md.SafeMode = false;
            foreach (var model in models)
            {
                model.Description = md.Transform(model.Description);
            }
            return PartialView(models);
        }
    }
}