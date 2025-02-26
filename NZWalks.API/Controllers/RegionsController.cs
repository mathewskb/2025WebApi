using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using System.Net;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController(NZWalksDbContext nZWalksDbContext) : ControllerBase
    {

        [HttpGet]
        public IActionResult GetAll()
        {
            //var regions = new List<Region> {
            //    new Region { Id = Guid.NewGuid(), Name="Auckland Region", Code ="AKL",RegionImageUrl="https://pixabay.com/photos/lentils-food-beans-healthy-7772450/" },
            //    new Region { Id = Guid.NewGuid(), Name="Wellington Region", Code ="WLG",RegionImageUrl="https://pixabay.com/photos/building-architecture-facade-5676506/" },
            //    new Region { Id = Guid.NewGuid(), Name="Kottarakara", Code ="KTR",RegionImageUrl="https://pixabay.com/photos/automobile-house-orange-road-5128760/" }
            //};

            // GET data from database - Domain Model
            var regionsDomain = nZWalksDbContext.Regions.ToList();

            // Convert To DTO
            var regionsDto = new List<RegionDto>();

            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = regionDomain.Id,
                    Name = regionDomain.Name,
                    Code = regionDomain.Code,
                    RegionImageUrl = regionDomain.RegionImageUrl
                });
            }


            if (regionsDto == null) return NotFound();

            // returning DTOs to Client
            return Ok(regionsDto);
        }
        [HttpGet("{id:guid}")]
        public IActionResult GetById(Guid id)
        {
            var regionDomain = nZWalksDbContext.Regions.Find(id);

            if (regionDomain == null) return NotFound();

            var regionDto = new RegionDto()
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            // returning DTOs to Client
            return Ok(regionDto);
        }

        [HttpPost]
        public IActionResult Create(AddRegionRequestDto addRegionRequestDto)
        {

            // convert DTO to Domain model
            var regionDomain = new Region
            {
                // Id = Guid.NewGuid(), // no need to pass as this one is ID (Entity framework will add by default
                Code = addRegionRequestDto.Code,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl,
                Name = addRegionRequestDto.Name,
            };

            nZWalksDbContext.Regions.Add(regionDomain);
            nZWalksDbContext.SaveChanges();

            // map back to dto
            var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetById), new { id = regionDomain.Id }, regionDto);

        }

        [HttpPut("{id:guid}")] // https://localhost:6001/api/Regions/943cd20b-1537-4e3e-aee4-66082bd99150
        public IActionResult Update(Guid id, UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionDomain = nZWalksDbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (regionDomain == null) return NotFound();

            regionDomain.Name = updateRegionRequestDto.Name;
            regionDomain.Code = updateRegionRequestDto.Code;
            regionDomain.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            nZWalksDbContext.SaveChanges();

            // now retun back region dto to client

            var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            return Ok(regionDto);
        }


        [HttpDelete("{id:guid}")] // https://localhost:6001/api/Regions/943cd20b-1537-4e3e-aee4-66082bd99150
        public IActionResult Delete(Guid id)
        {
            var region = nZWalksDbContext.Regions.FirstOrDefault(y => y.Id == id);

            if (region == null) return NotFound();

            nZWalksDbContext.Regions.Remove(region);
            nZWalksDbContext.SaveChanges();

            // if you want, you can return deleted item by converting from domain to dto -- region to RegionDto
            return Ok();


        }

    }
}
