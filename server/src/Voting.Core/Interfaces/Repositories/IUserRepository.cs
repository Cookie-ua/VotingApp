using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using Voting.Core.DTOs.User;
using Voting.Core.Entities;

namespace Voting.Core.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<IdentityResult> CreateUser(User user, string password);
        Task<User> GetUser(string userId);
        Task<IEnumerable<UserDTO>> GetAllUsers(string roleName);
    }
}
