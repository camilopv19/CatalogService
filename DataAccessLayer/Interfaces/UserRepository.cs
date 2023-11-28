using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public class UserRepository : IUserRepository
    {

        private readonly AppDbContext _dbContext;
        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private static string UserHash(string username)
        {
            return Convert.ToBase64String(MD5.HashData(Encoding.UTF8.GetBytes(username)));
        }
        public async Task<Hashes?> GetByHashAsync(string username)
        {
            var hash = UserHash(username);
            return await _dbContext.Hashes
                        .Where(user => user.Hash == hash)
                        .FirstOrDefaultAsync();
        }

        public int AddAsync(User user)
        {
            try
            {
                int result = 0;
                var hash = UserHash(user.UserName);
                var userExists = _dbContext.Hashes.Any(hashObj => hashObj.Hash == hash);
                if (!userExists)
                {
                    var userHash = new Hashes { Hash = hash };
                    user.Claims.Add(new UserClaim() { Type = "role", Value = user.Role });
                    _dbContext.Hashes.Add(userHash);
                    _dbContext.Users.Add(user);
                    result = _dbContext.SaveChanges();
                }
                return result;
            }
            catch (Exception _e)
            {

                throw _e;
            }

        }
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbContext.Users
                        .Where(user => user.UserName == username)
                        .FirstOrDefaultAsync();
        }
    }
}
