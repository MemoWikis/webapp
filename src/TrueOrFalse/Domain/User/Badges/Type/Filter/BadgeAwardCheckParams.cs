public class BadgeAwardCheckParams
{
    public User CurrentUser;
    public BadgeType BadgeType;

    public BadgeAwardCheckParams(BadgeType badgeType, User user)
    {
        BadgeType = badgeType;
        CurrentUser = user;
    }

    public int WishknowledgeCount()
    {
        return Sl.R<QuestionValuationRepo>().GetByUser(CurrentUser.Id).Count;
    }

    public int AnswerCount()
    {
        return Sl.R<AnswerHistoryRepo>().GetByUser(CurrentUser.Id).Count;
    }
}