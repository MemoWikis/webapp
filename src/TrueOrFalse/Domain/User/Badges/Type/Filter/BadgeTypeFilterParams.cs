public class BadgeTypeFilterParams
{
    public User CurrentUser;
    public BadgeType BadgeType;

    public int WishknowledgeCount()
    {
        return Sl.R<QuestionValuationRepo>().GetByUser(CurrentUser.Id).Count;
    }

    public int AnswerCount()
    {
        return Sl.R<AnswerHistoryRepo>().GetByUser(CurrentUser.Id).Count;
    }
}