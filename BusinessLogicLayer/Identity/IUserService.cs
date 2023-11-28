using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Identity
{
    public interface IUserService
    {
        int AddAsync(User user);
        Task<Hashes?> GetByHashAsync(string hash);
        Task<User?> GetByUsernameAsync(string username);
    }
}