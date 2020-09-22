using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Voting.Core.Enums;

namespace Voting.Api.Models.Admin
{
    public class ChangeStatusViewModel
    {
        public string UserId { get; set; }
        public Status Status { get; set; }
    }
}
