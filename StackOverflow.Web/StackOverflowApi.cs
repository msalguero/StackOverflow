using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using RestSharp;
using StackOverflow.Web.Models;

namespace StackOverflow.Web
{
    public class StackOverflowApi
    {
        private readonly RestClient _client;
        public StackOverflowApi()
        {
            //string hostName = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            _client = new RestClient
            {
                BaseUrl = "http://localhost:51894/"
            };
        }

        public IEnumerable<QuestionListModel> GetQuestions()
        {
            RestRequest request = new RestRequest {Resource = "/api/QuestionIndex"};
            var list = JsonConvert.DeserializeObject<IEnumerable<QuestionListModel>>(_client.Execute(request).Content);
            return list;
        }
    }
}