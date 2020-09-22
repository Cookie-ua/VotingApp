using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Voting.Api.Models.Question
{
    //public class VoteForQuestionViewModel
    //{
    //    public int QuestionId { get; set; }
    //    public bool Vote { get; set; }
    //}

    public class VoteForQuestionViewModel
    {
        public int QuestionId { get; set; }
        public string Answer { get; set; }
    }
}
