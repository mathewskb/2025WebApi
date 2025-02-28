using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Models.DTOs.Domain;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")] // api/walks
    [ApiController]
    public class WalksController(IWalkRepository walkRepository, IMapper mapper) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<WalkDto>> Create(AddWalkRequestDto addWalksRequestDto)
        {
            var walkDomainModel = mapper.Map<Walk>(addWalksRequestDto);

            await walkRepository.CreateAsync(walkDomainModel);

            // map to dto

            return Ok(mapper.Map<WalkDto>(walkDomainModel));

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WalkDto>>> GetAll()
        {
            var walksDomainModel = await walkRepository.GetAllAsync();

            return Ok(mapper.Map<IEnumerable<WalkDto>>(walksDomainModel));


        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<WalkDto>> GetWalkById(Guid id)
        {
            var walkdomainmodel = await walkRepository.GetByIdAsync(id);

            if (walkdomainmodel == null) return NotFound();

            return Ok(mapper.Map<WalkDto>(walkdomainmodel));

        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<WalkDto>> Update(Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            // lets map dto to domain model
            var walkdomain = mapper.Map<Walk>(updateWalkRequestDto);
            // database call
            var resultmodel = await walkRepository.UpdateAsync(id, walkdomain);

            if (resultmodel == null) return NotFound();

            // map back to dto 
            var result = mapper.Map<WalkDto>(resultmodel);

            return Ok(result);


        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Walk>> DeleteAsync(Guid id)
        {
            var result = await walkRepository.DeleteAsync(id);

            if (result == null) return NotFound();

            //map tp dto

            return Ok(mapper.Map<WalkDto>(result));
        }
    }
}
