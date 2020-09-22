using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voting.Core.Entities;
using Voting.Core.Interfaces;
using Voting.Data.EF;

namespace Voting.Data.Repositories
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        public QuestionRepository(ApplicationDbContext db) : base(db) {}

        public async Task<IEnumerable<Question>> GetQuestion(int id)
        {
            return await _context.Questions
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Include(x => x.Answers)
                .Include(x => x.Votes)
                .ThenInclude(x => x.Answer)
                .Select(x => new Question()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    PublishDate = x.PublishDate,
                    ExpiryDate = x.ExpiryDate,
                    IsActive = x.IsActive,
                    Answers = x.Answers.ToList(),
                    Votes = x.Votes.ToList()
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<Question>> GetAllQuestions(int skip, int take)
        {
            return await _context.Questions
                .AsNoTracking()
                .Where(x => x.IsActive == true)
                .Include(x => x.Answers)
                .Include(x => x.Votes)
                .ThenInclude(x => x.Answer)
                .Skip(skip)
                .Take(take)
                .OrderByDescending(o => o.ExpiryDate)
                .Select(x => new Question()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    PublishDate = x.PublishDate,
                    ExpiryDate = x.ExpiryDate,
                    IsActive = x.IsActive,
                    Answers = x.Answers.ToList(),
                    Votes = x.Votes.ToList()
                })
                .ToListAsync();
        }
    }
}