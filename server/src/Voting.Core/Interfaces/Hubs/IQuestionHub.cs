using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Voting.Core.DTOs.Question;

namespace Voting.Core.Interfaces.Hubs
{
    public interface IQuestionHub
    {
        Task QuestionAdded(QuestionDTO question);
        Task QuestionDeleted(int questionId);
        Task VotedForQuestion(VoteDTO voteDTO);
    }
}
