using System;
using System.Collections.Generic;
using System.Text;
using Voting.Core.Entities;
using Voting.Core.Interfaces.Repositories;
using Voting.Data.EF;

namespace Voting.Data.Repositories
{
    public class AnswerRepository : Repository<Answer>, IAnswerRepository
    {
        public AnswerRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
