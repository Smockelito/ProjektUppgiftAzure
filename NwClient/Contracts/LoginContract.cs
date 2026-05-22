using System.ComponentModel.DataAnnotations;

namespace NwClient.Contracts
{
    public class LoginContract
    {
        [Required(ErrorMessage = "E-post krävs")]
        [EmailAddress(ErrorMessage = "Ogiltig e-postadress")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Lösenord krävs")]
        public string Password { get; set; } = "";
    }
}
