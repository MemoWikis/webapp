using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TrueOrFalse.Core.Web;

public class PasswordResetModel
{
    public Message Message;

    [Required(ErrorMessage = "* Pflichtfeld")]
    public string NewPassword1 { get; set; }

    [Required(ErrorMessage = "* Pflichtfeld")]
    [Compare("NewPassword1", ErrorMessage = "Die Passwörter stimmen nicht über ein.")]
    public string NewPassword2 { get; set; }

    public string Token { get; set; }

    public bool TokenFound;
}
