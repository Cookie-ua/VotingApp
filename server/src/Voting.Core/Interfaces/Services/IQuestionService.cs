using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Voting.Core.DTOs.Question;
using Voting.Core.Entities;
using Voting.Core.Enums;
using Voting.Core.Infrastructure;

namespace Voting.Core.Interfaces.Services
{
    public interface IQuestionService
    {
        Task<PageResult<QuestionDTO>> GetAllQuestionsAsync(int? page, int pageSize);
        Task<QuestionDTO> GetQuestionAsync(int questionId);
        Task<IEnumerable<QuestionDTO>> GetAllQuestionsByUserAsync(string userId);
        Task<IEnumerable<UserVoteDTO>> GetAllVotesByUserAsync();
        Task<OperationResult> VoteForQuestionAsync(int questionId, string selectedAnswer);
        Task<OperationResult> AddQuestionByUserAsync(Question question);
        Task<OperationResult> DeleteQuestionAsync(int questionId);


        Task<PageResult<QuestionDTO>> GetQuestionsForAdminAsync(int? page, int pageSize);
        Task<QuestionDTO> GetQuestionForAdminAsync(int questionId);
        Task<OperationResult> AddQuestionByAdminAsync(Question question);
        Task<OperationResult> UpdateQuestionByAdminAsync(Question question);
        Task<OperationResult> ChangeQuestionStatusAsync(int id, Status status);
        Task<OperationResult> DeleteQuestionByAdminAsync(int questionId);
    }
}