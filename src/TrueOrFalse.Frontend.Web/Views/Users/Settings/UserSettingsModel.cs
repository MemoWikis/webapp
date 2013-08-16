using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Models;
using TrueOrFalse.Web;

public class UserSettingsModel : BaseModel
{
    public Message Message;

    [Required(ErrorMessage = "Bitte gib einen Nutzernamen an.")]
    [DisplayName("Nutzername")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Wir benötigen Deine E-Mail Adresse.")]
    [RegularExpression(Regexes.Email, ErrorMessage = "Wir benötigen eine gültige E-Mail Adresse.")]
    [DisplayName("Email")]
    public string Email { get; set; }

    public string ImageUrl_200;
    public bool ImageIsCustom;

    public UserSettingsModel(){}

    public UserSettingsModel(User user)
    {
        Name = user.Name;
        Email = user.EmailAddress;
    }
}