using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StackOverflow.Data;
using StackOverflow.Domain.Entities;

namespace StackOverflow.Web.Controllers.Api
{
    public class QuestionDetailController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public QuestionDetailController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public string Get(Guid id)
        {
            Question question = _unitOfWork.QuestionRepository.GetById(id);
            return question.ToString();
        }
    }
}
