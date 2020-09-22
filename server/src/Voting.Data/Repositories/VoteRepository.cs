using System;
using System.Collections.Generic;
using System.Text;
using Voting.Core.Entities;
using Voting.Core.Interfaces;
using Voting.Data.EF;

namespace Voting.Data.Repositories
{
    public class VoteRepository : Repository<Vote>, IVoteRepository
    {
        public VoteRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
