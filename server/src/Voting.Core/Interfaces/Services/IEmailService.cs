using System.Threading.Tasks;
using Voting.Core.Entities;

namespace Voting.Core.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendMailAsync(Message message);
    }
}