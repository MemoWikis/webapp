using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static VueApp.ChildModifier;

namespace VueApp;

public class TopicRelationEditController(
    SessionUser _sessionUser,
    CategoryCreator _categoryCreator,
    PermissionCheck _permissionCheck,
    CategoryRepository _categoryRepository,
    IHttpContextAccessor _httpContextAccessor,
    CategoryRelationRepo _categoryRelationRepo,
    UserWritingRepo _userWritingRepo,
    IWebHostEnvironment _webHostEnvironment,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    QuestionReadingRepo _questionReadingRepo,
    IGlobalSearch _search) : Controller
{
    public readonly record struct ValidateNameParam(string Name);

    [HttpPost]
    public ValidateNameResult ValidateName([FromBody] ValidateNameParam param)
    {
        var data = ValidateName(param.Name);
        return data;
    }

    public readonly record struct QuickCreateParam(string Name, int ParentTopicId);

    public readonly record struct QuickCreateResult(
        bool Success,
        string MessageKey,
        CreateTinyTopicItem Data);

    public readonly record struct CreateTinyTopicItem(
        bool CantSavePrivateTopic,
        string Name,
        int Id);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public QuickCreateResult QuickCreate([FromBody] QuickCreateParam param)
    {
        var data = _categoryCreator.Create(param.Name, param.ParentTopicId, _sessionUser);
        var result = new QuickCreateResult
        {
            Success = data.Success,
            MessageKey = data.MessageKey,
            Data = new CreateTinyTopicItem
            {
                Name = data.Data.Name,
                Id = data.Data.Id,
                CantSavePrivateTopic = data.Data.CantSavePrivateTopic
            }
        };

        return result;
    }

    public readonly record struct SearchParam(string term, int[] topicIdsToFilter);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<SearchTopicResult> SearchTopicAsync([FromBody] SearchParam param)
    {
        var data = await SearchTopicAsync(param.term, param.topicIdsToFilter)
            .ConfigureAwait(false);
        return data;
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<SearchTopicInPersonalWikiResult> SearchTopicInPersonalWiki(
        [FromBody] SearchParam param)
    {
        var data = await SearchTopicInPersonalWikiAsync(param.term, param.topicIdsToFilter);
        return data;
    }

    public readonly record struct MoveChildParam(
        int childId,
        int parentIdToRemove,
        int parentIdToAdd);

    public readonly record struct MoveChildResult(
        bool Success,
        string MessageKey,
        TinyTopicItem Data);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<MoveChildResult> MoveChild([FromBody] MoveChildParam param)
    {
        return await MoveChildAsync(
                param.childId,
                param.parentIdToRemove,
                param.parentIdToAdd)
            .ConfigureAwait(false);
    }

    private async Task<MoveChildResult> MoveChildAsync(
        int childId,
        int parentIdToRemove,
        int parentIdToAdd)
    {
        if (childId == parentIdToRemove || childId == parentIdToAdd)
            return new MoveChildResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Category.LoopLink
            };

        if (parentIdToRemove == RootCategory.RootCategoryId &&
            !_sessionUser.IsInstallationAdmin || parentIdToAdd == RootCategory.RootCategoryId &&
            !_sessionUser.IsInstallationAdmin)
            return new MoveChildResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Category.ParentIsRoot
            };

        var childmodifier = new ChildModifier(_permissionCheck,
            _sessionUser,
            _categoryRepository,
            _userWritingRepo,
            _httpContextAccessor,
            _webHostEnvironment,
            _categoryRelationRepo);

        var result = await childmodifier
            .AddChildAsync(childId,
                parentIdToAdd)
            .ConfigureAwait(false);

        await childmodifier
            .RemoveParentAsync(
                parentIdToRemove,
                childId,
                new int[] { parentIdToAdd, parentIdToRemove })
            .ConfigureAwait(false);
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
        TinyTopicItem Data);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<AddChildResult> AddChild([FromBody] AddChildParam param)
    {
        var childModifier = new ChildModifier(
            _permissionCheck,
            _sessionUser,
            _categoryRepository,
            _userWritingRepo,
            _httpContextAccessor,
            _webHostEnvironment,
            _categoryRelationRepo);

        var result = await childModifier
            .AddChildAsync(
                param.ChildId,
                param.ParentId,
                param.AddIdToWikiHistory)
            .ConfigureAwait(false);

        return new AddChildResult(result.Success, result.MessageKey, result.Data);
    }

    public readonly record struct RemoveParentParam(
        int parentIdToRemove,
        int childId,
        int[] affectedParentIdsByMove = null);

    public readonly record struct RemoveParentResult(
        bool Success,
        string MessageKey,
        TinyTopicItem Data);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<RemoveParentResult> RemoveParent([FromBody] RemoveParentParam param)
    {
        var childModifier = new ChildModifier(
            _permissionCheck,
            _sessionUser,
            _categoryRepository,
            _userWritingRepo,
            _httpContextAccessor,
            _webHostEnvironment,
            _categoryRelationRepo);

        var result = await childModifier
            .RemoveParentAsync(
                param.parentIdToRemove,
                param.childId,
                param.affectedParentIdsByMove)
            .ConfigureAwait(false);
        return new RemoveParentResult(result.Success, result.MessageKey, result.Data);
    }

    public readonly record struct SearchTopicResult(
        int TotalCount,
        List<SearchTopicItem> Topics);

    private async Task<SearchTopicResult> SearchTopicAsync(
        string term,
        int[] topicIdsToFilter = null)
    {
        var items = new List<SearchTopicItem>();
        var elements = await _search.GoAllCategoriesAsync(term, topicIdsToFilter)
            .ConfigureAwait(false);

        if (elements.Categories.Any())
            new SearchHelper(_imageMetaDataReadingRepo,
                    _httpContextAccessor,
                    _questionReadingRepo)
                .AddTopicItems(items, elements, _permissionCheck, _sessionUser.UserId);

        return new SearchTopicResult
        {
            TotalCount = elements.CategoriesResultCount,
            Topics = items,
        };
    }

    public readonly record struct SearchTopicInPersonalWikiResult(
        int TotalCount,
        List<SearchTopicItem> Topics);

    private async Task<SearchTopicInPersonalWikiResult> SearchTopicInPersonalWikiAsync(
        string term,
        int[] topicIdsToFilter = null)
    {
        var items = new List<SearchTopicItem>();
        var elements = await _search
            .GoAllCategoriesAsync(term, topicIdsToFilter)
            .ConfigureAwait(false);

        if (elements.Categories.Any())
            new SearchHelper(_imageMetaDataReadingRepo,
                    _httpContextAccessor,
                    _questionReadingRepo)
                .AddTopicItems(items, elements, _permissionCheck, _sessionUser.UserId);

        var wikiChildren = GraphService.VisibleDescendants(_sessionUser.User.StartTopicId,
            _permissionCheck, _sessionUser.UserId);
        items = items.Where(i => wikiChildren.Any(c => c.Id == i.Id)).ToList();

        return new SearchTopicInPersonalWikiResult
        {
            TotalCount = elements.CategoriesResultCount,
            Topics = items,
        };
    }

    public readonly record struct ValidateNameResult(
        bool Success,
        string MessageKey,
        ValidateTinyTopicItem Data);

    public readonly record struct ValidateTinyTopicItem(bool CategoryNameAllowed, string name);

    private ValidateNameResult ValidateName(string name)
    {
        var nameValidator = new TopicNameValidator();

        if (nameValidator.IsForbiddenName(name))
        {
            return new ValidateNameResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Category.NameIsForbidden,
                Data = new ValidateTinyTopicItem
                {
                    CategoryNameAllowed = false,
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