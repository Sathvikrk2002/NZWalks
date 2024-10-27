using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    // https://localhost:portnumber/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository repository;
        private readonly IMapper mapper;
        public RegionsController(IRegionRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        // GET: ALL REGIONS
        // GET: https://localhost:portnumber/api/regions
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegionDto[]))]
        public async Task<IActionResult> GetAllRegions()
        {
            var regionsDomain = await repository.GetAllRegionsAsync();
            var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);
            return Ok(regionsDto);
        }

        // GET: ALL REGION BY ID
        // GET: https://localhost:portnumber/api/regions/08dd2694-27f9-4997-b8bc-5020631574c0
        [HttpGet]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegionDto[]))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            var regionDomain = await repository.GetRegionByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }
            var regionDto = mapper.Map<RegionDto>(regionDomain);
            return Ok(regionDto);
        }

        // POST: POST NEW REGION
        // POST: https://localhost:portnumber/api/regions
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RegionDto))]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            if (ModelState.IsValid)
            {
                var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);
                await repository.CreateRegionAsync(regionDomainModel);
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);
                return CreatedAtAction(nameof(GetRegionById), new { id = regionDto.Id }, regionDto);
            }
            else
            {
                return BadRequest();
            }
        }

        // PUT: UPDATE REGION
        // PUT: https://localhost:portnumber/api/regions/08dd2694-27f9-4997-b8bc-5020631574c0
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateDto)
        {
            var regionDomainModel = mapper.Map<Region>(updateDto);
            regionDomainModel = await repository.UpdateRegionAsync(id, regionDomainModel);
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            regionDomainModel.RegionImageUrl = updateDto.RegionImageUrl;
            regionDomainModel.Code = updateDto.Code;
            regionDomainModel.Name = updateDto.Name;
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            return Ok(regionDto);

        }

        // DELETE: DELETE REGION
        // DELETE: https://localhost:portnumber/api/regions/08dd2694-27f9-4997-b8bc-5020631574c0

        [HttpDelete]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegionDto[]))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            var regionDomainModel = await repository.DeleteRegionAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            return Ok(regionDto);
        }

    }
}
