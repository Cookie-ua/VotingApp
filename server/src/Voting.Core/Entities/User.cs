using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Voting.Core.Entities
{
    public class User : IdentityUser
    {
        public bool IsBlocked { get; set; }
        public DateTime RegisterDate { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}
