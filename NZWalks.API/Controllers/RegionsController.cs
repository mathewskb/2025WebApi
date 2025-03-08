using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Interfaces;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Models.DTOs.Domain;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController(IRegionRepository regionRepository,
        IMapper mapper, ILogger<RegionsController> logger1) : ControllerBase
    {

        [HttpGet]
        // logging check
        // [Authorize(Roles = "Reader")]
        public async Task<ActionResult> GetAll()
        {
            // throw new Exception("Mathews Error");

            logger1.LogInformation("Get All Method started");

            var regionsDomain = await regionRepository.GetAllAsync();

            logger1.LogInformation($"Get All Method completed - {JsonSerializer.Serialize(regionsDomain)}");

            return Ok(mapper.Map<IEnumerable<RegionDto>>(regionsDomain));

        }
        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<ActionResult<RegionDto>> GetById(Guid id)
        {
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null) return NotFound();

            return Ok(mapper.Map<RegionDto>(regionDomain));
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<ActionResult<RegionDto>> Create(AddRegionRequestDto addRegionRequestDto)
        {

            var regionDomain = mapper.Map<Region>(addRegionRequestDto);


            await regionRepository.CreateAsync(regionDomain);

            // map back to dto
            var regionDto = mapper.Map<RegionDto>(regionDomain);


            return CreatedAtAction(nameof(GetById), new { id = regionDomain.Id }, regionDto);

        }

        [HttpPut("{id:guid}")] // https://localhost:6001/api/Regions/943cd20b-1537-4e3e-aee4-66082bd99150
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<ActionResult<RegionDto>> Update(Guid id, UpdateRegionRequestDto updateRegionRequestDto)
        {

            // var regionDomain = await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            var regionDomain = mapper.Map<Region>(updateRegionRequestDto);

            await regionRepository.UpdateAsync(id, regionDomain);

            if (regionDomain == null) return NotFound();

            // now retun back region dto to client

            var regionDto = mapper.Map<RegionDto>(regionDomain);

            return Ok(regionDto);

        }


        [HttpDelete("{id:guid}")] // https://localhost:6001/api/Regions/943cd20b-1537-4e3e-aee4-66082bd99150
        [Authorize(Roles = "Writer")]
        public async Task<ActionResult<RegionDto>> Delete(Guid id)
        {
            var region = await regionRepository.GetByIdAsync(id);

            if (region == null) return NotFound();

            await regionRepository.DeleteAsync(id); // no async for remove

            // if you want, you can return deleted item by converting from domain to dto -- region to RegionDto
            return Ok(mapper.Map<RegionDto>(region));


        }

    }
}
