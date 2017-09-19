using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TrueOrFalse.Web;

public class PasswordResetModel : BaseModel
{
    public UIMessage Message;

    [Required(ErrorMessage = "* Pflichtfeld")]
    [StringLength(40, MinimumLength = 5, ErrorMessage = "Mindestens 5 Zeichen und maximal 40 Zeichen.")]
    [DisplayName("Neues Password")]
    public string NewPassword1 { get; set; }

    [Required(ErrorMessage = "* Pflichtfeld")]
    [Compare("NewPassword1", ErrorMessage = "Die Passwörter stimmen nicht über ein.")]
    [DisplayName("Neues Password (bestätigen)")]
    public string NewPassword2 { get; set; }

    public string Token { get; set; }

    public bool TokenFound;
}