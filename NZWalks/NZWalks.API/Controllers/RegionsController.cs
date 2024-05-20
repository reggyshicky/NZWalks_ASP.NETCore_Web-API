using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilter;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.Dto;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbContext;
        private readonly IRegionRepository _iRegionRepo;
        private readonly IMapper _iMapper;
        public RegionsController(NZWalksDbContext dbContext, IRegionRepository iRegionRepository, IMapper Mapper)
        {
            _dbContext = dbContext;
            _iRegionRepo = iRegionRepository;
            _iMapper = Mapper;

        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get Data from Db - Domain Models
            var regionsDomain = await _iRegionRepo.GetAllAsync();

            var regionDtos = _iMapper.Map<List<RegionDto>>(regionsDomain);
            //Return Dtos to the client
            return Ok(regionDtos);
        }



        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //Region region = _dbContext.Regions.Find(id); //one way to get a single region, Find() method onyly takes the primary key
            var regionDomain = await _iRegionRepo.GetByIdAsync(id);


            if (regionDomain == null)
            {
                return NotFound();
            }
            var regionDto = _iMapper.Map<List<RegionDto>>(regionDomain);
            //Return Dto 
            return Ok(regionDto);
        }

        [HttpPost]
        [ValidateModel] //using NZWalks.API.CustomActionFilter;

        public async Task<IActionResult> Create([FromBody] AddRequestDto addRequestDto)
        {

            var regionDomainModel = _iMapper.Map<Region>(addRequestDto);
            regionDomainModel = await _iRegionRepo.CreateAsync(regionDomainModel);
            var regionDto = _iMapper.Map<RegionDto>(regionDomainModel);
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);

        }


        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionDomainModel = _iMapper.Map<Region>(updateRegionRequestDto);
            regionDomainModel = await _iRegionRepo.UpdateAsync(id, regionDomainModel);
            if (regionDomainModel == null)
            {
                return NotFound();
            }


            var regionDto = _iMapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")] //making it typesafe
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            var regionDomainModel = await _iRegionRepo.DeleteAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            var regionDto = _iMapper.Map<RegionDto>(regionDomainModel);
            return Ok(regionDto);
        }
    }
}