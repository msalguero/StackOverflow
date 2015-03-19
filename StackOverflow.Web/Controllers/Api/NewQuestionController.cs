using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using StackOverflow.Data;
using StackOverflow.Domain.Entities;
using StackOverflow.Web.Models;

namespace StackOverflow.Web.Controllers.Api
{
    public class NewQuestionController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public NewQuestionController()
        {
            _unitOfWork = new UnitOfWork();
        }

        public HttpResponseMessage PostQuestion(AskQuestionModel model)
        {
            if (!this.ModelState.IsValid)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));

            Question question = Mapper.Map<AskQuestionModel, Question>(model);
            var creatorId = Guid.Parse("402d1c43-55e7-4fc7-98b6-dcd05d42f07c");
            question.Owner = _unitOfWork.AccountRepository.GetById(creatorId);
            question.ModificationDate = question.CreationDate =
                DateTime.Now;

            _unitOfWork.QuestionRepository.Add(question);
            _unitOfWork.Commit();
            
            HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.Created, model);

            return response;
        }
    }
}
