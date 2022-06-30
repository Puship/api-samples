using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sample.Shared.DTO;
using Sample.Machine2Machine.Models;
using System.Net.Http.Headers;
using PagedList;
using Sample.Machine2Machine.Core;
using System.Text;

namespace Sample.Machine2Machine.Controllers
{
    public class AppController : BaseController
    {
        // GET: AppController
        public async Task<ActionResult> Index(int? pageIndex, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name desc" : "";
            ViewBag.CreatedSortParm = sortOrder == "created" ? "created desc" : "created";
            ViewBag.UpdatedSortParm = sortOrder == "updated" ? "updated desc" : "updated";
            ViewBag.AccessCodeSortParm = sortOrder == "accesscode" ? "accesscode desc" : "accesscode";
            ViewBag.EnableAPNSSortParm = sortOrder == "enableapns" ? "enableapns desc" : "enableapns";
            ViewBag.EnableGCMSortParm = sortOrder == "enablegcm" ? "enablegcm desc" : "enablegcm";
            ViewBag.EnableMPNSSortParm = sortOrder == "enablempns" ? "enablempns desc" : "enablempns";
            ViewBag.EnableBPNSSortParm = sortOrder == "enablebpns" ? "enablebpns desc" : "enablebpns";
            ViewBag.DevelopmentSortParm = sortOrder == "development" ? "development desc" : "development";



            AppViewModel vm = new AppViewModel();

            int pageNumber = (pageIndex ?? 1);

            vm.pageIndex = pageNumber;
            vm.apps = await CallGetApps(pageNumber, ConstInfo.pageSize, sortOrder ?? "Name");


            return View(vm);
        }

        private string Compact(string[] pars)
        {
            return string.Join(",",pars.Where(w => !string.IsNullOrEmpty(w)));
        }


        private async Task<PagedResponse<AppDTO[]>?> CallGetApps(int pagenumber, int pagesize, string orderby)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());

            var res = await client.GetAsync(string.Format("https://api.puship.com/api/Apps/GetApps?pagenumber={0}&pagesize={1}&OrderBy={2}", pagenumber, pagesize, orderby));

            res.EnsureSuccessStatusCode();

            var resParsed = await res.Content.ReadFromJsonAsync<PagedResponse<AppDTO[]>>();

            if (resParsed?.Data != null)
                return resParsed;

            return null;
        }

    }
}
