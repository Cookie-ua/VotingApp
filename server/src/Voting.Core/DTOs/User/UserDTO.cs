using System;
using System.Collections.Generic;
using System.Text;

namespace Voting.Core.DTOs.User
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool IsBlocked { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}