using RunGroopWebApp.Models;

namespace RunGroopWebApp.Interfaces
{
    public interface IRaceRepository
    {
        Task<IEnumerable<Race>> GetAll();
        Task<Race> GetByIdAysnc(int id);
        Task<IEnumerable<Race>> GetAllRacesByCity(string city);

        bool Add(Race race );
        bool Delete(Race race);
        bool Update(Race race);
        bool Save();
    }
}
