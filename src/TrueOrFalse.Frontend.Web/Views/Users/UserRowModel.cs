using System;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class UserRowModel : BaseModel
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
    public bool IsMember;

    public bool DoIFollow;
    
    public UserRowModel(
        User user, 
        int indexInResultSet, 
        FollowerIAm followerIAm)
    {
        Id = user.Id;
        Name = user.Name;

        Reputation = user.Reputation;
        Rank = user.ReputationPos;

        WishCountQuestions = user.WishCountQuestions;
        WishCountSets = user.WishCountSets;

        CreatedQuestions = Sl.R<UserSummary>().AmountCreatedQuestions(user.Id, false);
        CreatedSets = Sl.R<UserSummary>().AmountCreatedSets(user.Id);

        IsCurrentUser = Id == _sessionUser.UserId;
        IsInstallationLogin = _sessionUser.IsInstallationAdmin;
        AllowsSupportiveLogin = user.AllowsSupportiveLogin;
        ShowWishKnowlede = user.ShowWishKnowledge;
        IsMember = user.IsMember();

        DescriptionShort = "";

        IndexInResult = indexInResultSet;

        UserLink = urlHelper => Links.UserDetail(user.Name, user.Id);

        ImageUrl = new UserImageSettings(user.Id).GetUrl_128px_square(user).Url;

        DoIFollow = followerIAm.Of(user.Id);
    }

}