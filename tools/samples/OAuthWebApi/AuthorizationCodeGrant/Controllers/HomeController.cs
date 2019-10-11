using DotNetOpenAuth.OAuth2;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AuthorizationCodeGrant.Controllers
{
    public class HomeController : Controller
    {
        private WebServerClient _webServerClient;

        public async Task<ActionResult> Index()
        {
            ViewBag.AccessToken = Request.Form["AccessToken"] ?? "";
            ViewBag.RefreshToken = Request.Form["RefreshToken"] ?? "";
            ViewBag.Action = "";
            ViewBag.ApiResponse = "";

            InitializeWebServerClient();
            var accessToken = Request.Form["AccessToken"];
            if (string.IsNullOrEmpty(accessToken))
            {
                var authorizationState = _webServerClient.ProcessUserAuthorization(Request);
                if (authorizationState != null)
                {
                    ViewBag.AccessToken = authorizationState.AccessToken;
                    ViewBag.RefreshToken = authorizationState.RefreshToken;
                    ViewBag.Action = Request.Path;
                }
            }

            if (!string.IsNullOrEmpty(Request.Form.Get("submit.Authorize")))
            {
                var userAuthorization = _webServerClient.PrepareRequestUserAuthorization(new[] { "bio", "notes" });
                userAuthorization.Send(HttpContext);
                Response.End();
            }
            else if (!string.IsNullOrEmpty(Request.Form.Get("submit.Refresh")))
            {
                var state = new AuthorizationState
                {
                    AccessToken = Request.Form["AccessToken"],
                    RefreshToken = Request.Form["RefreshToken"]
                };
                if (_webServerClient.RefreshAuthorization(state))
                {
                    ViewBag.AccessToken = state.AccessToken;
                    ViewBag.RefreshToken = state.RefreshToken;
                }
            }
            else if (!string.IsNullOrEmpty(Request.Form.Get("submit.CallApi")))
            {
                Raml.RamlApi api = new Raml.RamlApi("http://localhost:11625");
                api.OAuthAccessToken = accessToken;

                //api.AddDefaultRequestHeader("Authorization", "Bearer " + accessToken);

                var response = await api.ApiMe.Get();
                var body = await response.RawContent.ReadAsStringAsync();
                                
                ViewBag.ApiResponse = body;
            }

            return View();
        }

        private void InitializeWebServerClient()
        {
            var authorizationServerUri = new Uri("http://localhost:11625");
            var authorizationServer = new AuthorizationServerDescription
            {
                AuthorizationEndpoint = new Uri(authorizationServerUri, "/OAuth/Authorize"),
                TokenEndpoint = new Uri(authorizationServerUri, "/OAuth/Token")
            };
            _webServerClient = new WebServerClient(authorizationServer, "123456", "abcdef");
        }
    }
}