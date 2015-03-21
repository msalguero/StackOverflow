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
    public class CommentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentController()
        {
            _unitOfWork = new UnitOfWork();
        }
        // GET: Comment
        public ActionResult CommentIndex(ICollection<CommentListModel> models, string entity)
        {
            @ViewBag.Entity = entity;
            return PartialView(models);
        }
        [Authorize]
        public ActionResult CreateComment(string comment, string AnswerId)
        {
            Guid idQuestion = Guid.Parse(TempData["id"].ToString());
            var question = _unitOfWork.QuestionRepository.GetById(idQuestion);
            Guid creatorId = Guid.Parse(HttpContext.User.Identity.Name);
            var account = _unitOfWork.AccountRepository.GetById(creatorId);

            var newComment = new Comment { Owner = account, Description = comment};
            if (AnswerId != "Question")
            {
                Guid idAnswer = Guid.Parse(AnswerId);
                newComment.Answer = _unitOfWork.AnswerRepository.GetById(idAnswer);
            }
            else
            {
                newComment.Question = question;
            }
            _unitOfWork.CommentRepository.Add(newComment);
            _unitOfWork.Commit();
            
            return RedirectToAction("Details","Question", new { id = idQuestion });
        }
    }
}