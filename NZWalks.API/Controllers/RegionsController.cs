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
        public RegionsController(IRegionRepository repository)
        {
            this.repository = repository;
        }

        // GET: ALL REGIONS
        // GET: https://localhost:portnumber/api/regions
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegionDto[]))]
        public async Task<IActionResult> GetAllRegions()
        {
            var regionsDomain = await repository.GetAllRegionsAsync();
            var regionsDto = new List<RegionDto>();
            foreach (var region in regionsDomain)
            {
                regionsDto.Add(new RegionDto()
                {
                    Code = region.Code,
                    Id = region.Id,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl,
                });
            }

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
            var regionDto = new RegionDto()
            {
                Code = regionDomain.Code,
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl,
            };

            return Ok(regionDto);
        }

        // POST: POST NEW REGION
        // POST: https://localhost:portnumber/api/regions
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RegionDto))]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            await repository.CreateRegionAsync(regionDomainModel);

            var regionDto = new RegionDto
            {
                RegionImageUrl = regionDomainModel.RegionImageUrl,
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
            };

            return CreatedAtAction(nameof(GetRegionById), new { id = regionDto.Id }, regionDto);
        }

        // PUT: UPDATE REGION
        // PUT: https://localhost:portnumber/api/regions/08dd2694-27f9-4997-b8bc-5020631574c0
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateDto)
        {
            var regionDomainModel = new Region
            {
                Code = updateDto.Code,
                Name = updateDto.Name,
                RegionImageUrl = updateDto.RegionImageUrl,
            };

            regionDomainModel = await repository.UpdateRegionAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            regionDomainModel.RegionImageUrl = updateDto.RegionImageUrl;
            regionDomainModel.Code = updateDto.Code;
            regionDomainModel.Name = updateDto.Name;


            var regionDto = new RegionDto
            {
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
                Id = regionDomainModel.Id
            };

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

            var regionDto = new RegionDto
            {
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
                Id = regionDomainModel.Id
            };
            return Ok(regionDto);
        }

    }
}
