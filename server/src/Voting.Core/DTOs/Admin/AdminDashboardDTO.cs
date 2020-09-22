using System;
using System.Collections.Generic;
using System.Text;

namespace Voting.Core.DTOs.Admin
{
    public class AdminDashboardDTO
    {
        public int QuestionCount { get; set; }
        public int UserCount { get; set; }
        public int VoteCount { get; set; }
    }

    public class InfoForAdminDTO
    {
        public List<QuestionInfo> QuestionInfo { get; set; }
    }

    public class QuestionInfo
    {
        public bool? Type { get; set; }
        public int Count { get; set; }
    }
}