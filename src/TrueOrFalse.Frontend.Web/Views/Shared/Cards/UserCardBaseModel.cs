using System.Collections.Generic;

public class UserCardBaseModel:BaseResolve
{
    public ReputationCalcResult Reputation;
    public bool DoIFollow;
    public int AmountWishCountQuestions;
    public bool IsCurrentUser;
    private SessionUser _sessionUser => Resolve<SessionUser>();
    public bool IsLoggedIn => _sessionUser.IsLoggedIn;
    public User User => _sessionUser.User;
    public UserTinyModel Author; 
    

    public void FillUserCardBaseModel(IList<UserTinyModel> authors, int currentUserId)
    {
        if (authors.Count == 1)
        {
            Author = authors[0];
            Reputation = Resolve<ReputationCalc>().Run(authors[0].User);
            if (authors[0].IsKnown)
            {
                AmountWishCountQuestions = Resolve<GetWishQuestionCount>().Run(authors[0].Id);
                var followerIAm = R<FollowerIAm>().Init(new List<int> { authors[0].Id }, currentUserId);
                DoIFollow = followerIAm.Of(authors[0].Id);
                IsCurrentUser = authors[0].Id == currentUserId && IsLoggedIn;
            }
        }
    }
}