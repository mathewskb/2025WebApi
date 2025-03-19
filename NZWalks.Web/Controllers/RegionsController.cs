using Microsoft.AspNetCore.Mvc;
using NZWalks.Web.Models.DTOs;

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
                // var response = await httpresponsemessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>();
                regions.AddRange(await httpresponsemessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());

                // ViewBag.Response = response;

            }
            catch (Exception ex)
            {

                throw;
            }
            return View(regions);
        }
    }
}
