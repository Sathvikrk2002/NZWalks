using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository repository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository repository,IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        // GET: ALL WALKS
        // GET: https://localhost:portnumber/api/walks
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WalkDto[]))]
        public async Task<IActionResult> GetAllWalks()
        {
            var walksDomain = await repository.GetAllWalksAsync();
            return Ok(mapper.Map<List<WalkDto>>(walksDomain));
        }

        // POST: POST NEW WALK
        // POST: https://localhost:portnumber/api/walks
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(WalkDto))]
        public async Task<IActionResult> CreateWalk([FromBody] AddWalkDto addWalkDto)
        {
            var walkDomainModel = mapper.Map<Walk>(addWalkDto);
            await repository.CreateWalkAsync(walkDomainModel);
            var walkDto = mapper.Map<WalkDto>(walkDomainModel);
            return CreatedAtAction(nameof(GetWalkById), new { id = walkDto.Id }, walkDto);
        }

        // GET: ALL Walk BY ID
        // GET: https://localhost:portnumber/api/walks/08dd2694-27f9-4997-b8bc-5020631574c0
        [HttpGet]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WalkDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWalkById([FromRoute] Guid id)
        {
            var walkDomain = await repository.GetWalkByIdAsync(id);
            if (walkDomain == null)
            {
                return NotFound();
            }
            var walkDto = mapper.Map<WalkDto>(walkDomain);
            return Ok(walkDto);
        }
    }
}
