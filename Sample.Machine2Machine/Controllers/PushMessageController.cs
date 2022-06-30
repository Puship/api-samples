using Microsoft.AspNetCore.Mvc;
using Sample.Shared.DTO;
using Sample.Machine2Machine.Models;
using System.Net.Http.Headers;
using Sample.Shared.Filters;
using Newtonsoft.Json;
using System.Text;
using Sample.Machine2Machine.Core;

namespace Sample.Machine2Machine.Controllers
{
    public class PushMessageController : BaseController
    {
        public async Task<ActionResult> Index(string? id,string accessToken, int? pageIndex, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CreatedSortParm = String.IsNullOrEmpty(sortOrder) ? "Created desc" : "";
            ViewBag.PushMessageIdSortParm = sortOrder == "PushMessageId" ? "PushMessageId desc" : "PushMessageId";
            ViewBag.MessageSortParm = sortOrder == "Message" ? "Message desc" : "Message";
            ViewBag.BadgeSortParm = sortOrder == "Badge" ? "Badge desc" : "Badge";
            ViewBag.SoundSortParm = sortOrder == "Sound" ? "Sound desc" : "Sound";
            ViewBag.SendApnsSortParm = sortOrder == "SendApns" ? "ExpiSendApnsred desc" : "SendApns";
            ViewBag.SendC2dmSortParm = sortOrder == "SendC2dm" ? "SendC2dm desc" : "SendC2dm";
            ViewBag.SendMpnsSortParm = sortOrder == "SendMpns" ? "SendMpns desc" : "SendMpns";
            ViewBag.SendBpnsSortParm = sortOrder == "SendBpns" ? "SendBpns desc" : "SendBpns";
            ViewBag.SendPushSortParm = sortOrder == "SendPush" ? "SendPush desc" : "SendPush";

            PushMessageViewModel vm = new PushMessageViewModel();
            vm.deviceId = id;
            vm.accessCode = accessToken;

            int pageNumber = (pageIndex ?? 1);

            vm.pageIndex = pageNumber;
            if (id == null)
                vm.pushMessage = await CallGetPushMessageByAppApi(accessToken, pageNumber, ConstInfo.pageSize, sortOrder ?? "Created");
            else
                vm.pushMessage = await CallGetPushMessageByDeviceApi(id, accessToken, pageNumber, ConstInfo.pageSize, sortOrder ?? "Created");



            return View(vm);
        }

        private async Task<PagedResponse<PushMessageDTO[]>?> CallGetPushMessageByAppApi(string accessToken, int pagenumber, int pagesize, string orderby)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());

            var res = await client.GetAsync(string.Format("https://api.puship.com/api/PushMessages/GetPushMessages?AppCode={0}&PageNumber={1}&PageSize={2}&OrderBy={3}", accessToken, pagenumber, pagesize, orderby));

            res.EnsureSuccessStatusCode();

            var resParsed = await res.Content.ReadFromJsonAsync<PagedResponse<PushMessageDTO[]>>();


            if (resParsed?.Data != null)
            {
                return resParsed;
            }

            return null;
        }

        private async Task<PagedResponse<PushMessageDTO[]>?> CallGetPushMessageByDeviceApi(string deviceId, string accessToken, int pagenumber, int pagesize, string orderby)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());

            var res = await client.GetAsync(string.Format("https://api.puship.com/api/PushMessages/GetPushMessagesByDevice?DeviceId={0}&AppCode={1}&pagenumber={2}&pagesize={3}&OrderBy={4}", deviceId, accessToken, pagenumber, pagesize, orderby));

            res.EnsureSuccessStatusCode();

            var resParsed = await res.Content.ReadFromJsonAsync<PagedResponse<PushMessageDTO[]>>();


            if (resParsed?.Data != null)
            {
                return resParsed;
            }

            return null;
        }

        private async Task<PushMessageDTO?> CallGetPushMessageDetail(string pushMessageId)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());

            var res = await client.GetAsync(string.Format("https://api.puship.com/api/PushMessages/GetPushMessage/{0}", pushMessageId));

            res.EnsureSuccessStatusCode();

            return await res.Content.ReadFromJsonAsync<PushMessageDTO>();

        }

        public async Task<IActionResult> Detail(string id)
        {
            PushMessageDTO? homeViewModel = await CallGetPushMessageDetail(id);

            if (homeViewModel != null)
                return View(homeViewModel);
            else
                return View();
        }

        public ActionResult SendPushMessageByDevice(string id, string accessToken)
        {
            PushMessageByDevice pm = new PushMessageByDevice();
            pm.AppCode = accessToken;
            pm.FirstDeviceId = id;
            pm.Message = "example message";
            pm.Badge = 1;
            pm.Sound = "Default";
            pm.SendPush = true;

            return View(pm);
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendPushMessageByDevice([Bind("AppCode,FirstDeviceId,Message,Badge,Sound,SendPush,Param1,Param2,Param3,Param4,Param5")] PushMessageByDevice pm)
        {
            await CallSendPushMessageByDevice(pm);

            return View(pm);
        }

        private async Task<ActionResult> CallSendPushMessageByDevice(PushMessageByDevice pm)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());

            var param = JsonConvert.SerializeObject(pm);

            var res = await client.PostAsync("https://api.puship.com/api/PushMessages/SendPushMessageByDevice", new StringContent(param, Encoding.UTF8, "application/json"));

            res.EnsureSuccessStatusCode();
            
            ViewBag.IsSuccess = true;

            return NoContent();
        }


        public ActionResult SendPushMessageByPlatform(string appCode)
        {
            PushMessageByPlatform pm = new PushMessageByPlatform();
            pm.AppCode = appCode;
            pm.Message = "example message";
            pm.Badge = 1;
            pm.Sound = "Default";
            pm.SendPush = true;

            return View(pm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendPushMessageByPlatform([Bind("SendApns,SendC2dm,SendMpns,SendBpns,P1Latitude,P1Longitude,P2Latitude,P2Longitude,LastPositionDate,LastPositionNumber,AppCode,Message,Badge,Sound,SendPush,Param1,Param2,Param3,Param4,Param5")] PushMessageByPlatform pm)
        {
            await CallSendPushMessageByPlatform(pm);

            return View(pm);
        }

        private async Task<ActionResult> CallSendPushMessageByPlatform(PushMessageByPlatform pm)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());

            var param = JsonConvert.SerializeObject(pm);

            var res = await client.PostAsync("https://api.puship.com/api/PushMessages/SendPushMessage", new StringContent(param, Encoding.UTF8, "application/json"));

            res.EnsureSuccessStatusCode();

            ViewBag.IsSuccess = true;

            return NoContent();
        }


        private async Task<bool> DeletePushApi(string PushMessageId)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
 
            var res = await client.DeleteAsync(String.Format("https://api.puship.com/api/PushMessages/DeletePushMessage/{0}", PushMessageId));

            res.EnsureSuccessStatusCode();

            if (res.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }


        // GET: PushMessageController/Delete/5
        public ActionResult Delete(string id)
        {
            PushMessageDTO dev = new PushMessageDTO();
            dev.PushMessageId = id;
            return View(dev);
        }

        // POST: DeviceController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, IFormCollection collection)
        {
            try
            {
                await DeletePushApi(id);
                ViewBag.IsSuccess = true;
                return View();
            }
            catch
            {
                return View();
            }
        }

    }
}
