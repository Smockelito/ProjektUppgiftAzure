using Data.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NwAPI.Services;

namespace NwAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SessionController : ControllerBase
    {
        private readonly SessionService _sessionService;

        public SessionController(SessionService sessionService)
        {
            _sessionService = sessionService;
        }

        // GET: api/Session
        [HttpGet]
        public async Task<ActionResult<List<SessionDto>>> Get()
        {
            var sessions = await _sessionService.GetAllAsync();
            return Ok(sessions);
        }

        // GET api/Session/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SessionDto>> Get(int id)
        {
            var session = await _sessionService.GetByIdAsync(id);
            if (session == null) return NotFound();
            return Ok(session);
        }

        // GET api/Session/filter?difficultyId=2
        [HttpGet("filter")]
        public async Task<ActionResult<List<SessionDto>>> GetByDifficulty([FromQuery] int difficultyId)
        {
            var sessions = await _sessionService.GetByDifficultyAsync(difficultyId);
            return Ok(sessions);
        }

        // GET api/Session/random?difficultyId=2&count=3
        [HttpGet("random")]
        public async Task<ActionResult<List<SessionDto>>> GetRandom([FromQuery] int difficultyId, [FromQuery] int count = 3)
        {
            var sessions = await _sessionService.GetRandomAsync(difficultyId, count);
            return Ok(sessions);
        }

        // POST api/Session
        [HttpPost]
        public async Task<ActionResult<SessionDto>> Post([FromBody] CreateSessionDto dto)
        {
            var created = await _sessionService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        // PUT api/Session/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CreateSessionDto dto)
        {
            var updated = await _sessionService.UpdateAsync(id, dto);
            if (!updated) return NotFound();
            return NoContent();
        }

        // DELETE api/Session/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _sessionService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}

