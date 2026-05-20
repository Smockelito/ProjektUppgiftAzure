using Data;
using Data.Dto;
using Data.Ent;
using Microsoft.EntityFrameworkCore;

namespace NwAPI.Services
{
    public class SessionService
    {
        private readonly NoseworkDbContext _context;

        public SessionService(NoseworkDbContext context)
        {
            _context = context;
        }

        public async Task<List<SessionDto>> GetAllAsync()
        {
            return await _context.Sessions
                .Include(s => s.Difficulties)
                .Select(s => new SessionDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    ImageUrl = s.ImageUrl,
                    DifficultyIds = s.Difficulties.Select(d => d.Id).ToList()
                })
                .ToListAsync();
        }

        public async Task<List<SessionDto>> GetByDifficultyAsync(int difficultyId)
        {
            return await _context.Sessions
                .Include(s => s.Difficulties)
                .Where(s => s.Difficulties.Any(d => d.Id == difficultyId))
                .Select(s => new SessionDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    ImageUrl = s.ImageUrl,
                    DifficultyIds = s.Difficulties.Select(d => d.Id).ToList()
                })
                .ToListAsync();
        }

        public async Task<List<SessionDto>> GetRandomAsync(int difficultyId, int count = 3)
        {
            var sessions = await _context.Sessions
                .Include(s => s.Difficulties)
                .Where(s => s.Difficulties.Any(d => d.Id == difficultyId))
                .ToListAsync();

            return sessions
                .OrderBy(_ => Guid.NewGuid())
                .Take(count)
                .Select(s => new SessionDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    ImageUrl = s.ImageUrl,
                    DifficultyIds = s.Difficulties.Select(d => d.Id).ToList()
                })
                .ToList();
        }

        public async Task<SessionDto?> GetByIdAsync(int id)
        {
            var session = await _context.Sessions
                .Include(s => s.Difficulties)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (session == null) return null;

            return new SessionDto
            {
                Id = session.Id,
                Name = session.Name,
                Description = session.Description,
                ImageUrl = session.ImageUrl,
                DifficultyIds = session.Difficulties.Select(d => d.Id).ToList()
            };
        }

        public async Task<SessionDto> CreateAsync(CreateSessionDto dto)
        {
            var difficulties = await _context.DifficultyLevels
                .Where(d => dto.DifficultyIds.Contains(d.Id))
                .ToListAsync();

            var session = new SessionEnt
            {
                Name = dto.Name,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                Difficulties = difficulties
            };

            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            return new SessionDto
            {
                Id = session.Id,
                Name = session.Name,
                Description = session.Description,
                ImageUrl = session.ImageUrl,
                DifficultyIds = session.Difficulties.Select(d => d.Id).ToList()
            };
        }

        public async Task<bool> UpdateAsync(int id, CreateSessionDto dto)
        {
            var session = await _context.Sessions
                .Include(s => s.Difficulties)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (session == null) return false;

            var difficulties = await _context.DifficultyLevels
                .Where(d => dto.DifficultyIds.Contains(d.Id))
                .ToListAsync();

            session.Name = dto.Name;
            session.Description = dto.Description;
            session.ImageUrl = dto.ImageUrl;
            session.Difficulties = difficulties;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var session = await _context.Sessions.FindAsync(id);
            if (session == null) return false;

            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
