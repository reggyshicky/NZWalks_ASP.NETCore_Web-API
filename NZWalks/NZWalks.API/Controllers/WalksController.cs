using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Logging;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.Dto;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _iMapper;
        private readonly IWalkRepository _walkRepo;
        private readonly ILogger<WalksController> _logger;
        public WalksController(IMapper mapper, IWalkRepository walkRepo, ILogger<WalksController> logger)
        {
            this._iMapper = mapper;
            this._walkRepo = walkRepo;
            this._logger = logger;
        }
        //CREATE WALK
        //POST> /api/walks
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkDto addWalkDto)
        {
            //Map Dto to Domain Model
            var walk = _iMapper.Map<Walk>(addWalkDto);
            await _walkRepo.CreateAsync(walk);
            //Map Domain Model to Dto
            var walkResponseDto = _iMapper.Map<WalkResponseDto>(walk);
            return Ok(walkResponseDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var walksDomainModel = await _walkRepo.GetAllAsync();

            return Ok(_iMapper.Map<List<WalkResponseDto>>(walksDomainModel));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await _walkRepo.GetByIdAsync(id);
            if (walkDomainModel == null)
            {
                return NotFound();
            }
            return Ok(_iMapper.Map<WalkResponseDto>(walkDomainModel));
        }

    }

}

