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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace StackOverflow.Phone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private IEnumerable<QuestionListModel> _list;
        public MainPage()
        {
            this.InitializeComponent();
            
            this.NavigationCacheMode = NavigationCacheMode.Required;
            
            GetList();
        }

        private async void GetList()
        {
            RestClient client = new RestClient
            {
                BaseUrl = new Uri("http://localhost:51894/")
            };
            RestRequest request = new RestRequest { Resource = "/api/QuestionIndex" };
            var response = await client.Execute(request);
            RestSharp.Portable.Deserializers.JsonDeserializer deserial = new JsonDeserializer();
            _list = deserial.Deserialize<IEnumerable<QuestionListModel>>(response);

            foreach (var question in _list)
            {
                if (QuestionList.Items != null)
                    QuestionList.Items.Add(question.Title+" "+"asked by "+question.OwnerName+" on "+question.CreationDate);
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.
            
            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void QuestionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (QuestionList.Items != null)
            {
                var questionPos = QuestionList.Items.IndexOf(e.AddedItems.First());
                var questionId = _list.ElementAt(questionPos).QuestionId;
                this.Frame.Navigate(typeof(QuestionDetail), questionId);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Register));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (Login));
        }
    }
}
