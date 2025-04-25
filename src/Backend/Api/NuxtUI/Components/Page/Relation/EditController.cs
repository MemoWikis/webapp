using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using TrueOrFalse.Search;
using static ChildModifier;

public class PageRelationEditController(
    SessionUser _sessionUser,
    PageCreator pageCreator,
    PermissionCheck _permissionCheck,
    PageRepository pageRepository,
    IHttpContextAccessor _httpContextAccessor,
    PageRelationRepo pageRelationRepo,
    UserWritingRepo _userWritingRepo,
    IWebHostEnvironment _webHostEnvironment,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    QuestionReadingRepo _questionReadingRepo,
    IGlobalSearch _search,
    MeiliSearchReIndexUser _meiliSearchReIndexUser) : ApiBaseController
{
    public readonly record struct ValidateNameParam(string Name);

    [HttpPost]
    public ValidateNameResult ValidateName([FromBody] ValidateNameParam param)
    {
        var data = ValidateName(param.Name);
        return data;
    }

    public readonly record struct QuickCreateParam(string Name, int ParentPageId);

    public readonly record struct QuickCreateResult(
        bool Success,
        string MessageKey,
        CreateTinyPageItem Data);

    public readonly record struct CreateTinyPageItem(
        bool CantSavePrivatePage,
        string Name,
        int Id);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public QuickCreateResult QuickCreate([FromBody] QuickCreateParam param)
    {
        var data = pageCreator.Create(param.Name, param.ParentPageId, _sessionUser);
        var result = new QuickCreateResult
        {
            Success = data.Success,
            MessageKey = data.MessageKey,
            Data = new CreateTinyPageItem
            {
                Name = data.Data.Name,
                Id = data.Data.Id,
                CantSavePrivatePage = data.Data.CantSavePrivatePage
            }
        };

        if (result.Success)
            _meiliSearchReIndexUser.Run(EntityCache.GetUserById(_sessionUser.UserId));

        return result;
    }

    public readonly record struct SearchParam(string term, int[] pageIdsToFilter);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<SearchPageResult> SearchPageAsync([FromBody] SearchParam param)
    {
        var data = await SearchPageAsync(param.term, param.pageIdsToFilter)
            .ConfigureAwait(false);
        return data;
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<SearchPageInPersonalWikiResult> SearchPageInPersonalWiki(
        [FromBody] SearchParam param)
    {
        var data = await SearchPageInPersonalWikiAsync(param.term, param.pageIdsToFilter);
        return data;
    }

    public readonly record struct MoveChildParam(
        int childId,
        int parentIdToRemove,
        int parentIdToAdd);
    public readonly record struct MoveChildResult(
        bool Success,
        string MessageKey,
        TinyPageItem Data);
    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public MoveChildResult MoveChild([FromBody] MoveChildParam param)
    {
        return MoveChild(
            param.childId,
            param.parentIdToRemove,
            param.parentIdToAdd);
    }

    private MoveChildResult MoveChild(int childId, int parentIdToRemove, int parentIdToAdd)
    {
        if (childId == parentIdToRemove || childId == parentIdToAdd)
            return new MoveChildResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Page.LoopLink
            };

        if (parentIdToRemove == FeaturedPage.RootPageId &&
            !_sessionUser.IsInstallationAdmin ||
            parentIdToAdd == FeaturedPage.RootPageId &&
            !_sessionUser.IsInstallationAdmin)
            return new MoveChildResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Page.ParentIsRoot
            };

        var childModifier = new ChildModifier(_permissionCheck,
            _sessionUser,
            pageRepository,
            _userWritingRepo,
            _httpContextAccessor,
            _webHostEnvironment,
            pageRelationRepo);

        var result = childModifier.AddChild(childId, parentIdToAdd);

        childModifier.RemoveParent(parentIdToRemove, childId);
        return new MoveChildResult(result.Success, result.MessageKey, result.Data);
    }

    public readonly record struct AddChildParam(
        int ChildId,
        int ParentId,
        int ParentIdToRemove,
        bool AddIdToWikiHistory);
    public readonly record struct AddChildResult(
        bool Success,
        string MessageKey,
        TinyPageItem Data);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public AddChildResult AddChild([FromBody] AddChildParam param)
    {
        if (param.ChildId <= 0)
            throw new Exception("No ChildId");

        var result =
            new ChildModifier(_permissionCheck,
                    _sessionUser,
                    pageRepository,
                    _userWritingRepo,
                    _httpContextAccessor,
                    _webHostEnvironment,
                    pageRelationRepo)
                .AddChild(
                    param.ChildId,
                    param.ParentId,
                    param.AddIdToWikiHistory);

        return new AddChildResult(result.Success, result.MessageKey, result.Data);
    }

    public readonly record struct RemoveParentParam(
        int parentIdToRemove,
        int childId);
    public readonly record struct RemoveParentResult(
        bool Success,
        string MessageKey,
        TinyPageItem Data);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public RemoveParentResult RemoveParent([FromBody] RemoveParentParam param)
    {
        var result = new ChildModifier(_permissionCheck,
                _sessionUser,
                pageRepository,
                _userWritingRepo,
                _httpContextAccessor,
                _webHostEnvironment,
                pageRelationRepo)
            .RemoveParent(
                param.parentIdToRemove,
                param.childId);
        return new RemoveParentResult(result.Success, result.MessageKey, result.Data);
    }

    public readonly record struct SearchPageResult(
        int TotalCount,
        List<SearchPageItem> Pages);

    private async Task<SearchPageResult> SearchPageAsync(
        string term,
        int[] pageIdsToFilter = null)
    {
        var items = new List<SearchPageItem>();
        var elements = await _search.GoAllPagesAsync(term)
            .ConfigureAwait(false);

        if (elements.Pages.Any())
            new SearchHelper(_imageMetaDataReadingRepo,
                    _httpContextAccessor,
                    _questionReadingRepo)
                .AddPageItems(items, elements, _permissionCheck, _sessionUser.UserId);

        return new SearchPageResult
        {
            TotalCount = elements.PageCount,
            Pages = items,
        };
    }

    public readonly record struct SearchPageInPersonalWikiResult(
        int TotalCount,
        List<SearchPageItem> Pages);

    private async Task<SearchPageInPersonalWikiResult> SearchPageInPersonalWikiAsync(
        string term,
        int[] pageIdsToFilter = null)
    {
        var items = new List<SearchPageItem>();
        var elements = await _search
            .GoAllPagesAsync(term)
            .ConfigureAwait(false);

        if (elements.Pages.Any())
            new SearchHelper(_imageMetaDataReadingRepo,
                    _httpContextAccessor,
                    _questionReadingRepo)
                .AddPageItems(items, elements, _permissionCheck, _sessionUser.UserId, pageIdsToFilter);

        var wikiChildren = GraphService.VisibleDescendants(_sessionUser.User.StartPageId,
            _permissionCheck, _sessionUser.UserId);
        items = items.Where(i => wikiChildren.Any(c => c.Id == i.Id)).ToList();

        return new SearchPageInPersonalWikiResult
        {
            TotalCount = elements.PageCount,
            Pages = items,
        };
    }

    public readonly record struct ValidateNameResult(
        bool Success,
        string MessageKey,
        ValidateTinyPageItem Data);

    public readonly record struct ValidateTinyPageItem(bool PageNameAllowed, string name);

    private ValidateNameResult ValidateName(string name)
    {
        var nameValidator = new PageNameValidator();

        if (nameValidator.IsForbiddenName(name))
        {
            return new ValidateNameResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Page.NameIsForbidden,
                Data = new ValidateTinyPageItem
                {
                    PageNameAllowed = false,
                    name = name,
                }
            };
        }

        return new ValidateNameResult
        {
            Success = true
        };
    }
}
