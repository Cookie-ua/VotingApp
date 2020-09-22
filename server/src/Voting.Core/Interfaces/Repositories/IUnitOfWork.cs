using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Voting.Core.Interfaces.Repositories;

namespace Voting.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IQuestionRepository QuestionRepository { get; }
        IAnswerRepository AnswerRepository { get; }
        ICommentRepository CommentRepository { get; }
        IVoteRepository VoteRepository { get; }
        IUserRepository UserRepository { get; }

        Task SaveAsync();
    }
}
