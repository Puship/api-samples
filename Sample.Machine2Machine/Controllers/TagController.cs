using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sample.Shared.DTO;
using System.Net.Http.Headers;
using System.Text;

namespace Sample.Machine2Machine.Controllers
{
    public class TagController : BaseController
    {
        public async Task<ActionResult> Index(string appCode, string deviceId)
        {
            
            var tags = await CallGetTagList(appCode, deviceId);
            return View(tags);
        }

        private async Task<List<TagFilterDTO>?> CallGetTagList(string appCode, string deviceId)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());

            StringBuilder str = new StringBuilder();
            str.AppendFormat("https://api.puship.com/api/TagFilters/GetTagFilters?appCode={0}", appCode);
            if (!string.IsNullOrEmpty(deviceId))
                str.AppendFormat("&deviceid={0}", deviceId);

            var res = await client.GetAsync(str.ToString());

            res.EnsureSuccessStatusCode();

            return await res.Content.ReadFromJsonAsync<List<TagFilterDTO>>();

        }

        private async Task<bool> CleanAllTags(string appCode, string deviceId)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());

            StringBuilder str = new StringBuilder();
            str.AppendFormat("https://api.puship.com/api/TagFilters/CleanTagFilter?appCode={0}&deviceid={1}", appCode, deviceId);



            var res = await client.DeleteAsync(str.ToString());

            res.EnsureSuccessStatusCode();

            return true;

        }

        public async Task<ActionResult> DeleteAll(string appCode, string deviceId)
        {

            await CleanAllTags(appCode, deviceId);
            return View("Index", new List<TagFilterDTO>());
        }


        public ActionResult Create(string deviceId, string appCode)
        {
            
            TagFilterDTO pm = new TagFilterDTO();
            pm.Tag = "new tag";

            return View(pm);
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string deviceId, string appCode, [Bind("Tag")] TagFilterDTO pm)
        {
            await CallPostTagFilters(deviceId, appCode, pm);

            return RedirectToAction(nameof(Index), new { deviceId = deviceId, appCode = appCode });
        }

        private async Task<ActionResult> CallPostTagFilters(string deviceId, string appCode, TagFilterDTO pm)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());

            var values = new Dictionary<string, string>
            {
                { "appCode", appCode },
                { "deviceid", deviceId},
                { "tagFilter", pm.Tag ?? "" }
            };

            string url = String.Format("https://api.puship.com/api/TagFilters/PostTagFilter?appCode={0}&deviceid={1}&tagFilter={2}", appCode, deviceId, pm.Tag);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url)
            };
            var res = await client.SendAsync(request);


            res.EnsureSuccessStatusCode();

            ViewBag.IsSuccess = true;

            return NoContent();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string deviceId, string appCode, string Tag, IFormCollection collection)
        {
            try
            {
                await DeleteDeviceApi(deviceId, appCode, Tag);
                return RedirectToAction(nameof(Index), new { deviceId = deviceId, appCode = appCode });
            }
            catch
            {
                return View();
            }
        }

        private async Task<bool> DeleteDeviceApi(string deviceId, string appCode, string Tag)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());

            var res = await client.DeleteAsync(string.Format("https://api.puship.com/api/TagFilters/RemoveTagFilter?appCode={0}&deviceid={1}&tagFilter={2}", appCode, deviceId, Tag));

            res.EnsureSuccessStatusCode();

            if (res.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

    }
}
