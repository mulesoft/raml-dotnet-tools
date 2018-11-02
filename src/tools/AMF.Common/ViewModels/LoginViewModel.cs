using System.Windows;
using Caliburn.Micro;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Security;
using System;
using System.Runtime.InteropServices;

namespace AMF.Common.ViewModels
{
    public class LoginViewModel : Screen
    {
        static HttpClient client = new HttpClient();
        private static readonly string BaseUrl = "https://anypoint.mulesoft.com";

        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                NotifyOfPropertyChange();
            }
        }

        public string Token { get; internal set; }

        public async void Login(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                MessageBox.Show("Please type your username.");
                return;
            }
            var password = string.Empty;
            if (parameter is IHavePassword passwordContainer)
            {
                var secureString = passwordContainer.Password;
                password = ConvertToUnsecureString(secureString);
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please type your password.");
                return;
            }

            var json = "{ \"username\" : \"" + Username  + "\", \"password\" : \"" + password + "\" }";
            var loginUrl = BaseUrl + "/accounts/login";
            var response = await client.PostAsync(loginUrl, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            if(!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    message = "Invalid username or password";

                MessageBox.Show("Error when trying to login: " + message);
                return;
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var tokenObj = new JavaScriptSerializer().Deserialize<TokenResponse>(jsonResponse);
            Token = tokenObj.Access_Token;

            TryClose();
        }

        public void CancelButton()
        {
            Token = null;
            TryClose();
        }

        private string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
            {
                return string.Empty;
            }

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        private class TokenResponse
        {
            public string Access_Token { get; set; }
            public string Token_Type { get; set; }
            public string RedirectUrl { get; set; }
        }

    }

    public interface IHavePassword
    {
        System.Security.SecureString Password { get; }
    }


}