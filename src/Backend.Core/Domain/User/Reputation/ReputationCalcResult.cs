public class ReputationCalcResult
{
    public UserTinyModel User;

    public int ForQuestionsCreated;
    public int ForQuestionsInOtherWishKnowledge;

    public int ForUsersFollowingMe;

    public int ForPublicWishKnowledge;

    public int TotalReputation =>
        ForQuestionsCreated +
        ForQuestionsInOtherWishKnowledge +
        ForUsersFollowingMe + 
        ForPublicWishKnowledge;
}