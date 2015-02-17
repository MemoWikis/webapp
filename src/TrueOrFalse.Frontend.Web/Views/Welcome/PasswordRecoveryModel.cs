using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TrueOrFalse;
using TrueOrFalse.Web;

public class PasswordRecoveryModel
{
    public UIMessage Message;

    [Required(ErrorMessage = "Wir benötigen deine E-Mail Adresse.")]
    [RegularExpression(Regexes.Email, ErrorMessage = "Wir benötigen eine gültige E-Mail Adresse.")]
    [DisplayName("Email")]
    public string Email { get; set; }
}