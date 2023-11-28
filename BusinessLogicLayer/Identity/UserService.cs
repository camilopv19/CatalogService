using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Identity
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<User?> GetByUsernameAsync(string username) => await _repository.GetByUsernameAsync(username);
        public async Task<Hashes?> GetByHashAsync(string hash) => await _repository.GetByHashAsync(hash);
        public int AddAsync(User user) => _repository.AddAsync(user);
    }
}
