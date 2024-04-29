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

        [HttpPost]
        [Route("addRegion")]
        public IActionResult AddRegion(Region  region)
        {
            _dbContext.Regions.Add(region);
            _dbContext.SaveChanges();
            return Ok("Region added succesfully");
        }
    }
} 