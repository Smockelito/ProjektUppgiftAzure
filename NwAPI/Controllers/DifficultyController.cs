using Data.Dto;
using Microsoft.AspNetCore.Mvc;
using NwAPI.Services;

namespace NwAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DifficultyController : ControllerBase
    {
        private readonly DifficultyService _difficultyService;

        public DifficultyController(DifficultyService difficultyService)
        {
            _difficultyService = difficultyService;
        }

        // GET: api/Difficulty
        [HttpGet]
        public async Task<ActionResult<List<DifficultyDto>>> Get()
        {
            var difficulties = await _difficultyService.GetAllAsync();
            return Ok(difficulties);
        }
    }
}
