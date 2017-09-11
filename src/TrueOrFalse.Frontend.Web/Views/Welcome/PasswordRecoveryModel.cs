using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TrueOrFalse;
using TrueOrFalse.Web;

public class PasswordRecoveryModel : BaseModel
{
    public UIMessage Message;

    [Required(ErrorMessage = "Wir benötigen die E-Mail-Adresse, mit der du dich registriert hast.")]
    [RegularExpression(Regexes.Email, ErrorMessage = "Bitte gib eine gültige E-Mail-Adresse an.")]
    [DisplayName("E-Mail")]
    public string Email { get; set; }
}