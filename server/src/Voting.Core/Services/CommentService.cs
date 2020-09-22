using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Voting.Core.DTOs.Comment;
using Voting.Core.DTOs.Question;
using Voting.Core.Entities;
using Voting.Core.Enums;
using Voting.Core.Hubs;
using Voting.Core.Infrastructure;
using Voting.Core.Interfaces;
using Voting.Core.Interfaces.Hubs;
using Voting.Core.Interfaces.Services;

namespace Voting.Core.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _uow;
        private readonly IHubContext<CommentHub, ICommentHub> _hubContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommentService(
            IUnitOfWork uow, 
            IHubContext<CommentHub, ICommentHub> hubContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _uow = uow;
            _hubContext = hubContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CommentDTO> GetCommentAsync(int id)
        {
            return await _uow.CommentRepository.Get("")
                .Where(x => x.Id == id && x.IsBlocked == false)
                .Select(c => new CommentDTO()
                {
                    Id = c.Id,
                    Message = c.Message,
                    PublishDate = c.PublishDate,
                    CommentId = c.CommentId,
                    QuestionId = c.QuestionId,
                    UserId = c.UserId,
                    UserName = c.User.UserName,
                })
                .FirstOrDefaultAsync();
        }
        public async Task<PageResult<CommentDTO>> GetAllCommentsAsync(int? page, int questionId, int pageSize = 8)
        {
            return new PageResult<CommentDTO>()
            {
                Count = await _uow.CommentRepository.Get("")
                                                    .Where(x => x.QuestionId == questionId && x.IsBlocked == false && x.CommentId == null)
                                                    .CountAsync(),
                PageIndex = page ?? 1,
                PageSize = pageSize,
                Items = await _uow.CommentRepository.Get("")
                                    .Where(x => x.QuestionId == questionId && x.IsBlocked == false && x.CommentId == null)
                                    .OrderByDescending(x => x.PublishDate)
                                    .Skip((page - 1 ?? 0) * pageSize)
                                    .Take(pageSize)
                                    .Select(c => new CommentDTO()
                                    {
                                        Id = c.Id,
                                        Message = c.Message,
                                        PublishDate = c.PublishDate,
                                        AnswersCount = c.Comments.Count(),
                                        QuestionId = c.QuestionId,
                                        UserId = c.UserId,
                                        UserName = c.User.UserName,
                                        IsMine = _httpContextAccessor.HttpContext.User.Identity.Name == c.UserId
                                    })
                                    .ToListAsync()
            };
        }
        public async Task<IEnumerable<CommentDTO>> GetAllChildCommentsAsync(int commentId)
        {
            return await _uow.CommentRepository.Get("")
                                    .Where(x => x.IsBlocked == false && x.CommentId == commentId)
                                    .OrderByDescending(x => x.PublishDate)
                                    .Select(c => new CommentDTO()
                                    {
                                        Id = c.Id,
                                        Message = c.Message,
                                        PublishDate = c.PublishDate,
                                        AnswersCount = c.Comments.Count(),
                                        UserId = c.UserId,
                                        UserName = c.User.UserName,
                                        IsMine = _httpContextAccessor.HttpContext.User.Identity.Name == c.UserId
                                    })
                                    .ToListAsync();
        }
        public async Task<IEnumerable<CommentDTO>> GetAllCommentsByUserAsync(string userId)
        {
            return await _uow.CommentRepository.Get("")
                .Where(x => x.UserId == userId)
                .OrderBy(o => o.QuestionId)
                .Select(c => new CommentDTO()
                {
                    Id = c.Id,
                    Message = c.Message,
                    PublishDate = c.PublishDate,
                    IsBlocked = c.IsBlocked,
                    QuestionId = c.QuestionId
                })
                .ToListAsync();
        }
        public async Task<OperationResult> AddCommentAsync(Comment comment)
        {
            try
            {
                var nComment = _uow.CommentRepository.Add(comment);
                await _uow.SaveAsync();

                return new OperationResult(true, "", nComment.Id.ToString());
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, "");
            }
        }
        public async Task<OperationResult> UpdateCommentAsync(Comment comment)
        {
            try
            {
                var updatedComment = _uow.CommentRepository.Get("")
                                            .Where(x => x.Id == comment.Id)
                                            .FirstOrDefault();

                updatedComment.Message = comment.Message;
                _uow.CommentRepository.Update(updatedComment);
                await _uow.SaveAsync();

                return new OperationResult(true, "", comment.Id.ToString());
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, "");
            }
        }
        public async Task<OperationResult> DeleteCommentAsync(int id)
        {
            try
            {
                var comment = await _uow.CommentRepository.Get("Comments")
                                        .Where(x => x.Id == id)
                                        .FirstOrDefaultAsync();
                
                if (comment == null)
                    return new OperationResult(false, "Not found", "");

                if (comment.Comments != null)
                    foreach (var childComennt in comment.Comments)
                       _uow.CommentRepository.Delete(childComennt);

                _uow.CommentRepository.Delete(comment);
                await _uow.SaveAsync();

                await _hubContext.Clients.All.CommentDeletedAsync(comment.Id, comment.CommentId);

                return new OperationResult(true);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, "");
            }
        }
        public async Task<OperationResult> ChangeCommentStatusAsync(int id, Status status)
        {
            try
            {
                var comment = _uow.CommentRepository.Get(id);
                if (comment == null)
                    return new OperationResult(false);

                comment.IsBlocked = Status.Block == status;
                _uow.CommentRepository.Update(comment);
                await _uow.SaveAsync();

                return new OperationResult(true);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, "");
            }
        }
    }
}