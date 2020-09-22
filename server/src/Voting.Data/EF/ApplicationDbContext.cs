using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Voting.Core.Entities;

namespace Voting.Data.EF
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Vote> Votes { get; set; }
    }
}
