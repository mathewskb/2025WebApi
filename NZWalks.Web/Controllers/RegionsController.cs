using Microsoft.AspNetCore.Mvc;
using NZWalks.Web.Models;
using NZWalks.Web.Models.DTOs;
using System.Text;
using System.Text.Json;

namespace NZWalks.Web.Controllers
{
    public class RegionsController(HttpClient httpClient, IConfiguration config) : Controller
    {

        public async Task<IActionResult> Index()
        {
            List<RegionDto> regions = new List<RegionDto>();

            try
            {
                var httpresponsemessage = await httpClient.GetAsync(config["WebAPIUrl"]?.ToString() + "api/regions");

                httpresponsemessage.EnsureSuccessStatusCode();

                // var response = await httpresponsemessage.Content.ReadAsStringAsync();
                
                regions.AddRange(await httpresponsemessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());

                // ViewBag.Response = response;

            }
            catch (Exception ex)
            {

                throw;
            }
            return View(regions);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddRegionViewModel model)
        {

            var httprequestmessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(config["WebAPIUrl"]?.ToString() + "api/regions"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
            };

            var httpresponsemessage = await httpClient.SendAsync(httprequestmessage);
            httpresponsemessage.EnsureSuccessStatusCode();

            var response = httpresponsemessage.Content.ReadFromJsonAsync<RegionDto>();

            if(response != null)
            {
                return RedirectToAction("Index","Regions");
            }

            return View();
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            ViewBag.Id = id;

            //var httprequestmessage = new HttpRequestMessage()
            //{
            //    Method = HttpMethod.Get,
            //    RequestUri = new Uri(config["WebAPIUrl"]?.ToString() + $"api/regions/{id}")
            //};

            var regionDto = await httpClient.GetFromJsonAsync<RegionDto>(config["WebAPIUrl"]?.ToString() + $"api/regions/{id}");
            if(regionDto != null)
            {
                return View(regionDto);
            }

            return View(null);
        }
    }
}
