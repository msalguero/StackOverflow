using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using RestSharp.Portable;
using RestSharp.Portable.Deserializers;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace StackOverflow.Phone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class QuestionDetail : Page
    {
        private QuestionDetailsModel _question;
        public QuestionDetail()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null) 
                GetQuestion(Guid.Parse(e.Parameter.ToString()));
        }

        private async void GetQuestion(Guid id)
        {
            RestClient client = new RestClient
            {
                BaseUrl = new Uri("http://localhost:51894/")
            };
            RestRequest request = new RestRequest { Resource = "/api/QuestionDetail/"+id.ToString() };
            var response = await client.Execute(request);
            RestSharp.Portable.Deserializers.JsonDeserializer deserial = new JsonDeserializer();
            _question = deserial.Deserialize<QuestionDetailsModel>(response);
            Title.Text = _question.Title;
            Description.Text = _question.Description;
            Owner.Text = _question.OwnerName;
            foreach (var answer in _question.Answers)
            {
                AnswerList.Items.Add(answer.Description);
            }
        }

    }
}
