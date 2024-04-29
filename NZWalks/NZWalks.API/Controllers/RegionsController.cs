using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.Dto;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbContext;
        public RegionsController(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            //Get Data from Db - Domain Models
            List<Region> regionsDomain = _dbContext.Regions.ToList();

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
        public IActionResult GetById([FromRoute] Guid id)
        {
            //Region region = _dbContext.Regions.Find(id); //one way to get a single region, Find() method onyly takes the primary key
            Region regionDomain = _dbContext.Regions.FirstOrDefault(x => x.Id == id);
         
            
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
        public IActionResult CreateRegion([FromBody] AddRequestDto addRequestDto)
        {
            //Map or covert the Dto to Domain Model

            var regionDomainModel = new Region()
            {
                Code = addRequestDto.Code,
                Name = addRequestDto.Name,
                RegionImageUrl = addRequestDto.RegionImageUrl

            };
            _dbContext.Regions.Add(regionDomainModel);
            _dbContext.SaveChanges();

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
        public IActionResult Update([FromRoute] Guid id, [FromBody]     UpdateRegionRequestDto updateRegionRequestDto)
        {
            //check f region exists
            var regionDomainModel = _dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            _dbContext.SaveChanges();

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
        public IActionResult DeleteRegion([FromRoute] Guid id)
        {
            var regionDomainModel = _dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            _dbContext.Regions.Remove(regionDomainModel);
            _dbContext.SaveChanges();

            return Ok("Deleted Successfully");
        }

    }
} 