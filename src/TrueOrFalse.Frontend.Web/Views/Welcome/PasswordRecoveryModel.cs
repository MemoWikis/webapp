using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TrueOrFalse;
using TrueOrFalse.Web;

public class PasswordRecoveryModel
{
    public UIMessage Message;

    [Required(ErrorMessage = "Wir benötigen die Email-Adresse, mit der du dich angemeldet hast.")]
    [RegularExpression(Regexes.Email, ErrorMessage = "Bitte gib eine gültige Email-Adresse an.")]
    [DisplayName("Email")]
    public string Email { get; set; }
}