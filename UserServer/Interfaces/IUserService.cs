using System.Threading.Tasks;
using UserServer.Entities;

namespace UserServer.Interfaces
{
    public interface IUserService
    {
        Task RegisterUser(ApplicationUser user);
        Task<ApplicationUser> FindUserByEmail(string email);
        Task UpdateUser(string email, ApplicationUser newUserData);
    }
}
