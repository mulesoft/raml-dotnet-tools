using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace AMF.Tools
{
    public class Tracking
    {
        private const string ApiKey = "7856322d613393b800727439f0954eb9";
        public static void Init()
        {
            // production settings this is your project's write key
            //Analytics.Initialize("5aee4965dc93cb3816250ce65e6a9868");
            // dev settings
            //Analytics.Initialize("5aee4965dc93cb3816250ce65e6a9868", new Config().SetAsync(false));
            //Analytics.Client.Identify("0", new Dictionary<string, object>());

            var url = "https://api.amplitude.com/httpapi";
            var eventData = "{ " +
                "user_id: \"" + UserInfo.Get() + "\", " +
                "event_type: \"$identify\" " +
            " }";

            //    {"user_id":"datamonster@gmail.com",
            //"event_type":, "user_properties":{"$set": {"Cohort":"Test B"},
            //"$add": {"friendCount":3}}, "country":"United States", "ip":"127.0.0.1",
            //"time":1396381378123}]
            var parameters = new Dictionary<string, string> {
                        { "api_key", ApiKey },
                        { "event", eventData }
                    };


            var encodedContent = new FormUrlEncodedContent(parameters);
            var client = new HttpClient();
            try
            {
                var response = client.PostAsync(url, encodedContent).ConfigureAwait(false).GetAwaiter().GetResult();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // Do something with response. Example get content:
                    var responseContent = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                }
                else
                {
                    var a = response.StatusCode;
                    var responseContent = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                }
            }
            // this is on purpose, if internet fails the tools should continue to work
            catch (HttpRequestException) { }
            catch (WebException) { }
        }

        public static void Track(string eventDescription)
        {
            //Analytics.Client.Track(userId, eventDescription);

            var url = "https://api.amplitude.com/httpapi";
            var eventData = "{ " +
                "user_id: \"" + UserInfo.Get() + "\", " +
                "event_type: \".Net API Tools - " + eventDescription + "\", " +
                "country: \"" + System.Globalization.RegionInfo.CurrentRegion.EnglishName + "\", " +
                "platform: \"" + UserInfo.GetPlatform() + "\" " +
            " }";

            //    {"user_id":"datamonster@gmail.com",
            //"event_type":, "user_properties":{"$set": {"Cohort":"Test B"},
            //"$add": {"friendCount":3}}, "country":"United States", "ip":"127.0.0.1",
            //"time":1396381378123}]

            var parameters = new Dictionary<string, string> {
                        { "api_key", ApiKey },
                        { "event", eventData }
                    };


            var encodedContent = new FormUrlEncodedContent(parameters);
            var client = new HttpClient();
            try
            {
                var response = client.PostAsync(url, encodedContent).ConfigureAwait(false).GetAwaiter().GetResult();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var statusCode = response.StatusCode;
                    var responseContent = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                }
            }
            // this is on purpose, if internet fails the tools should continue to work
            catch (HttpRequestException) { }
            catch (WebException) { }
        }

    }
}
