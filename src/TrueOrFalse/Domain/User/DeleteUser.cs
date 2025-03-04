public class DeleteUser(UserWritingRepo _userWritingRepo) : IRegisterAsInstancePerLifetime
{
    private readonly int _viewLimit = 2000;
    public bool CanDelete(int userId)
    {
        var userExists = EntityCache.GetUserByIdNullable(userId);
        if (userExists == null)
            return false;

        var pages = EntityCache.GetAllPagesList()
            .Where(p => p.CreatorId == userId && p.Visibility == PageVisibility.All);
        var questions = EntityCache.GetAllQuestions()
            .Where(q => q.CreatorId == userId && q.Visibility == QuestionVisibility.All);

        var pageViews = pages.Sum(p => p.TotalViews);
        var questionViews = questions.Sum(q => q.TotalViews);

        var hasOrphan = false;

        foreach (var page in pages)
        {
            hasOrphan = GraphService.Children(page.Id).Any(p => p.CreatorId != userId);
            if (hasOrphan)
                break;

            hasOrphan = page.GetAggregatedQuestionsFromMemoryCache(userId, false, true).Any(q => q.CreatorId != userId);
            if (hasOrphan)
                break;
        }

        //return false;

        return _viewLimit >= pageViews + questionViews && !hasOrphan;
    }

    public void Run(int userId)
    {
        _userWritingRepo.DeleteFromAllTables(userId);
        EntityCache.RemoveUser(userId);
    }
}