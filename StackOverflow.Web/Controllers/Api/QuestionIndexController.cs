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
    public class QuestionIndexController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public QuestionIndexController()
        {
            _unitOfWork = new UnitOfWork();
        }

        public IEnumerable<QuestionListModel> Get()
        {
            List<QuestionListModel> models = new List<QuestionListModel>();
            var questions = _unitOfWork.QuestionRepository.GetList();
            foreach (var item in questions)
            {
                var model = Mapper.Map<Question, QuestionListModel>(item);
                models.Add(model);
            }
            return models;
        }
    }
}
