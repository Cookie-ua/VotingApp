using System;
using System.Collections.Generic;
using System.Text;
using Voting.Core.Entities;
using Voting.Core.Interfaces;
using Voting.Data.EF;

namespace Voting.Data.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
