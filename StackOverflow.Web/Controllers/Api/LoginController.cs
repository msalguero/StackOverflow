using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using StackOverflow.Data;
using StackOverflow.Domain.Entities;
using StackOverflow.Web.Models;

namespace StackOverflow.Web.Controllers.Api
{
    public class LoginController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoginController()
        {
            _unitOfWork = new UnitOfWork();
        }

        public HttpResponseMessage PostLogin(AccountLoginModel model)
        {
            if (!model.Email.IsNullOrWhiteSpace() && !model.Password.IsNullOrWhiteSpace())
            {
                Account account = _unitOfWork.AccountRepository.GetWithFilter(x => x.Email == model.Email && x.Password == model.Password);
                if (account != null)
                {
                    HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.Created, model);

                    return response;
                }
            }
            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
        }
    }
}
