using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Voting.Core.Interfaces;
using Voting.Core.Interfaces.Services;
using Voting.Core.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Voting.Core.DTOs.Question;
using Microsoft.AspNetCore.SignalR;
using Voting.Core.Hubs;
using Voting.Core.Interfaces.Hubs;
using Microsoft.AspNetCore.Http;
using Voting.Core.Infrastructure;
using Voting.Core.Enums;

namespace Voting.Core.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _uow;
        private readonly IHubContext<QuestionHub, IQuestionHub> _hubContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public QuestionService(
            IUnitOfWork uow,
            IHubContext<QuestionHub, IQuestionHub> hubContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _uow = uow;
            _hubContext = hubContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PageResult<QuestionDTO>> GetAllQuestionsAsync(int? page, int pageSize)
        {
            var questions = await _uow.QuestionRepository.GetAllQuestions((page - 1 ?? 0) * pageSize, pageSize);
            return new PageResult<QuestionDTO>()
            {
                Count = await _uow.QuestionRepository.Get("").Where(x => x.IsActive == true).CountAsync(),
                PageIndex = page ?? 1,
                PageSize = pageSize,
                Items = questions.Select(q => new QuestionDTO()
                {
                    Id = q.Id,
                    Title = q.Title,
                    Description = q.Description,
                    PublishDate = q.PublishDate,
                    ExpiryDate = q.ExpiryDate,
                    IsActive = q.IsActive,
                    TotalCount = q.Votes.Count(),
                    Answers = (from a in q.Answers
                               join v in q.Votes on a.Id equals v.AnswerId into g
                               select new AnswerDTO()
                               {
                                   Answer = a.Text,
                                   Count = g.Count(),
                                   Voted = g.Where(x => x.UserId == _httpContextAccessor.HttpContext.User.Identity.Name).FirstOrDefault() != null
                               }).ToList()
                }).ToList()
            };
        }
        public async Task<QuestionDTO> GetQuestionAsync(int questionId)
        {
            return await _uow.QuestionRepository.Get("")
                .Where(x => x.Id == questionId && x.IsActive == true)
                .Select(q => new QuestionDTO()
                {
                    Id = q.Id,
                    Title = q.Title,
                    Description = q.Description,
                    PublishDate = q.PublishDate,
                    ExpiryDate = q.ExpiryDate
                })
                .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<QuestionDTO>> GetAllQuestionsByUserAsync(string userId)
        {
            return await _uow.QuestionRepository.Get("")
                .Where(x => x.OwnerId == userId)
                .Select(x => new QuestionDTO()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    ExpiryDate = x.ExpiryDate
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<UserVoteDTO>> GetAllVotesByUserAsync()
        {
            var votes = await _uow.VoteRepository.Get("Answer")
                .Where(x => x.UserId == _httpContextAccessor.HttpContext.User.Identity.Name)
                .OrderByDescending(o => o.VoteDate)
                .Select(s => new UserVoteDTO
                {
                    Id = s.Id,
                    VoteDate = s.VoteDate,
                    QuestionId = s.QuestionId,
                    QuestionTitle = s.Question.Title,
                    Answer = s.Answer.Text
                })
                .ToListAsync();
            
            return votes;
        }
        public async Task<OperationResult> VoteForQuestionAsync(int questionId, string selectedAnswer)
        {
            try
            {
                var question = _uow.QuestionRepository.Get(questionId);
                if (question.ExpiryDate < DateTime.Now)
                    return new OperationResult(false, "Invalid operation", "");

                var answer = await _uow.AnswerRepository.Get("")
                    .Where(x => x.QuestionId == questionId && x.Text == selectedAnswer)
                    .FirstOrDefaultAsync();

                var voted = await _uow.VoteRepository.Get("Answer")
                    .Where(x => x.QuestionId == questionId && x.UserId == _httpContextAccessor.HttpContext.User.Identity.Name)
                    .FirstOrDefaultAsync();

                if (voted == null)
                {
                    var newVote = new Vote()
                    {
                        Answer = answer,
                        VoteDate = DateTime.Now,
                        UserId = _httpContextAccessor.HttpContext.User.Identity.Name,
                        QuestionId = questionId
                    };
                    _uow.VoteRepository.Add(newVote);
                    await _uow.SaveAsync();

                    await _hubContext.Clients.All.VotedForQuestion(new VoteDTO()
                    {
                        Type = true,
                        QuestionId = questionId,
                        NewAnswer = answer.Text
                    });

                    return new OperationResult(true);
                }
                else 
                {
                    if (selectedAnswer == voted.Answer.Text)
                        return new OperationResult(true);

                    string oldAnswer = voted.Answer.Text;
                    voted.AnswerId = answer.Id;
                    voted.VoteDate = DateTime.Now;
                    _uow.VoteRepository.Update(voted);
                    await _uow.SaveAsync();

                    await _hubContext.Clients.All.VotedForQuestion(new VoteDTO()
                    {
                        Type = false,
                        QuestionId = questionId,
                        OldAnswer = oldAnswer,
                        NewAnswer = answer.Text
                    });

                    return new OperationResult(true);
                }
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, "");
            }
        }
        public async Task<OperationResult> AddQuestionByUserAsync(Question question)
        {
            try
            {
                if (question.Answers.Count() < 2 || question.Answers.All(item => string.IsNullOrWhiteSpace(item.Text)))
                    return new OperationResult(false, "Answers is null or contain only white spaces", "");

                question.OwnerId = _httpContextAccessor.HttpContext.User.Identity.Name;
                _uow.QuestionRepository.Add(question);
                await _uow.SaveAsync();
                return new OperationResult(true, "", "");
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, "");
            }
        }
        public async Task<OperationResult> DeleteQuestionAsync(int questionId)
        {
            var question = _uow.QuestionRepository.Get(questionId);
            if (question == null)
                return new OperationResult(false, "Not found", "");

            _uow.QuestionRepository.Delete(question);
            await _uow.SaveAsync();

            return new OperationResult(true);
        }

        public async Task<PageResult<QuestionDTO>> GetQuestionsForAdminAsync(int? page, int pageSize)
        {
            return new PageResult<QuestionDTO>()
            {
                Count = await _uow.QuestionRepository.Get("").CountAsync(),
                PageIndex = page ?? 1,
                PageSize = pageSize,
                Items = await _uow.QuestionRepository.Get("")
                                    .AsNoTracking()
                                    .OrderBy(x => x.IsActive)
                                    .ThenByDescending(x => x.PublishDate)
                                    .Skip((page - 1 ?? 0) * pageSize)
                                    .Take(pageSize)
                                    .Select(x => new QuestionDTO()
                                    {
                                        Id = x.Id,
                                        Title = x.Title,
                                        Description = x.Description,
                                        PublishDate = x.PublishDate,
                                        ExpiryDate = x.ExpiryDate,
                                        IsActive = x.IsActive
                                    })
                                    .ToListAsync()
            };
        }
        public async Task<QuestionDTO> GetQuestionForAdminAsync(int questionId)
        {
            var question = await _uow.QuestionRepository.GetQuestion(questionId);
            return question.Select(q => new QuestionDTO()
            {
                Id = q.Id,
                Title = q.Title,
                Description = q.Description,
                PublishDate = q.PublishDate,
                ExpiryDate = q.ExpiryDate,
                IsActive = q.IsActive,
                TotalCount = q.Votes.Count(),
                Answers = (from a in q.Answers
                           join v in q.Votes on a.Id equals v.AnswerId into g
                           select new AnswerDTO()
                           {
                               Answer = a.Text,
                               Count = g.Count()
                           }).ToList()
            })
            .FirstOrDefault();
        }
        public async Task<OperationResult> AddQuestionByAdminAsync(Question question)
        {
            try
            {
                question.IsActive = true;
                question.OwnerId = _httpContextAccessor.HttpContext.User.Identity.Name;
                _uow.QuestionRepository.Add(question);
                await _uow.SaveAsync();

                await _hubContext.Clients.All.QuestionAdded(new QuestionDTO()
                {
                    Id = question.Id,
                    Title = question.Title,
                    Description = question.Description,
                    PublishDate = question.PublishDate,
                    ExpiryDate = question.ExpiryDate,
                    IsActive = question.IsActive,
                    TotalCount = 0,
                    Answers = question.Answers.Select(x => new AnswerDTO()
                    { 
                        Answer = x.Text
                    }).ToList()
                });

                return new OperationResult(true);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, "");
            }
        }
        public async Task<OperationResult> UpdateQuestionByAdminAsync(Question question)
        {
            try
            {
                var votes = await _uow.VoteRepository.Get("")
                                        .Where(x => x.QuestionId == question.Id)
                                        .ToListAsync();
                if (votes.Count() > 0)
                    return new OperationResult(false);

                var answers = await _uow.AnswerRepository.Get("")
                                        .Where(x => x.QuestionId == question.Id)
                                        .ToListAsync();
                foreach (var answer in answers)
                    _uow.AnswerRepository.Delete(answer);

                question.IsActive = true;
                question.OwnerId = _httpContextAccessor.HttpContext.User.Identity.Name;
                _uow.QuestionRepository.Update(question);
                await _uow.SaveAsync();
                return new OperationResult(true);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, "");
            }
        }
        public async Task<OperationResult> ChangeQuestionStatusAsync(int id, Status status)
        {
            try
            {
                var question = _uow.QuestionRepository.Get(id);
                if (question == null)
                    return new OperationResult(false);

                question.IsActive = Status.Block != status;
                _uow.QuestionRepository.Update(question);
                await _uow.SaveAsync();

                return new OperationResult(true);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, "");
            }
        }
        public async Task<OperationResult> DeleteQuestionByAdminAsync(int questionId)
        {
            try
            {
                var question = _uow.QuestionRepository.Get("Votes").Where(x => x.Id == questionId).FirstOrDefault();

                if (question.Votes.Count() > 0)
                    return new OperationResult(false, "Invalid operation", "");

                if (question == null)
                    return new OperationResult(false, "Not found", "");

                _uow.QuestionRepository.Delete(question);
                await _uow.SaveAsync();
                await _hubContext.Clients.All.QuestionDeleted(question.Id);

                return new OperationResult(true);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, "");
            }
        }
    }
}