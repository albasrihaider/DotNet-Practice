using RunGroopWebApp.Models;

namespace RunGroopWebApp.Interfaces
{
    public interface IClubRepository
    {

        Task<IEnumerable<Club>> GetAll();
        Task<Club> GetByIdAysnc(int id);
        Task<IEnumerable<Club>> GetClubsByCity(string city);

        bool Add(Club club);
        bool Delete(Club club);
        bool Update(Club club);
        bool Save();
    }
}
