﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TrueOrFalse;
using TrueOrFalse.Web;

public class UserSettingsModel : BaseModel
{
    public UIMessage Message;

    public bool IsMember;
    public IList<string> WidgetHosts;

    public string ImageUrl_200;
    public ImageFrontendData ImageFrontendData;
    public bool ImageIsCustom;

    public UserSettingsModel()
    {
    }

    public UserSettingsModel(UserCacheItem user)
    {
        if (user == null) // page is called if user changes notification settings via email; user needs not to be logged in
        {
            return;
        }

        Name = user.Name;
        Email = user.EmailAddress;
        IsMember = user.IsMember;
        WidgetHosts = user.WidgetHosts();
        AllowsSupportiveLogin = user.AllowsSupportiveLogin;
        ShowWishKnowledge = user.ShowWishKnowledge;
        KnowledgeReportInterval = user.KnowledgeReportInterval;

        var imageResult = new UserImageSettings(user.Id).GetUrl_200px(user);
        ImageUrl_200 = imageResult.Url;
        ImageIsCustom = imageResult.HasUploadedImage;
    }

    [Required(ErrorMessage = "Wir benötigen deine E-Mail-Adresse.")]
    [RegularExpression(Regexes.Email, ErrorMessage = "Wir benötigen eine gültige E-Mail-Adresse.")]
    [DisplayName("E-Mail")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Bitte gib einen Nutzernamen an.")]
    [DisplayName("Nutzername")]
    public string Name { get; set; }

    public bool AllowsSupportiveLogin { get; set; }
    public UserSettingNotificationInterval KnowledgeReportInterval { get; set; }
    public bool ShowWishKnowledge { get; set; }
}