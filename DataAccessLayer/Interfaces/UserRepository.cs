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
    public class UserRepository
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
        public async Task<UserHash?> GetUserAsync(string username)
        {
            var hash = UserHash(username);
            return await _dbContext.Hashes
                        .Where(user => user.Hash == hash)
                        .FirstOrDefaultAsync();
        }

        public async Task<int> AddAsync(User user)
        {
            var hash = UserHash(user.UserName);
            var userExists = await _dbContext.Hashes
                        .Where(user => user.Hash == hash)
                        .FirstOrDefaultAsync();
            if (userExists != null)
            {
                _dbContext.Users.Add(user);
            }
            else
            {
                _dbContext.Users.Update(user);
            }
            return _dbContext.SaveChanges();
        }
    }
}
