using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TrueOrFalse;

public class RegisterModel : BaseModel
{
    //Validation serves as backup for client side validation

    [Required(ErrorMessage="Wir benötigen einen Benutzernamen für dich.")]
    [StringLength(40, MinimumLength=2, ErrorMessage = "Minimum 2 Zeichen, Maximum 40 Zeichen.")]
    [DisplayName("Benutzername")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Wir benötigen deine E-Mail-Adresse.")]
    [RegularExpression(Regexes.Email, ErrorMessage = "Wir benötigen eine gültige E-Mail-Adresse.")]
    [DisplayName("E-Mail")]  
    public string Login { get; set; }

    [Required(ErrorMessage ="Ein Passwort ist Pflicht!")]
    [StringLength(40, MinimumLength = 5, ErrorMessage = "Mindestens 5 Zeichen und maximal 40 Zeichen.")]
    [DisplayName("Passwort")]

    public string Password { get; set; }
}
