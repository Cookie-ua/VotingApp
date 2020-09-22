using System;
using System.Collections.Generic;
using System.Text;

namespace Voting.Core.DTOs.Question
{
    public class VoteDTO
    {
        public int QuestionId { get; set; }
        public string NewAnswer { get; set; }
        public string OldAnswer { get; set; }
        public bool Type { get; set; }
    }

    public class UserVoteDTO
    {
        public int Id { get; set; }
        public DateTime VoteDate { get; set; }
        public string Answer { get; set; }
        public int QuestionId { get; set; }
        public string QuestionTitle { get; set; }
    }
}