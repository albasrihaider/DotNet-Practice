using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Repository
{
    public class RaceRepository : IRaceRepository
    {
        private readonly ApplicationDbContext _context;

        public RaceRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Race race)
        {
           _context.Add(race);
            return Save();
        }

        public bool Delete(Race race)
        {
            _context.Remove(race);
            return Save();
        }

        public async Task<IEnumerable<Race>> GetAll()
        {
            return await _context.Races.ToListAsync();

            /*
              return await _context.Races
                .Include(r => r.Address) // Eagerly load the Address navigation property
                .ToListAsync();
             */
        }

        public async Task<IEnumerable<Race>> GetAllRacesByCity(string city)
        {
            return await _context.Races.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public async Task<Race> GetByIdAysnc(int id)
        {
            return await _context.Races
                .Include(c => c.Address) // Eagerly load the Address navigation property
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Race> GetByIdAysncNoTracking(int id)
        {
            return await _context.Races
                .Include(c => c.Address) // Eagerly load the Address navigation property
                .AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Race race)
        {

            _context.Update(race);
            return Save();
        }
    }
}
