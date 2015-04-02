using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RestSharp.Portable;
using RestSharp.Portable.Deserializers;

namespace StackOverflow.Phone
{
    public class StackOverflowApi
    {
        private readonly RestClient _client;
        public StackOverflowApi()
        {
            //string hostName = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            _client = new RestClient
            {
                BaseUrl = new Uri("http://localhost:51894/")
            };
        }

        public IEnumerable<QuestionListModel> GetQuestions()
        {
            RestRequest request = new RestRequest {Resource = "/api/QuestionIndex"};
            var result = _client.Execute(request).Result;
            RestSharp.Portable.Deserializers.JsonDeserializer deserial = new JsonDeserializer();
            var list = deserial.Deserialize<IEnumerable<QuestionListModel>>(result);
            return list;
        }
    }
}