using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace AMF.Common
{
    public static class Downloader
    {
        public static string GetContents(Uri uri, string username = null, string password = null)
        {
            var client = new HttpClient();
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            }
            var downloadTask = client.GetStringAsync(uri);
            downloadTask.WaitWithPumping();
            return downloadTask.Result;
        }
    }
}