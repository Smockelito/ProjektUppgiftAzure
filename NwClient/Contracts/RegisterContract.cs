using System.ComponentModel.DataAnnotations;

namespace NwClient.Contracts
{
    public class RegisterContract
    {
        [Required(ErrorMessage = "E-post krävs")]
        [EmailAddress(ErrorMessage = "Ogiltig e-postadress")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Lösenord krävs")]
        [MinLength(6, ErrorMessage = "Lösenordet måste vara minst 6 tecken")]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Bekräfta lösenordet")]
        [Compare("Password", ErrorMessage = "Lösenorden matchar inte")]
        public string ConfirmPassword { get; set; } = "";
    }
}
