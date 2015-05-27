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

    public IList<Question> WishQuestions;
    public IList<Category> WishQuestionsCategories;

    public IList<Set> WishSets;

    public bool IsCurrentUser;

    public User User;

    public UserModel(User user)
    {
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

        WishQuestions =
            Resolve<QuestionRepository>().GetByIds(
                Resolve<QuestionValuationRepository>()
                    .GetByUser(user.Id)
                    .QuestionIds().ToList()
            );

        if (!IsCurrentUser)
            WishQuestions = WishQuestions.Where(q => q.Visibility == QuestionVisibility.All).ToList();

        WishQuestionsCategories = WishQuestions.GetAllCategories();

        WishSets = Resolve<SetRepository>().GetByIds(
            Resolve<SetValuationRepository>()
                .GetByUser(user.Id)
                .SetIds().ToArray()
            );
    }
}
