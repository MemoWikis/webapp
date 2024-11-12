public class UpdateQuestionCountForPage(SessionUser sessionUser) : IRegisterAsInstancePerLifetime
{
    public void Run(Page page, int userId)
    {
        page.UpdateCountQuestionsAggregated(userId);
    }

    public void RunForJob(Page page, int authorId)
    {
        page.UpdateCountQuestionsAggregated(authorId);
    }

    public void Run(IList<Page> pages, int? userId = null)
    {
        userId ??= sessionUser.UserId;

        foreach (var page in pages)
        {
            Run(page, (int)userId);
        }
    }
}