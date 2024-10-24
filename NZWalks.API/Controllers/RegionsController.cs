using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    // https://localhost:portnumber/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext nZWalksDbContext;
        public RegionsController(NZWalksDbContext dbContext)
        {
            this.nZWalksDbContext = dbContext;
        }

        // GET: ALL REGIONS
        // GET: https://localhost:portnumber/api/regions
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Region[]))]
        public IActionResult GetAllRegions()
        {
            var regionsDomain = nZWalksDbContext.Region.ToList();
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Region[]))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetRegionById([FromRoute] Guid id)
        {
            var regionDomain = nZWalksDbContext.Region.FirstOrDefault(x => x.Id == id);
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
        [ProducesResponseType(StatusCodes.Status201Created,Type = typeof(Region))]
        public IActionResult CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            nZWalksDbContext.Add(regionDomainModel);
            nZWalksDbContext.SaveChanges();

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
        public IActionResult UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateDto)
        {
            var regionDomainModel = nZWalksDbContext.Region.FirstOrDefault(x => x.Id == id);

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            regionDomainModel.RegionImageUrl = updateDto.RegionImageUrl;
            regionDomainModel.Code = updateDto.Code;
            regionDomainModel.Name = updateDto.Name;

            nZWalksDbContext.SaveChanges();

            var regionDto = new RegionDto
            {
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
                Id = regionDomainModel.Id
            };

            return Ok(regionDto);

        }

    }
}
