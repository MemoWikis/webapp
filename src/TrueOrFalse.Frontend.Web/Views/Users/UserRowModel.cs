using System;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class UserRowModel
{
    public int Id;
    public string Name;

    public int Rank;
    public int Reputation;

    public int WishCountQuestions;
    public int WishCountSets;

    public int CreatedQuestions;
    public int CreatedSets;

    public string DescriptionShort;

    public int IndexInResult;

    public Func<UrlHelper, string> DetailLink;
    public Func<UrlHelper, string> UserLink;

    public string ImageUrl;

    public string CreatorName;
    public int CreatorId;

    public bool IsInstallationLogin;
    public bool IsCurrentUser;
    public bool AllowsSupportiveLogin;
    public bool ShowWishKnowlede;
    
    public UserRowModel(User user, int indexInResultSet, SessionUser sessionUser)
    {
        Id = user.Id;
        Name = user.Name;

        Reputation = user.Reputation;
        Rank = user.ReputationPos;

        WishCountQuestions = user.WishCountQuestions;
        WishCountSets = user.WishCountSets;

        CreatedQuestions = Sl.R<UserSummary>().AmountCreatedQuestions(user.Id);
        CreatedSets = Sl.R<UserSummary>().AmountCreatedSets(user.Id);

        IsCurrentUser = Id == sessionUser.UserId;
        IsInstallationLogin = sessionUser.IsInstallationAdmin;
        AllowsSupportiveLogin = user.AllowsSupportiveLogin;
        ShowWishKnowlede = user.ShowWishKnowledge;

        DescriptionShort = "";

        IndexInResult = indexInResultSet;

        UserLink = urlHelper => Links.UserDetail(urlHelper, user.Name, user.Id);

        ImageUrl = new UserImageSettings(user.Id).GetUrl_128px_square(user.EmailAddress).Url;
    }

}