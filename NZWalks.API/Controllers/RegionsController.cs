using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

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
            var regions = nZWalksDbContext.Region.ToList();
            return Ok(regions);
        }

        // GET: ALL REGION BY ID
        // GET: https://localhost:portnumber/api/regions/08dd2694-27f9-4997-b8bc-5020631574c0
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetRegionById([FromRoute] Guid id)
        {
            var region = nZWalksDbContext.Region.FirstOrDefault(x => x.Id == id);
            if (region == null)
            {
                return NotFound();
            }
            return Ok(region);
        }

    }
}
