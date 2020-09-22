using System;
using System.Collections.Generic;

namespace Voting.Core.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool? IsActive { get; set; }

        public string OwnerId { get; set; }
        public User Owner { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<Vote> Votes { get; set; }
        public IEnumerable<Answer> Answers { get; set; }
    }
}
