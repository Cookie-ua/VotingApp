using System;
using System.Collections.Generic;
using System.Text;

namespace Voting.Core.Infrastructure
{
    public class PageResult<T> where T: class
    {
        public int Count { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<T> Items { get; set; }
    }
}