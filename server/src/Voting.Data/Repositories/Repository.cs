using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Voting.Core.Interfaces;
using Voting.Data.EF;

namespace Voting.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        private readonly DbSet<T> _entities;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }

        public IQueryable<T> Get(string includeProperties = "")
        {
            IQueryable<T> query = _entities;
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);
            
            return query;
        }
        public T Get(int id)
        {
            return _entities.Find(id);
        }
        public T Add(T entity)
        {
            _entities.Add(entity);
            return entity;
        }
        public T Update(T entity)
        {
            _entities.Update(entity);
            return entity;
        }
        public T Delete(T entity)
        {
            _entities.Remove(entity);
            return entity;
        }
    }
}
