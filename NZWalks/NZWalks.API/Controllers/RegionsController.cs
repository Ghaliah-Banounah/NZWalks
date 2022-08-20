using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await _regionRepository.GetAllAsync();

            /* retrun DTO regions
            var regionsDTO = new List<Models.DTO.Region>();
            regions.ToList().ForEach(region =>
            {
                var regionDTO = new Models.DTO.Region()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    Area = region.Area,
                    Lat = region.Lat,
                    Long = region.Long,
                    Population = region.Population,

                };
                regionsDTO.Add(regionDTO);
            });*/

            var regionsDTO = _mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await _regionRepository.GetAsync(id);

            if (region == null)
            {
                return NotFound();
            }

            var regionDTO = _mapper.Map<Models.DTO.Region>(region);

            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionsAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            // Validate the request
            //if (!ValidateAddRegionAsync(addRegionRequest))
            //{
            //    return BadRequest(ModelState);
            //}

            // Convert passed region to Domain model
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population,
            };

            // Pass details to repository
            region = await _regionRepository.AddAsync(region);

            // Convert back to DTO
            var regionsDTO = _mapper.Map<Models.DTO.Region>(region);

            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionsDTO.Id }, regionsDTO);
        }

        [HttpDelete]
        // Specify the input value to be of type Guid
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionsAsync(Guid id)
        {
            // Get Region from Db
            var region = await _regionRepository.DeleteAsync(id);

            // If null then region not found 
            if (region == null)
            {
                return NotFound();
            }

            // Convert response to DTO
            var regionDTO = _mapper.Map<Models.DTO.Region>(region);

            // Return Ok response
            return Ok(regionDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            // Validate incoming request
            //if (!ValidateUpdateRegionAsync(updateRegionRequest))
            //{
            //    return BadRequest(ModelState);
            //}

            // Convert DTO to Domain model
            var region = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population,
            };

            // Update Region using repository
            region = await _regionRepository.UpdateAsync(id, region);

            // If null then not found
            if (region == null)
            {
                return NotFound();
            }

            // Convert Domaint back to DTO 
            var regionsDTO = _mapper.Map<Models.DTO.Region>(region);

            // return Ok response

            return Ok(regionsDTO);
        }

        #region Private Methods

        private bool ValidateAddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            if (addRegionRequest == null)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Code), "Add region data is required");
            }

            if (string.IsNullOrWhiteSpace(addRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Code), $"{nameof(addRegionRequest.Code)} can't be empty");
            }

            if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Name), $"{nameof(addRegionRequest.Name)} can't be empty");
            }

            if (addRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Area), $"{nameof(addRegionRequest.Area)} can't be less than or equal to 0");
            }

            if (addRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Population), $"{nameof(addRegionRequest.Population)} can't be less than 0");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }

        private bool ValidateUpdateRegionAsync(Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            if (updateRegionRequest == null)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code), "Add region data is required");
            }

            if (string.IsNullOrWhiteSpace(updateRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code), $"{nameof(updateRegionRequest.Code)} can't be empty");
            }

            if (string.IsNullOrWhiteSpace(updateRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Name), $"{nameof(updateRegionRequest.Name)} can't be empty");
            }

            if (updateRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Area), $"{nameof(updateRegionRequest.Area)} can't be less than or equal to 0");
            }

            if (updateRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Population), $"{nameof(updateRegionRequest.Population)} can't be less than 0");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
