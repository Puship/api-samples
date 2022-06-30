using Microsoft.AspNetCore.Mvc;
using Sample.Machine2Machine.Core;
using Sample.Shared.DTO;
using Sample.Machine2Machine.Models;
using System.Diagnostics;

namespace Sample.Machine2Machine.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel();
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(ConstInfo._SessionKeyName)))
            {
                homeViewModel.isLogged = true;
                homeViewModel.token = HttpContext.Session.GetString(ConstInfo._SessionKeyName);
            }

            return View(homeViewModel);
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

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginMachine2Machine([Bind("client_id,client_secret")] HomeViewModel home)
        {

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString(ConstInfo._SessionKeyName)))
                {
                    string tk = await GetToken(home.client_id ?? "", home.client_secret ?? "");
                    HttpContext.Session.SetString(ConstInfo._SessionKeyName, tk);
                }
                return RedirectToAction(nameof(Index));
            } else
            {
                return View("Index",home);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {

            HttpContext.Session.Remove(ConstInfo._SessionKeyName);

            return RedirectToAction(nameof(Index));
        }

        private async Task<string> GetToken(string client_id, string client_secret)
        {
            string token_endpoint = "https://auth.puship.com/connect/token";
            var values = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", client_id},
                { "client_secret", client_secret }
            };

            HttpClient tokenClient = new HttpClient();
            var content = new FormUrlEncodedContent(values);
             var response = await tokenClient.PostAsync(token_endpoint, content);

            var res = await response.Content.ReadFromJsonAsync<TokenResponse>();

            return res == null ? "" : res.access_token;
        }
    }
}