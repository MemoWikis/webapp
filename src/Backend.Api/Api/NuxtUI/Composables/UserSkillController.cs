using static MissionControlController;

public class UserSkillController(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    IHttpContextAccessor _httpContextAccessor,
    ExtendedUserCache _extendedUserCache,
    UserSkillService _userSkillService) : ApiBaseController
{
    public readonly record struct AddRequest(int PageId);

    public readonly record struct AddResult(
        bool Success,
        string ErrorMessageKey,
        PageItem AddedSkill);

    public readonly record struct RemoveRequest(int PageId);

    public readonly record struct RemoveResult(
        bool Success,
        string ErrorMessageKey);

    public readonly record struct CheckRequest(int PageId);

    public readonly record struct CheckResult(
        bool IsSkill);

    [HttpPost]
    public CheckResult Check([FromRoute] int id, [FromBody] CheckRequest request)
    {
        if (_sessionUser.UserId != id)
        {
            return new CheckResult(false);
        }

        try
        {
            var extendedUserCacheItem = _extendedUserCache.GetUser(id);
            var existingSkill = extendedUserCacheItem.GetSkill(request.PageId);
            
            return new CheckResult(existingSkill != null);
        }
        catch (Exception)
        {
            return new CheckResult(false);
        }
    }

    [HttpPost]
    public AddResult Add([FromRoute] int id, [FromBody] AddRequest request)
    {
        var defaultPageItem = new PageItem(0, "", "", 0, FillKnowledgeSummaryResponse(new KnowledgeSummary()));

        if (_sessionUser.UserId != id)
        {
            return new AddResult(false, "error.unauthorized", defaultPageItem);
        }

        var page = EntityCache.GetPage(request.PageId);
        if (page == null)
        {
            return new AddResult(false, "error.pageNotFound", defaultPageItem);
        }

        if (!_permissionCheck.CanView(page))
        {
            return new AddResult(false, "error.noPermission", defaultPageItem);
        }

        try
        {
            var extendedUserCacheItem = _extendedUserCache.GetUser(id);
            
            // Check if skill already exists
            var existingSkill = extendedUserCacheItem.GetSkill(request.PageId);
            if (existingSkill != null)
            {
                return new AddResult(false, "error.skillAlreadyExists", defaultPageItem);
            }

            // Use UserSkillService to create the skill
            _userSkillService.CreateUserSkill(id, request.PageId);

            // Get the newly created skill from cache to return it
            var newSkill = extendedUserCacheItem.GetSkill(request.PageId);
            if (newSkill != null)
            {
                var addedSkillPageItem = new PageItem(
                    page.Id,
                    page.Name,
                    new PageImageSettings(page.Id, _httpContextAccessor).GetUrl_128px(true).Url,
                    page.CountQuestions,
                    FillKnowledgeSummaryResponse(newSkill.KnowledgeSummary));

                return new AddResult(true, "", addedSkillPageItem);
            }

            return new AddResult(false, "error.addSkill.failed", defaultPageItem);
        }
        catch (Exception)
        {
            return new AddResult(false, "error.addSkill.failed", defaultPageItem);
        }
    }

    [HttpPost]
    public RemoveResult Remove([FromRoute] int id, [FromBody] RemoveRequest request)
    {
        if (_sessionUser.UserId != id)
        {
            return new RemoveResult(false, "error.unauthorized");
        }

        try
        {
            var extendedUserCacheItem = _extendedUserCache.GetUser(id);
            
            // Check if skill exists
            var existingSkill = extendedUserCacheItem.GetSkill(request.PageId);
            if (existingSkill == null)
            {
                return new RemoveResult(false, "error.skillNotFound");
            }

            // Use UserSkillService to remove the skill
            _userSkillService.RemoveUserSkill(id, request.PageId);

            return new RemoveResult(true, "");
        }
        catch (Exception)
        {
            return new RemoveResult(false, "error.removeSkill.failed");
        }
    }

    private KnowledgeSummaryResponse FillKnowledgeSummaryResponse(KnowledgeSummary knowledgeSummary)
    {
        return new KnowledgeSummaryResponse(
            knowledgeSummary.NotLearned,
            knowledgeSummary.NotLearnedPercentage,
            knowledgeSummary.NeedsLearning,
            knowledgeSummary.NeedsLearningPercentage,
            knowledgeSummary.NeedsConsolidation,
            knowledgeSummary.NeedsConsolidationPercentage,
            knowledgeSummary.Solid,
            knowledgeSummary.SolidPercentage,
            knowledgeSummary.NotInWishknowledge,
            knowledgeSummary.NotInWishknowledgePercentage,
            knowledgeSummary.Total);
    }
}
