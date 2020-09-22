using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voting.Core.DTOs;
using Voting.Core.DTOs.Admin;
using Voting.Core.DTOs.User;
using Voting.Core.Entities;
using Voting.Core.Enums;
using Voting.Core.Infrastructure;
using Voting.Core.Interfaces;
using Voting.Core.Interfaces.Services;

namespace Voting.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager, IUnitOfWork uow, IMapper mapper)
        {
            _userManager = userManager;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<OperationResult> CreateUser(CreateUserDTOs createUser, string roleName)
        {
            try
            {
                var user = new User()
                {
                    UserName = createUser.Email,
                    Email = createUser.Email
                };
                var result = await _uow.UserRepository.CreateUser(user, createUser.Password);

                if (result.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, roleName);
                    if (!roleResult.Succeeded)
                        return new OperationResult(
                            false,
                            string.Join(",", roleResult.Errors.Select(x => x.Description)),
                            "");

                    return new OperationResult(true, "", user.Id.ToString());
                }
                else
                {
                    return new OperationResult(
                            false,
                            string.Join(",", result.Errors.Select(x => x.Description)),
                            "");
                }

            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, "");
            }
        }
        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync(string roleName)
        {
            return await _uow.UserRepository.GetAllUsers(roleName);
        }
        public async Task<UserDTO> GetUserByIdAsync(string userId)
        {
            return _mapper.Map<UserDTO>(await _uow.UserRepository.GetUser(userId));
        }
        public async Task<OperationResult> ChangeUserStatusAsync(string userId, Status status)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                var userRoles = await _userManager.GetRolesAsync(user);

                if (userRoles.Contains("SuperAdmin"))
                    return new OperationResult(false, "Invalid operation", "");

                if (user == null)
                    return new OperationResult(false);

                user.IsBlocked = Status.Block == status;
                await _userManager.UpdateAsync(user);

                return new OperationResult(true);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, "");
            }
        }
        public async Task<OperationResult> ChangeUserInRole(string userId, string roleName)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles.Contains("SuperAdmin"))
                    return new OperationResult(false, "Invalid operation", "");

                if (userRoles.Contains(roleName))
                    return new OperationResult(true);

                await _userManager.RemoveFromRolesAsync(user, userRoles);
                await _userManager.AddToRoleAsync(user, roleName);

                return new OperationResult(true);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, "");
            }
        }
        public async Task<IEnumerable<QuestionInfo>> GetAdminInfoAsync()
        {
            //var mostVotedQuestion = await _uow.VoteRepository.Get("")
            //                                .GroupBy(x => x.QuestionId)
            //                                .Select(x => new
            //                                {
            //                                    QuestionId = x.Key,
            //                                    Count = x.Count()
            //                                }).ToListAsync();
            //var id = mostVotedQuestion.First(x => x.Count == mostVotedQuestion.Max(x => x.Count)).QuestionId;
            //var question = _uow.QuestionRepository.Get(id);

            return await _uow.QuestionRepository.Get("")
                                        .GroupBy(x => x.IsActive)
                                        .Select(x => new QuestionInfo()
                                        {
                                            Type = x.Key,
                                            Count = x.Count()
                                        })
                                        .OrderByDescending(o => o.Type)
                                        .ToListAsync();
        }
        public async Task<OperationResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return new OperationResult(false);

                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                if(!result.Succeeded)
                    return new OperationResult(false, string.Join(",", result.Errors.Select(x => x.Description)), "");

                return new OperationResult(true);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, "");
            }
        }
        public async Task<OperationResult> DeleteUserAsync(string userId, string password)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return new OperationResult(false);

                if (!await _userManager.CheckPasswordAsync(user, password))
                    return new OperationResult(false, "Incorrect password", "");

                var deleteResult = await _userManager.DeleteAsync(user);
                if (!deleteResult.Succeeded)
                    return new OperationResult(false, "Unexpected error occurred deleting user", "");

                return new OperationResult(true);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, "");
            }
        }
        public async Task<UserDTO> GetAdminByIdAsync(string userId)
        {
            var admin = await _userManager.FindByIdAsync(userId);
            if (await _userManager.IsInRoleAsync(admin, "Admin"))
                return _mapper.Map<UserDTO>(admin);

            return null;
        }
        public async Task<OperationResult> ResetPassword(string userId, string password)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return new OperationResult(false, "User not found", "");

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var resetResult = await _userManager.ResetPasswordAsync(user, token, password);
                if (!resetResult.Succeeded)
                    return new OperationResult(false, string.Join(",", resetResult.Errors.Select(x => x.Description)), "");
                
                return new OperationResult(true);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, "");
            }
        }
    }
}