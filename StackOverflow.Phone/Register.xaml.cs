using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace StackOverflow.Phone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Register : Page
    {
        public Register()
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
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            AccountRegisterModel newAccount = new AccountRegisterModel
            {
                Name = Name.Text,
                Password = Password.Password,
                ConfirmPassword = ConfirmPassword.Password,
                Email = Email.Text,
                LastName = LastName.Text
            };

            RestClient client = new RestClient
            {
                BaseUrl = new Uri("http://localhost:51894/")
            };
            RestRequest request = new RestRequest { Resource = "/api/Register" , Method = HttpMethod.Post};
            
            request.AddJsonBody(newAccount);
            var response = await client.Execute(request);
        }

    }
}
