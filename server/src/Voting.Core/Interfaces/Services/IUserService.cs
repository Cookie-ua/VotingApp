using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Voting.Core.DTOs;
using Voting.Core.DTOs.Admin;
using Voting.Core.DTOs.User;
using Voting.Core.Entities;
using Voting.Core.Enums;
using Voting.Core.Infrastructure;

namespace Voting.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<OperationResult> CreateUser(CreateUserDTOs createUser, string roleName);
        Task<UserDTO> GetUserByIdAsync(string userId);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync(string roleName);
        Task<OperationResult> ChangeUserStatusAsync(string userId, Status status);
        Task<OperationResult> ChangeUserInRole(string userId, string roleName);
        Task<IEnumerable<QuestionInfo>> GetAdminInfoAsync();
        Task<OperationResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<OperationResult> DeleteUserAsync(string userId, string password);
        Task<UserDTO> GetAdminByIdAsync(string userId);
        Task<OperationResult> ResetPassword(string userId, string password);
    }
}