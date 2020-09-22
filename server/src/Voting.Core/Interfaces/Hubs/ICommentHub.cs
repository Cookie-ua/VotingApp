using System.Threading.Tasks;
using Voting.Core.DTOs.Comment;

namespace Voting.Core.Interfaces.Hubs
{
    public interface ICommentHub
    {
        Task Notify(string message);
        Task AddCommentAsync(CommentDTO comment);
        Task UpdateComment(CommentDTO comment);
        Task CommentDeletedAsync(int id, int? parentId);
        Task BlockCommentAsync(int id);
        Task UnblockCommentAsync(int id);
    }
}
