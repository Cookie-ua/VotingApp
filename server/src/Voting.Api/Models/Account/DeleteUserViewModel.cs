using System.ComponentModel.DataAnnotations;

namespace Voting.Api.Models.Account
{
    public class DeleteUserViewModel
    {
        [Required]
        public string Password { get; set; }
    }
}