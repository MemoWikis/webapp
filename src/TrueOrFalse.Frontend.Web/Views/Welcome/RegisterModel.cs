using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TrueOrFalse;

public class RegisterModel : BaseModel
{
    [DisplayName("Benutzername")]
    public string Name { get; set; }

    [DisplayName("Email")]  
    public string Email { get; set; }

    [DisplayName("Passwort")]
    public string Password { get; set; }

    [DisplayName("Ich akzeptiere die AGBs.")]
    public bool TermsAndConditionsApproved { get; set; }
}
