using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Voting.Core.DTOs.User;
using Voting.Core.Entities;
using Voting.Core.Interfaces.Repositories;
using Voting.Data.EF;

namespace Voting.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserRepository(ApplicationDbContext db, UserManager<User> userManager) : base(db)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsers(string roleName)
        {
            return (await _userManager.GetUsersInRoleAsync(roleName)).Select(x => new UserDTO()
            {
                Id = x.Id,
                UserName = x.UserName,
                IsBlocked = x.IsBlocked
            });

            //var us = await (from user in _context.Users
            //                let query = (from userRole in _context.UserRoles
            //                             where userRole.UserId.Equals(user.Id)
            //                             join role in _context.Roles on userRole.RoleId equals role.Id
            //                             where role.Name == roleName
            //                             select role.Name)
            //                select new UserDTO
            //                {
            //                    Id = user.Id,
            //                    UserName = user.UserName,
            //                    Roles = query.ToList()
            //                }).ToListAsync();
        }
        public async Task<User> GetUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "SuperAdmin"))
                return null;

            return user;
        }
        public async Task<IdentityResult> CreateUser(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }
    }
}