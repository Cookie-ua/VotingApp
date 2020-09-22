using Microsoft.AspNetCore.SignalR;
using Voting.Core.Interfaces.Hubs;

namespace Voting.Core.Hubs
{
    public class QuestionHub : Hub<IQuestionHub>
    {
    }
}
