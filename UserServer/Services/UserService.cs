using System;
using System.Threading.Tasks;
using UserServer.Database;
using UserServer.Entities;
using UserServer.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace UserServer.Services
{
    public class UserService : IUserService
    {
        private readonly UserDbContext _userDb;

        public UserService(UserDbContext userDb)
        {
            _userDb = userDb;
        }

        public async Task<ApplicationUser> FindUserByEmail(string email)
        {
            var user = await _userDb.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
            if (user == null)
                throw new Exception("User not found");
            return user;
        }

        public async Task RegisterUser(ApplicationUser user)
        {
            _userDb.Add(user);
            await _userDb.SaveChangesAsync();
        }
    }
}
