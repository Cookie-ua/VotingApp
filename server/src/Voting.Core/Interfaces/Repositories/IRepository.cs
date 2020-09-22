using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voting.Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);
        IQueryable<T> Get(string includeProperties = "");
        T Add(T entity);
        T Update(T entity);
        T Delete(T entity);
    }
}
