using Data;
using Data.Dto;
using Microsoft.EntityFrameworkCore;

namespace NwAPI.Services
{
    public class DifficultyService
    {
        private readonly NoseworkDbContext _context;

        public DifficultyService(NoseworkDbContext context)
        {
            _context = context;
        }

        public async Task<List<DifficultyDto>> GetAllAsync()
        {
            return await _context.DifficultyLevels
                .Select(d => new DifficultyDto
                {
                    Id = d.Id,
                    CategoryName = d.CategoryName
                })
                .ToListAsync();
        }
    }
}
