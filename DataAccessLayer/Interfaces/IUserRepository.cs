using DataAccessLayer.Entities;

namespace DataAccessLayer.Interfaces
{
    public interface IUserRepository
    {
        int AddAsync(User user);
        Task<Hashes?> GetByHashAsync(string username);
        Task<User?> GetByUsernameAsync(string username);
    }
}