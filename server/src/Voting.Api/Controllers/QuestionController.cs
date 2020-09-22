using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voting.Api.Models.Question;
using Voting.Core.Entities;
using Voting.Core.Interfaces.Services;

namespace Voting.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly IMapper _mapper;

        public QuestionController(
            IQuestionService questionService, 
            IMapper mapper)
        {
            _questionService = questionService;
            _mapper = mapper;
        }

        [HttpGet("get-questions")]
        public async Task<IActionResult> GetQuestions([FromQuery] int? page)
        {
            return Ok(await _questionService.GetAllQuestionsAsync(page, 10));
        }

        [HttpGet("get-question/{id}")]
        public async Task<IActionResult> GetQuestion(int id)
        {
            return Ok(await _questionService.GetQuestionAsync(id));
        }

        [HttpGet("get-questions-user")]
        public async Task<IActionResult> GetQuestionsByUser()
        {
            return Ok(await _questionService.GetAllQuestionsByUserAsync(User.Identity.Name));
        }

        [HttpPost("add-question-user")]
        public async Task<IActionResult> AddQuestion([FromBody] AddQuestionViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _questionService.AddQuestionByUserAsync(_mapper.Map<Question>(model));
            if (!result.Successed)
                return BadRequest(result.Message);

            return Ok();
        }

        [HttpDelete("delete-question/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _questionService.DeleteQuestionAsync(id);

            if (!result.Successed)
                return BadRequest(result.Message);

            return Ok();
        }

        [HttpGet("get-votes-by-user")]
        public async Task<IActionResult> GetAllVotesByUserAsync()
        {
            return Ok(await _questionService.GetAllVotesByUserAsync());
        }

        [HttpPost("vote-for-question"), Authorize(Roles = "User")]
        public async Task<IActionResult> VoteForQuestionAsync([FromBody] VoteForQuestionViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _questionService.VoteForQuestionAsync(model.QuestionId, model.Answer);

            if (!result.Successed)
                return BadRequest(result.Message);

            return Ok();
        }
        

        [HttpGet("get-questions-admin"), Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> GetAdminQuestions([FromQuery] int? page)
        {
            return Ok(await _questionService.GetQuestionsForAdminAsync(page, 10));
        }

        [HttpGet("get-question-admin/{id}"), Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> GetQuestionAdmin(int id)
        {
            return Ok(await _questionService.GetQuestionForAdminAsync(id));
        }

        [HttpPost("add-question-admin"), Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> AddQuestionAdminAsync([FromBody] AddQuestionViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _questionService.AddQuestionByAdminAsync(_mapper.Map<Question>(model));
            if (!result.Successed)
                return BadRequest(result.Message);

            return Ok();
        }

        [HttpPost("update-question-admin"), Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> UpdateQuestionAdminAsync([FromBody] AddQuestionViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _questionService.UpdateQuestionByAdminAsync(_mapper.Map<Question>(model));
            if (!result.Successed)
                return BadRequest(result.Message);

            return Ok();
        }

        [HttpPost("change-question-status"), Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> ChangeQuestionStatusAsync([FromBody] ChangeStatusViewModel model)
        {
            var result = await _questionService.ChangeQuestionStatusAsync(model.Id, model.Status);
            if (!result.Successed)
                return BadRequest(result.Message);

            return Ok();
        }

        [HttpDelete("delete-question-admin/{id}"), Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> DeleteQuestionAdminAsync(int id)
        {
            var result = await _questionService.DeleteQuestionByAdminAsync(id);
            if (!result.Successed)
                return BadRequest(result.Message);

            return Ok();
        }
    }
}