using Microsoft.VisualStudio.Threading;
using System;
using System.Net.Http;

namespace AMF.Common
{
    public static class Downloader
    {
        public static string GetContents(Uri uri)
        {
            var client = new HttpClient();
            var downloadTask = client.GetStringAsync(uri);
            downloadTask.WaitWithPumping();
            return downloadTask.Result;
        }
    }
}