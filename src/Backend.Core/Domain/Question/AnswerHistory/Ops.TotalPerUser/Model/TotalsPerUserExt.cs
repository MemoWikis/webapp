public static class TotalsPerUserExt
{
    public static TotalPerUser ByQuestionId(
        this IEnumerable<TotalPerUser> totalsPerUser,
        int questionId)
    {
        return totalsPerUser.First(item => item.QuestionId == questionId);
    }
}