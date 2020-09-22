using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Voting.Core.Entities;
using Voting.Core.Interfaces;
using Voting.Core.Interfaces.Repositories;
using Voting.Data.EF;

namespace Voting.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        private IQuestionRepository _questionRepository;
        private IAnswerRepository _answerRepository;
        private ICommentRepository _commentRepository;
        private IVoteRepository _voteRepository;
        private IUserRepository _userRepository;

        public UnitOfWork(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IQuestionRepository QuestionRepository => _questionRepository ??= new QuestionRepository(_context);
        public IAnswerRepository AnswerRepository => _answerRepository ??= new AnswerRepository(_context);
        public ICommentRepository CommentRepository => _commentRepository ??= new CommentRepository(_context);
        public IVoteRepository VoteRepository => _voteRepository ??= new VoteRepository(_context);
        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context, _userManager);

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                }
                this.disposed = true;
            }
        }
    }
}
