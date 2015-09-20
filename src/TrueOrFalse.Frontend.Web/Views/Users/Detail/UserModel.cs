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

    public bool IsCurrentUser;

    public bool ActiveTabKnowledge;
    public bool ActiveTabBadges;

    public User User;

    public UserModel(User user, bool isActiveTabKnowledge = false, bool isActiveTabBadges = false)
    {
        ActiveTabKnowledge = isActiveTabKnowledge;
        ActiveTabBadges = isActiveTabBadges;

        User = user;
        Name = user.Name;

        IsCurrentUser = user.Id == UserId && IsLoggedIn;

        AmountCreatedQuestions = Resolve<UserSummary>().AmountCreatedQuestions(user.Id);
        AmountCreatedSets = Resolve<UserSummary>().AmountCreatedSets(user.Id);
        AmountCreatedCategories = Resolve<UserSummary>().AmountCreatedCategories(user.Id);

        AmountWishCountQuestions = Resolve<GetWishQuestionCount>().Run(user.Id);
        AmountWishCountSets = Resolve<GetWishSetCount>().Run(user.Id);

        var imageResult = new UserImageSettings(user.Id).GetUrl_250px(user.EmailAddress);
        ImageUrl_250 = imageResult.Url;
        ImageIsCustom = imageResult.HasUploadedImage;

        var reputation = Resolve<ReputationCalc>().Run(user);
        ReputationRank = user.ReputationPos;
        ReputationTotal = reputation.TotalRepuation;
        Reputation = reputation;
    }
}
