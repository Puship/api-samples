using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sample.Shared.DTO;
using Sample.Machine2Machine.Models;
using System.Net.Http.Headers;
using System.Text;
using Sample.Machine2Machine.Core;

namespace Sample.Machine2Machine.Controllers
{
    public class DeviceController : BaseController
    {
        // GET: AppController
        public async Task<ActionResult> Index(string id, int? pageIndex, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.UpdatedSortParm = String.IsNullOrEmpty(sortOrder) ? "Updated desc" : "";
            ViewBag.DeviceTypeSortParm = sortOrder == "DeviceType" ? "DeviceType desc" : "DeviceType";
            ViewBag.TokenSortParm = sortOrder == "Token" ? "Token desc" : "Token";
            ViewBag.DevicePlatformIdSortParm = sortOrder == "DevicePlatformId" ? "DevicePlatformId desc" : "DevicePlatformId";
            ViewBag.CreatedSortParm = sortOrder == "Created" ? "Created desc" : "Created";
            ViewBag.ExpiredSortParm = sortOrder == "Expired" ? "Expired desc" : "Expired";
            ViewBag.LanguageSortParm = sortOrder == "Language" ? "Language desc" : "Language";



            DeviceViewModel vm = new DeviceViewModel();
            vm.accessCode = id;

            int pageNumber = (pageIndex ?? 1);

            vm.pageIndex = pageNumber;
            vm.devices = await CallGetDevices(id, pageNumber, ConstInfo.pageSize, sortOrder ?? "Updated");

            return View(vm);
        }

        private async Task<PagedResponse<DeviceDTO[]>?> CallGetDevices(string appCode, int pagenumber, int pagesize, string orderby)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());

            var res = await client.GetAsync(string.Format("https://api.puship.com/api/Devices/GetDevices?AppCode={0}&pagenumber={1}&pagesize={2}&OrderBy={3}", appCode, pagenumber, pagesize, orderby));

            res.EnsureSuccessStatusCode();

            var resParsed = await res.Content.ReadFromJsonAsync<PagedResponse<DeviceDTO[]>>();

            if (resParsed?.Data != null)
            {
                // add app access token for reference
                foreach (var item in resParsed.Data)
                {
                    item.appAccessToken = appCode;
                }
                return resParsed;
            }

            return null;
        }


        private async Task<bool> DeleteDeviceApi(string DevicePlatformId)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
            var param=JsonConvert.SerializeObject(new string[1] { DevicePlatformId });

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri("https://api.puship.com/api/Devices/DeleteDevice"),
                Content = new StringContent(param, Encoding.UTF8, "application/json")
            };
            var res = await client.SendAsync(request);

            res.EnsureSuccessStatusCode();

            // var res = await client.DeleteAsync(string.Format("https://api.puship.com/api/Devices/DeleteDevice"));


            if (res.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }


        // GET: DeviceController/Delete/5
        public ActionResult Delete(string id, string accessToken)
        {
            DeviceDTO dev = new DeviceDTO();
            dev.DevicePlatformId = id;
            dev.appAccessToken = accessToken;
            return View(dev);
        }

        // POST: DeviceController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, string accessToken, IFormCollection collection)
        {
            try
            {
                await DeleteDeviceApi(id);
                return RedirectToAction(nameof(Index), new { id = accessToken });
            }
            catch
            {
                return View();
            }
        }


    }
}
