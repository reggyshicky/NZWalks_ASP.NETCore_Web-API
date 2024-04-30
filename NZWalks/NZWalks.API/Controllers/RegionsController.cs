using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public RegionsController(NZWalksDbContext dbContext, IRegionRepository iRegionRepository)
        {
            _dbContext = dbContext;
            _iRegionRepo = iRegionRepository;

        }


        [HttpGet]
        public async Task<IActionResult>  GetAll()
        {
            //Get Data from Db - Domain Models
            var regionsDomain = await _iRegionRepo.GetAllAsync();
            
            
            //Map Domain Models to Dtos
            var regionDtos = new List<RegionDto>();
            foreach(var regionDomain in regionsDomain)
            {
                regionDtos.Add(new RegionDto()
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl
                }); ;
            }
           
            //Return Dtos to the client
            return Ok(regionDtos);
        }



        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //Region region = _dbContext.Regions.Find(id); //one way to get a single region, Find() method onyly takes the primary key
            var regionDomain = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
         
            
            if (regionDomain == null)
            {
                return NotFound();
            }
            //Map RrgionDomain Model to Region Dto
            RegionDto regionDto = new RegionDto()
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            //Return Dto 
            return Ok(regionDto);
        }


        //Creating a resource and getting back the resource
        [HttpPost]
        public async Task<IActionResult> CreateRegion([FromBody] AddRequestDto addRequestDto)
        {
            //Map or covert the Dto to Domain Model

            var regionDomainModel = new Region()
            {
                Code = addRequestDto.Code,
                Name = addRequestDto.Name,
                RegionImageUrl = addRequestDto.RegionImageUrl

            };
            await _dbContext.Regions.AddAsync(regionDomainModel);
            await _dbContext.SaveChangesAsync();

            //Map DomainModel to RegionDto
            var regionDto = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return CreatedAtAction(nameof(GetById), new {id = regionDto.Id}, regionDto);
        }


        //Update Region
        //PUT: https://localhost:portNumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody]     UpdateRegionRequestDto updateRegionRequestDto)
        {
            //check f region exists
            var regionDomainModel = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            await _dbContext.SaveChangesAsync();

            //Convert Domain Model to Dto
            var regionDto = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }

        //Delete a region
        //DELETE: https://localhost:portNumber/api/regions/{id
        [HttpDelete]
        [Route("{id:Guid}")] //making it typesafe
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            var regionDomainModel = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            _dbContext.Regions.Remove(regionDomainModel); //note that we do not have a remove async method
            await _dbContext.SaveChangesAsync();

            return Ok("Deleted Successfully");
        }

    }
} 