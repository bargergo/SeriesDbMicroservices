using System;
using System.Threading.Tasks;
using UserServer.Database;
using UserServer.Entities;
using UserServer.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
            var user = await _userDb.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Where(u => u.Email == email).FirstOrDefaultAsync();
            return user;
        }

        public async Task<ApplicationUser> FindUserById(long id)
        {
            var user = await _userDb.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Where(u => u.Id == id).FirstOrDefaultAsync();
            return user;
        }

        public async Task RegisterUser(ApplicationUser user)
        {
            _userDb.Add(user);
            var userRole = await _userDb.Roles.FirstOrDefaultAsync(r => r.NormalizedName == "USER");
            user.UserRoles = new List<ApplicationUserRole>
            {
                new ApplicationUserRole
                {
                    Role = userRole
                }
            };
            await _userDb.SaveChangesAsync();
        }

        public async Task<ApplicationUser> UpdateUser(string email, ApplicationUser newUserData)
        {
            var user = await FindUserByEmail(email);
            user.Firstname = newUserData.Firstname;
            user.Lastname = newUserData.Lastname;
            await _userDb.SaveChangesAsync();
            return user;
        }
    }
}
