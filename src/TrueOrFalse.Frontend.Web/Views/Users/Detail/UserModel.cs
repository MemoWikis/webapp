using System.Collections.Generic;
using System.Linq;

public class UserModel : BaseModel
{
    public string Name { get; private set; }
    
    public int AmountCreatedQuestions;
    public int AmountCreatedSets;
    public int AmountCreatedCategories;

    public int AmountWishCountQuestions;
    public int AmountWishCountSets;

    public string ImageUrl_250;
    public bool ImageIsCustom;

    public int ReputationRank;
    public int ReputationTotal;
    public ReputationCalcResult Reputation;

    public bool IsMember;
    public bool IsCurrentUser;

    public bool IsActiveTabKnowledge;
    public bool IsActiveTabBadges;

    public UserTinyModel User;
    public int UserIdProfile;
    public bool DoIFollow;

    public UserModel(User user, bool isActiveTabKnowledge = false, bool isActiveTabBadges = false)
    {
        User = new UserTinyModel(user);
        IsActiveTabKnowledge = isActiveTabKnowledge;
        IsActiveTabBadges = isActiveTabBadges;

        Name = User.Name;

        IsMember = User.IsMember;
        IsCurrentUser = User.Id == UserId && IsLoggedIn;

        AmountCreatedQuestions = Resolve<UserSummary>().AmountCreatedQuestions(User.Id, false);
        AmountCreatedSets = Resolve<UserSummary>().AmountCreatedSets(User.Id);
        AmountCreatedCategories = Resolve<UserSummary>().AmountCreatedCategories(User.Id);

        AmountWishCountQuestions = Resolve<GetWishQuestionCount>().Run(User.Id);
      //  AmountWishCountSets = Resolve<GetWishSetCount>().Run(User.Id);

        var imageResult = new UserImageSettings(User.Id).GetUrl_250px(User);
        ImageUrl_250 = imageResult.Url;
        ImageIsCustom = imageResult.HasUploadedImage;

        var reputation = Resolve<ReputationCalc>().Run(user);
        ReputationRank = User.ReputationPos;
        ReputationTotal = reputation.TotalReputation;
        Reputation = reputation;

        UserIdProfile = User.Id;
        var followerIAm = R<FollowerIAm>().Init(new List<int> { UserIdProfile }, UserId);
        DoIFollow = followerIAm.Of(User.Id);
    }
}
