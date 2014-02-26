using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TrueOrFalse.Web;

public class PasswordResetModel
{
    public UIMessage Message;

    [Required(ErrorMessage = "* Pflichtfeld")]
    [DisplayName("Neues Password")]
    public string NewPassword1 { get; set; }

    [Required(ErrorMessage = "* Pflichtfeld")]
    [System.Web.Mvc.Compare("NewPassword1", ErrorMessage = "Die Passwörter stimmen nicht über ein.")]
    [DisplayName("Neues Password (bestätigen)")]
    public string NewPassword2 { get; set; }

    public string Token { get; set; }

    public bool TokenFound;
}
