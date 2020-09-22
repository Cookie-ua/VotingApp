using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voting.Api.Models.Account;
using Voting.Core.Interfaces.Services;

namespace Voting.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _authService.Authenticate(model.Email, model.Password);
            if (!result.Successed)
                return BadRequest(result.Message);

            return Ok(new { auth_token = result.Message});
        }
    }
}
