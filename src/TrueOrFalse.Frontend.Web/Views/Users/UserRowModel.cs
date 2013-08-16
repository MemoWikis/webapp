using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seedworks.Lib;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web.Context;

public class UserRowModel
{
    public int Id;
    public string Name;
    public string DescriptionShort;

    public int IndexInResult;

    public int QuestionCount;

    public Func<UrlHelper, string> DetailLink;
    public Func<UrlHelper, string> UserLink;

    public string ImageUrl;

    public string CreatorName;
    public int CreatorId;

    public bool IsInstallationLogin;
    public bool IsCurrentUser;
    public bool AllowsSupportiveLogin;

    public UserRowModel(User user, int indexInResultSet, SessionUser sessionUser)
    {
        Id = user.Id;
        Name = user.Name;

        IsCurrentUser = Id == sessionUser.User.Id;
        IsInstallationLogin = sessionUser.IsInstallationAdmin;
        AllowsSupportiveLogin = user.AllowsSupportiveLogin;

        DescriptionShort = "";

        IndexInResult = indexInResultSet;

        UserLink = urlHelper => Links.UserDetail(urlHelper, user.Name, user.Id);

        ImageUrl = new QuestionSetImageSettings(user.Id).GetUrl_206px_square().Url;
        ImageUrl = new UserImageSettings(user.Id).GetUrl_85px_square(user.EmailAddress).Url;
    }

}