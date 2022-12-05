using System.Collections.Generic;

public class UserCardBaseModel:BaseResolve
{
    public int Reputation;
    public bool DoIFollow;
    public int AmountWishCountQuestions;
    public bool IsCurrentUser;
    public bool IsLoggedIn => SessionUser.IsLoggedIn;
    public SessionUserCacheItem User => SessionUser.User;
    public UserTinyModel Author;


    public void FillUserCardBaseModel(IList<UserTinyModel> authors, int currentUserId)
    {
        if (authors.Count == 1)
        {
            Author = authors[0];
            var userCashUser = SessionUserCache.GetItem(Author.Id).User;
            Author.ShowWishKnowledge = userCashUser.ShowWishKnowledge;
            Reputation = userCashUser.Reputation;

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