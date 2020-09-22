using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voting.Api.Models.Admin;
using Voting.Core.Interfaces.Services;
using Voting.Data.EF;

namespace Voting.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("get-admin-info")]
        public async Task<IActionResult> GetAdminInfoAsync()
        {
            return Ok(await _userService.GetAdminInfoAsync());
        }

        [HttpGet("get-user/{id}")]
        public async Task<IActionResult> GetUserByIdAsync(string id)
        {
            return Ok(await _userService.GetUserByIdAsync(id));
        }

        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            return Ok(await _userService.GetAllUsersAsync("User"));
        }

        [HttpPost("change-user-status")]
        public async Task<IActionResult> ChangeUserStatusAsync([FromBody] ChangeStatusViewModel model)
        {
            var result = await _userService.ChangeUserStatusAsync(model.UserId, model.Status);
            if (!result.Successed)
                return BadRequest(result.Message);

            return Ok();
        }


        [HttpGet("get-all-admins"), Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAllAdminsAsync()
        {
            return Ok(await _userService.GetAllUsersAsync("Admin"));
        }

        [HttpGet("get-admin/{id}"), Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAdminAsync(string id)
        {
            return Ok(await _userService.GetAdminByIdAsync(id));
        }

        [HttpPost("change-admin-status"), Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ChangeAdminStatusAsync([FromBody] ChangeStatusViewModel model)
        {
            var result = await _userService.ChangeUserStatusAsync(model.UserId, model.Status);
            if (!result.Successed)
                return BadRequest(result.Message);

            return Ok();
        }
        
        [HttpPost("change-role-admin"), Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> MakeUserAdminAsync([FromBody] ChangeUserInRoleViewModel model)
        {
            var result = await _userService.ChangeUserInRole(model.UserId, model.Role);
            if (!result.Successed)
                return BadRequest(result.Message);

            return Ok();
        }

        [HttpPost("reset-password"), Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.ResetPassword(model.UserId, model.Password);
            if (!result.Successed)
                return BadRequest(result.Message);

            return Ok();
        }
    }
}