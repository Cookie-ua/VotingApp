using System;
using System.Collections.Generic;
using System.Text;

namespace Voting.Core.DTOs.Comment
{
    public class AddCommentDTO
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int? CommentId { get; set; }
        public int QuestionId { get; set; }
        public string UserId { get; set; }
        public string RecipientId { get; set; }
    }
}