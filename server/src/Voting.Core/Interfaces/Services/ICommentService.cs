using System.Collections.Generic;
using System.Threading.Tasks;
using Voting.Core.DTOs.Comment;
using Voting.Core.Entities;
using Voting.Core.Enums;
using Voting.Core.Infrastructure;

namespace Voting.Core.Interfaces.Services
{
    public interface ICommentService
    {
        Task<CommentDTO> GetCommentAsync(int id);
        Task<PageResult<CommentDTO>> GetAllCommentsAsync(int? page, int questionId, int pageSize = 8);
        Task<IEnumerable<CommentDTO>> GetAllChildCommentsAsync(int commentId);
        Task<IEnumerable<CommentDTO>> GetAllCommentsByUserAsync(string userId);
        Task<OperationResult> AddCommentAsync(Comment comment);
        Task<OperationResult> UpdateCommentAsync(Comment comment);
        Task<OperationResult> DeleteCommentAsync(int id);
        Task<OperationResult> ChangeCommentStatusAsync(int id, Status status);
    }
}
