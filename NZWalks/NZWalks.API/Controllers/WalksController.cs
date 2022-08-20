using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalksRepository _walkRepository;
        private readonly IMapper _mapper;

        public WalksController(IWalksRepository walkRepository, IMapper mapper)
        {
            _walkRepository = walkRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            // Fetch data from DB - Domain Walks
            var walks = await _walkRepository.GetAllAsync();

            // Convert Domain walks to DTO
            var walksDTO = _mapper.Map<List<Models.DTO.Walks>>(walks);

            // Retrun response 
            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            // Get walk Domain object from DB
            var walk = await _walkRepository.GetAsync(id);

            // If null return not found
            if (walk == null)
            {
                return NotFound();
            }

            // Convert Domain object to DTO 
            var walkDTO = _mapper.Map<Models.DTO.Walks>(walk);

            // Return response
            return Ok(walkDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            // Convert DTO to Domain object
            var walk = new Models.Domain.Walks
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId,
            };

            // Pass Domain object to repository to presist this
            await _walkRepository.AddAsync(walk);

            // Convert domain object back to DTO 
            var walkDTO = _mapper.Map<Models.DTO.Walks>(walk);

            // Send DTO response back to client
            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }


        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updWalkRequest)
        {
            // Convert DTO to Domain object
            var walk = _mapper.Map<Models.Domain.Walks>(updWalkRequest);

            // Update Domain object using repository
            walk = await _walkRepository.UpdateAsync(id, walk);

            // If null then not found
            if (walk == null)
            {
                return NotFound();
            }

            // Convert domain object back to DTO 
            var walkDTO = _mapper.Map<Models.DTO.Walks>(walk);

            // return Ok response
            return Ok(walkDTO);
        }

        [HttpDelete]
        // Specify the input value to be of type Guid
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalksAsync(Guid id)
        {
            // Get Walk from Db
            var walk = await _walkRepository.DeleteAsync(id);

            // If null then region not found 
            if (walk == null)
            {
                return NotFound();
            }

            // Convert response to DTO
            var walkDTO = _mapper.Map<Models.DTO.Walks>(walk);

            // Return Ok response
            return Ok(walkDTO);
        }
    }
}
