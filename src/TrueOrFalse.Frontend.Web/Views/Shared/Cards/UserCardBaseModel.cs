﻿using System.Collections.Generic;

public class UserCardBaseModel : BaseModel
{
    public ReputationCalcResult Reputation;
    public bool DoIFollow;
    public int AmountWishCountQuestions;
    public bool IsCurrentUser;

    public void FillUserCardBaseModel(IList<UserTinyModel> authors, int currentUserId)
    {
        if (authors.Count == 1)
        {
            if (!authors[0].IsKnown)
            {
                Reputation = Resolve<ReputationCalc>().Run(authors[0].User);
                AmountWishCountQuestions = Resolve<GetWishQuestionCount>().Run(authors[0].Id);
                var followerIAm = R<FollowerIAm>().Init(new List<int> {authors[0].Id}, currentUserId);
                DoIFollow = followerIAm.Of(authors[0].Id);
                IsCurrentUser = authors[0].Id == currentUserId && IsLoggedIn;
            }
            else
            {
                Reputation = new ReputationCalcResult {User = authors[0]};
            }
        }
    }
}