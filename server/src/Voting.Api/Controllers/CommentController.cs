using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voting.Api.Models.Question;
using Voting.Core.Interfaces.Services;

namespace Voting.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("get-comments")]
        public async Task<IActionResult> GetCommentsAsync([FromQuery] int? page, int questionId)
        {
            return Ok(await _commentService.GetAllCommentsAsync(page, questionId));
        }

        [HttpGet("get-all-child-comments/{id}")]
        public async Task<IActionResult> GetCommentsByUser(int id)
        {
            return Ok(await _commentService.GetAllChildCommentsAsync(id));
        }

        [HttpGet("get-comments-by-user")]
        public async Task<IActionResult> GetCommentsByUser()
        {
            return Ok(await _commentService.GetAllCommentsByUserAsync(User.Identity.Name));
        }

        [HttpDelete("delete-comment/{id}")]
        public async Task<IActionResult> DeleteCommentAsync(int id)
        {
            var res = await _commentService.DeleteCommentAsync(id);
            if (!res.Successed)
                return BadRequest(res.Message);

            return Ok();
        }

        [HttpPost("change-comment-status/{id}"), Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> ChangeCommentStatusAsync([FromBody] ChangeStatusViewModel model)
        {
            var res = await _commentService.ChangeCommentStatusAsync(model.Id, model.Status);
            if (!res.Successed)
                return BadRequest(res.Message);

            return Ok();
        }
    }
}