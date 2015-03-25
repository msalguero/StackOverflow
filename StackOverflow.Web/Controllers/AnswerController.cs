using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using StackOverflow.Data;
using StackOverflow.Domain.Entities;
using StackOverflow.Web.Models;

namespace StackOverflow.Web.Controllers
{
    public class AnswerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnswerController()
        {
            _unitOfWork = new UnitOfWork();
        }
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

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult VoteAnswerUp(AnswerModel item)
        {
            item.Votes += 1;
            return RedirectToAction("VoteAnswer", item);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult VoteAnswerDown(AnswerModel item)
        {
            item.Votes += -1;
            return RedirectToAction("VoteAnswer", item);
        }
        [ValidateInput(false)]
        public ActionResult VoteAnswer(AnswerModel itemModel)
        {
            Guid voterId = Guid.Parse(HttpContext.User.Identity.Name);
            Answer answer = Mapper.Map<AnswerModel, Answer>(itemModel);
            var voter = answer.Voters.FirstOrDefault(x => x.Voter.Id == voterId);
            if (voter != null)
                return RedirectToAction("Details","Question", new { id = itemModel.QuestionId });
            var question = _unitOfWork.QuestionRepository.GetById(itemModel.QuestionId);
            var account = _unitOfWork.AccountRepository.GetById(itemModel.OwnerId);
            answer.Question = question;
            answer.Owner = account;

            
            //answer.Voters.Add(new Vote() { Answer = answer, Voter = voterAccount });
            _unitOfWork.AnswerRepository.Update(answer);
            _unitOfWork.Commit();

            var voterAccount = _unitOfWork.AccountRepository.GetById(voterId);
            _unitOfWork.VoteRepository.Add(new Vote() { Answer = answer, Voter = voterAccount });
            _unitOfWork.Commit();

            Guid idQuestion = Guid.Parse(TempData["id"].ToString());
            return RedirectToAction("Details","Question", new { id = idQuestion });
        }
    }
}