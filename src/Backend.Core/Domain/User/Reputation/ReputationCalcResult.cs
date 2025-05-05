public class ReputationCalcResult
{
    public UserTinyModel User;

    public int ForQuestionsCreated;
    public int ForQuestionsInOtherWishknowledge;

    public int ForUsersFollowingMe;

    public int ForPublicWishknowledge;

    public int TotalReputation =>
        ForQuestionsCreated +
        ForQuestionsInOtherWishknowledge +
        ForUsersFollowingMe + 
        ForPublicWishknowledge;
}