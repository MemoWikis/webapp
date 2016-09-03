using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TrueOrFalse;
using TrueOrFalse.Web;

public class UserSettingsModel : BaseModel
{
    public UIMessage Message;

    [Required(ErrorMessage = "Bitte gib einen Nutzernamen an.")]
    [DisplayName("Nutzername")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Wir benötigen deine E-Mail-Adresse.")]
    [RegularExpression(Regexes.Email, ErrorMessage = "Wir benötigen eine gültige E-Mail-Adresse.")]
    [DisplayName("E-Mail")]
    public string Email { get; set; }

    public bool IsMember;
    public Membership Membership;

    public bool AllowsSupportiveLogin { get; set; }
    public bool ShowWishKnowledge { get; set; }

    public string ImageUrl_200;
    public ImageFrontendData ImageFrontendData;
    public bool ImageIsCustom;

    public UserSettingsModel(){}

    public UserSettingsModel(User user)
    {
        Name = user.Name;
        Email = user.EmailAddress;
        AllowsSupportiveLogin = user.AllowsSupportiveLogin;
        ShowWishKnowledge = user.ShowWishKnowledge;
        IsMember = user.IsMember();
        Membership = user.CurrentMembership();

        var imageResult = new UserImageSettings(user.Id).GetUrl_200px(user.EmailAddress);
        ImageUrl_200 = imageResult.Url;
        ImageIsCustom = imageResult.HasUploadedImage;

    }
}