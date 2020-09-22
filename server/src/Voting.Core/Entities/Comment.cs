using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Voting.Core.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime PublishDate { get; set; }
        public bool IsBlocked { get; set; }

        public int? CommentId { get; set; }
        public IEnumerable<Comment> Comments { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public string RecipientId { get; set; }
        [ForeignKey("RecipientId")]
        public User Recipient { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("Comments")]
        public User User { get; set; }
    }
}