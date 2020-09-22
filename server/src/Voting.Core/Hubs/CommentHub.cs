using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Voting.Core.DTOs.Comment;
using Voting.Core.DTOs.Question;
using Voting.Core.Entities;
using Voting.Core.Interfaces.Hubs;
using Voting.Core.Interfaces.Services;

namespace Voting.Core.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CommentHub : Hub<ICommentHub>
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public CommentHub(ICommentService commentService, IMapper mapper)
        {
            _commentService = commentService;
            _mapper = mapper;
        }

        public async Task AddComment(AddCommentDTO comment)
        {
            comment.UserId = Context.User.Identity.Name;
            var res = await _commentService.AddCommentAsync(_mapper.Map<Comment>(comment));
            if (!res.Successed)
                await Clients.Caller.Notify("Comment not added");

            var sComment = _mapper.Map<CommentDTO>(await _commentService.GetCommentAsync(Convert.ToInt32(res.Property)));
            if (sComment != null)
            {
                sComment.IsMine = false;
                await Clients.Others.AddCommentAsync(sComment);
                sComment.IsMine = true;
                await Clients.Caller.AddCommentAsync(sComment);
            }
        }
        public async Task UpdateComment(AddCommentDTO commentDTO)
        {
            var res = await _commentService.UpdateCommentAsync(_mapper.Map<Comment>(commentDTO));
            if (!res.Successed)
                await Clients.Caller.Notify("Comment not updated");

            var comment = _mapper.Map<CommentDTO>(await _commentService.GetCommentAsync(Convert.ToInt32(res.Property)));
            if (comment != null)
            {
                comment.IsMine = false;
                await Clients.Others.UpdateComment(comment);
                comment.IsMine = true;
                await Clients.Caller.UpdateComment(comment);
            }
        }
    }
}