using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voting.Api.Models.Account;
using Voting.Core.DTOs;
using Voting.Core.Interfaces.Services;

namespace Voting.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AccountController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost("register"), AllowAnonymous]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.CreateUser(_mapper.Map<RegisterViewModel, CreateUserDTOs>(model), "User");
            if (result.Successed)
                return Ok(result.Property);
            else
                return BadRequest(result.Message);
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync()
        {
            await _userService.GetAllUsersAsync("");
            return Ok();
        }

        [HttpPost("register-admin"), Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> RegisterAdminAsync([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.CreateUser(_mapper.Map<RegisterViewModel, CreateUserDTOs>(model), "Admin");
            if (result.Successed)
                return Ok(result.Property);
            else
                return BadRequest(result.Message);
        }

        [HttpGet("get-user")]
        public async Task<IActionResult> GetUserByIdAsync()
        {
            return Ok(await _userService.GetUserByIdAsync(User.Identity.Name));
        }

        [HttpGet("get-all-users"), Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            return Ok(await _userService.GetAllUsersAsync("User"));
        }

        [HttpPost("change-password"), Authorize(Roles = "User")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.ChangePasswordAsync(User.Identity.Name, model.CurrentPassword, model.NewPassword);
            if (!result.Successed)
                return BadRequest(result.Message);

            return Ok();
        }

        [HttpPost("delete-account"), Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteAccountAsync([FromBody] DeleteUserViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.DeleteUserAsync(User.Identity.Name, model.Password);
            if (!result.Successed)
                return BadRequest(result.Message);

            return Ok();
        }
    }
}