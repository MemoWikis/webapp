public class ReputationCalcResult
{
    public User User;

    public int ForQuestionsCreated;
    public int ForQuestionsWishCount;
    public int ForQuestionsWishKnow;

    public int ForSetWishCount;
    public int ForSetWishKnow;

    public int ForDatesCreatedVisible;
    public int ForDatesCopied;

    public int TotalReputation
    {
        get
        {
            return
                ForQuestionsCreated +
                ForQuestionsWishCount +
                ForQuestionsWishKnow +
                ForSetWishCount +
                ForSetWishKnow +
                ForDatesCreatedVisible + 
                ForDatesCopied;
        }
    }
}