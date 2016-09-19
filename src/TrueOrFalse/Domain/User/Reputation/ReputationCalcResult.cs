public class ReputationCalcResult
{
    public User User;

    public int ForQuestionsCreated;
    public int ForQuestionsInOtherWishknowledge;

    public int ForSetsCreated;
    public int ForSetsInOtherWishknowledge;

    public int ForDatesCreatedVisible;
    public int ForDatesCopied;

    public int ForUsersFollowingMe;

    public int ForPublicWishknowledge;

    public int TotalReputation
    {
        get
        {
            return
                ForQuestionsCreated +
                ForQuestionsInOtherWishknowledge +
                ForSetsCreated +
                ForSetsInOtherWishknowledge +
                ForDatesCreatedVisible + 
                ForDatesCopied +
                ForUsersFollowingMe + 
                ForPublicWishknowledge;
        }
    }
}