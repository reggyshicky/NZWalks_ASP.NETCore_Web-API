using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Logging;
using NZWalks.API.CustomActionFilter;
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
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkDto addWalkDto)
        {
            //To check modelstate - we use the CustomValidateAttribute class
            var walk = _iMapper.Map<Walk>(addWalkDto);
            await _walkRepo.CreateAsync(walk);
            var walkResponseDto = _iMapper.Map<WalkResponseDto>(walk);
            return Ok(walkResponseDto);
        }

        //Get walks
        //GET: /api/walks?filterOn=Name&filterQuery=beach&sortBy=Name&isAscendnig=true
        //filterOn - what column do you want me to filter on
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending)
        {
            var walksDomainModel = await _walkRepo.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true);

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

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateWalk([FromRoute] Guid id, UpdateWalkRequestDto updateDto)
        {
            var walkDomainModel = _iMapper.Map<Walk>(updateDto);


            walkDomainModel = await _walkRepo.UpdateWalkAsync(id, walkDomainModel);
            if (walkDomainModel == null)
            {
                return NotFound();
            }

            return Ok(_iMapper.Map<UpdateWalkRequestDto>(walkDomainModel));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteWalk([FromRoute] Guid id)
        {
            var deletedWalkDomainModel = _walkRepo.DeleteWalkAsync(id);
            if (deletedWalkDomainModel == null)
            {
                return NotFound();
            }
            return Ok(_iMapper.Map<WalkResponseDto>(deletedWalkDomainModel));
        }

    }

}

