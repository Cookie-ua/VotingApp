using System.ComponentModel.DataAnnotations;

namespace Voting.Api.Models.Admin
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
