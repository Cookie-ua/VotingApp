using System.Threading.Tasks;
using Voting.Core.Infrastructure;

namespace Voting.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<OperationResult> Authenticate(string email, string password);
    }
}