using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using StackOverflow.Data;
using StackOverflow.Domain.Entities;
using StackOverflow.Web.Models;

namespace StackOverflow.Web.Controllers.Api
{
    public class RegisterController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegisterController()
        {
            _unitOfWork = new UnitOfWork();
        }

        public HttpResponseMessage PostRegister(AccountRegisterModel model)
        {
            bool modelIsValid = model != null && !model.Email.IsNullOrWhiteSpace() && !model.Name.IsNullOrWhiteSpace()
                                && !model.Password.IsNullOrWhiteSpace();
            if (modelIsValid && model.ConfirmPassword == model.Password)
            {
                Account newAccount = Mapper.Map<AccountRegisterModel, Account>(model);
                _unitOfWork.AccountRepository.Add(newAccount);
                _unitOfWork.Commit();

                HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.Created, model);

                return response;
            }
            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
        }
    }
}