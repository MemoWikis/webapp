using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TrueOrFalse;

public class RegisterModel : BaseModel
{
    [Required(ErrorMessage="Wir benötigen deinen Benutzernamen.")]
        
    [StringLength(40, MinimumLength=4, ErrorMessage = "Minimum 4 Zeichen, Maximum 40 Zeichen. Sollte aus Vornamen und Nachnamen bestehen.")]
    [DisplayName("Benutzername")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Wir benötigen deine E-Mail Adresse.")]
    [RegularExpression(Regexes.Email, ErrorMessage = "Wir benötigen eine gültige E-Mail Adresse.")]
    [DisplayName("Email")]  
    public string Email { get; set; }

    [Required(ErrorMessage ="Ein Passwort ist Pflicht!")]
    [StringLength(40, MinimumLength = 5, ErrorMessage = "Mindestens 5 Zeichen und maximal 20 Zeichen.")]
    [DisplayName("Passwort")]

    public string Password { get; set; }

    //[DisplayName("Bitte bestätige unsere AGBs.")]
    //public bool TermsAndConditionsApproved { get; set; }
}
