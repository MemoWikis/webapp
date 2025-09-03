public class UserSkillController(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    IHttpContextAccessor _httpContextAccessor,
    ExtendedUserCache _extendedUserCache,
    UserSkillService _userSkillService) : ApiBaseController
{
    public readonly record struct AddResult(
        bool Success,
        string ErrorMessageKey,
        PageItem? AddedSkill = null);

    public readonly record struct RemoveResult(
        bool Success,
        string ErrorMessageKey);

    public readonly record struct CheckRequest(int UserId, int PageId);

    [HttpGet]
    public bool Check([FromBody] CheckRequest request)
    {
        if (_sessionUser.UserId != request.UserId)
            return false;

        return _extendedUserCache.GetUser(request.UserId).IsSkill(request.PageId);
    }

    [HttpPost]
    public AddResult Add([FromRoute] int id)
    {
        if (!_sessionUser.IsLoggedIn)
            throw new UnauthorizedAccessException(FrontendMessageKeys.Error.User.NotLoggedIn);

        if (_sessionUser.IsLoggedIn)
            throw new UnauthorizedAccessException(FrontendMessageKeys.Error.User.NotLoggedIn);

        var page = EntityCache.GetPage(id);

        if (page == null)
        {
            return new AddResult(false, FrontendMessageKeys.Error.Page.NotFound, null);
        }

        if (!_permissionCheck.CanView(page))
        {
            return new AddResult(false, FrontendMessageKeys.Error.Page.NoRights, null);
        }

        var extendedUserCacheItem = _extendedUserCache.GetUser(_sessionUser.UserId);

        // Check if skill already exists
        var existingSkill = extendedUserCacheItem.GetSkill(id);
        if (existingSkill != null)
        {
            return new AddResult(false, FrontendMessageKeys.Error.Skill.AlreadyExists, null);
        }

        // Use UserSkillService to create the skill
        _userSkillService.CreateUserSkill(_sessionUser.UserId, id);

        // Get the newly created skill from cache to return it
        var newSkill = extendedUserCacheItem.GetSkill(id);
        if (newSkill != null)
        {
            var addedSkillPageItem = new PageItem(
                page.Id,
                page.Name,
                new PageImageSettings(page.Id, _httpContextAccessor).GetUrl_128px(true).Url,
                page.CountQuestions,
                new KnowledgeSummaryResponse(newSkill.KnowledgeSummary));

            return new AddResult(true, "", addedSkillPageItem);
        }

        return new AddResult(false, FrontendMessageKeys.Error.Skill.AddFailed, null);
    }

    [HttpPost]
    public RemoveResult Remove([FromRoute] int id)
    {
        if (!_sessionUser.IsLoggedIn)
            throw new UnauthorizedAccessException(FrontendMessageKeys.Error.User.NotLoggedIn);

        var extendedUserCacheItem = _extendedUserCache.GetUser(_sessionUser.UserId);

        // Check if skill exists
        var existingSkill = extendedUserCacheItem.GetSkill(id);
        if (existingSkill == null)
        {
            return new RemoveResult(false, FrontendMessageKeys.Error.Skill.NotFound);
        }

        // Use UserSkillService to remove the skill
        _userSkillService.RemoveUserSkill(_sessionUser.UserId, id);

        return new RemoveResult(true, "");
    }
}