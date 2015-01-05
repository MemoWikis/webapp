using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TrueOrFalse;
using TrueOrFalse.Web;

public class UserSettingsModel : BaseModel
{
    public UIMessage Message;

    [Required(ErrorMessage = "Bitte gib einen Nutzernamen an.")]
    [DisplayName("Nutzername")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Wir benötigen deine E-Mail Adresse.")]
    [RegularExpression(Regexes.Email, ErrorMessage = "Wir benötigen eine gültige E-Mail Adresse.")]
    [DisplayName("Email")]
    public string Email { get; set; }

    public bool AllowsSupportiveLogin { get; set; }

    public string ImageUrl_200;
    public bool ImageIsCustom;

    public int ReputationRank;
    public int ReputationTotal;

    public UserSettingsModel(){}

    public UserSettingsModel(User user)
    {
        Name = user.Name;
        Email = user.EmailAddress;
        AllowsSupportiveLogin = user.AllowsSupportiveLogin;

        var reputation = Resolve<ReputationCalc>().Run(user);
        ReputationRank = user.ReputationPos;
        ReputationTotal = reputation.TotalRepuation;
    }
}