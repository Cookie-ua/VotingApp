using Voting.Core.Enums;

namespace Voting.Api.Models.Question
{
    public class ChangeStatusViewModel
    {
        public int Id { get; set; }
        public Status Status { get; set; }
    }
}