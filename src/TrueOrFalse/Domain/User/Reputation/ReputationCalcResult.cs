public class ReputationCalcResult
{
    public UserTinyModel User;

    public int ForQuestionsCreated;
    public int ForQuestionsInOtherWishknowledge;

    public int ForSetsCreated;
    public int ForSetsInOtherWishknowledge;

    public int ForDatesCreatedVisible;
    public int ForDatesCopied;

    public int ForUsersFollowingMe;

    public int ForPublicWishknowledge;

    public int TotalReputation =>
        ForQuestionsCreated +
        ForQuestionsInOtherWishknowledge +
        ForSetsCreated +
        ForSetsInOtherWishknowledge +
        ForDatesCreatedVisible + 
        ForDatesCopied +
        ForUsersFollowingMe + 
        ForPublicWishknowledge;
}