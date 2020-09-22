using System;
using System.Collections.Generic;
using System.Text;

namespace Voting.Core.DTOs.Comment
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime PublishDate { get; set; }
        public bool IsMine { get; set; }
        public bool IsBlocked { get; set; }
        public int? CommentId { get; set; }
        public int QuestionId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int AnswersCount { get; set; }
    }
}