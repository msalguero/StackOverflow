using System;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Mvc;
using StackOverflow.Data;
using StackOverflow.Domain.Entities;
using StackOverflow.Web.Models;

namespace StackOverflow.Web.Controllers.Api
{
    public class QuestionDetailController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public QuestionDetailController()
        {
            _unitOfWork = new UnitOfWork();
        }

        public QuestionDetailsModel Get(Guid id)
        {
            Question question = _unitOfWork.QuestionRepository.GetById(id);
            
            return AutoMapper.Mapper.Map<Question,QuestionDetailsModel>(question);
        }

        
    }
}
