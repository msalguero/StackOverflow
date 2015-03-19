using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StackOverflow.Data;
using StackOverflow.Web.Models;

namespace StackOverflow.Web.Controllers.Api
{
    public class AnswerQuestionController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnswerQuestionController()
        {
            _unitOfWork = new UnitOfWork();
        }

        public HttpResponseMessage Post(AnswerModel model)
        {
            HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.Created, model);

            return response;
        }
    }
}
