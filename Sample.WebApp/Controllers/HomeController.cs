using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Sample.Shared.DTO;
using Sample.WebApp.Models;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace Sample.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("~/GetApps")]
        public async Task<ActionResult> GetApps(CancellationToken cancellationToken)
        {
            var apps = await CallApi(1, 10);

            return View("Index", model: (apps?.TotalRecords ?? 0).ToString());
        }

        private async Task<PagedResponse<AppDTO[]>?> CallApi(int pagenumber, int pagesize)
        {
            var token = await HttpContext.GetTokenAsync(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectParameterNames.AccessToken);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var res = await client.GetAsync(string.Format("https://api.puship.com/api/Apps/GetApps?pagenumber={0}&pagesize={1}", pagenumber, pagesize));

            var resParsed = await res.Content.ReadFromJsonAsync<PagedResponse<AppDTO[]>>();

            if (resParsed?.Data != null)
                return resParsed;

            return null;
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}