using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Voting.Api.Models.Admin
{
    public class ChangeUserInRoleViewModel
    {
        public string UserId { get; set; }
        public string Role { get; set; }
    }
}
