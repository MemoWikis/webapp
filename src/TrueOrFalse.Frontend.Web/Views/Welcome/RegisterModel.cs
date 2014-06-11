using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TrueOrFalse;

public class RegisterModel : BaseModel
{
    [Required(ErrorMessage="Wir benötigen Deinen Benutzernamen.")]
        
    [StringLength(20, MinimumLength=4, ErrorMessage = "Sollte aus Vornamen und Nachnamen bestehen. ")]
    [DisplayName("Ihre Name")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Wir benötigen Deine E-Mail Adresse.")]
    [RegularExpression(Regexes.Email, ErrorMessage = "Wir benötigen eine gültige E-Mail Adresse.")]
    [DisplayName("Email")]  
    public string Email { get; set; }

    [Required(ErrorMessage ="Ein Passwort ist Pflicht!")]
    [StringLength(20, MinimumLength = 5, ErrorMessage = "Mind. 5 Zeichen kurz und maximal 20 Zeichen lang sein.")]
    [DisplayName("Password")]

    public string Password { get; set; }

    [DisplayName("AGB Bestätigen: [TODO erstellen und verlinken]")]
    public bool TermsAndConditionsApproved { get; set; }
}
