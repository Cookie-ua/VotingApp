using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Voting.Core.Entities;

namespace Voting.Core.Interfaces
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Task<IEnumerable<Question>> GetQuestion(int id);
        Task<IEnumerable<Question>> GetAllQuestions(int skip, int take);
    }
}