using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.Security;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Exception = System.Exception;

namespace VueApp;

public class EditTopicRelationStoreController(
    SessionUser _sessionUser,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    IHttpContextAccessor _httpContextAccessor,
    QuestionReadingRepo _questionReadingRepo,
    PermissionCheck _permissionCheck,
    CategoryRepository _categoryRepository,
    CategoryRelationRepo _categoryRelationRepo,
    UserWritingRepo _userWritingRepo,
    IWebHostEnvironment _webHostEnvironment) : Controller
{
    public record struct PersonalWikiData(
        SearchTopicItem PersonalWiki,
        SearchTopicItem[] RecentlyUsedRelationTargetTopics);

    public record struct GetPersonalWikiDataResult(
        bool Success,
        string MessageKey,
        PersonalWikiData? Data);

    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public GetPersonalWikiDataResult GetPersonalWikiData([FromRoute] int id)
    {
        if (GraphService.Descendants(id).Any(c => c.Id == _sessionUser.User.StartTopicId))
            return new GetPersonalWikiDataResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Category.LoopLink
            };

        var personalWiki = EntityCache.GetCategory(_sessionUser.User.StartTopicId);
        var personalWikiItem = new SearchHelper(_imageMetaDataReadingRepo,
                _httpContextAccessor,
                _questionReadingRepo)
            .FillSearchTopicItem(personalWiki, _sessionUser.UserId);
        var recentlyUsedRelationTargetTopics = new List<SearchTopicItem>();

        if (_sessionUser.User.RecentlyUsedRelationTargetTopicIds.Count > 0)
        {
            foreach (var topicId in _sessionUser.User.RecentlyUsedRelationTargetTopicIds)
            {
                var topicCacheItem = EntityCache.GetCategory(topicId);
                recentlyUsedRelationTargetTopics.Add(new SearchHelper(_imageMetaDataReadingRepo,
                        _httpContextAccessor,
                        _questionReadingRepo)
                    .FillSearchTopicItem(topicCacheItem, _sessionUser.UserId));
            }
        }

        return new GetPersonalWikiDataResult
        {
            Success = true,
            Data = new PersonalWikiData
            {
                PersonalWiki = personalWikiItem,
                RecentlyUsedRelationTargetTopics = recentlyUsedRelationTargetTopics.ToArray()
            }
        };
    }

    public readonly record struct RemoveTopicsJson(int parentId, int[] childIds);

    public readonly record struct RemoveTopicsResult(bool Success, List<int> Data);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<RemoveTopicsResult> RemoveTopics([FromBody] RemoveTopicsJson json)
    {
        var removedChildrenIds = new List<int>();
        var childModifier = new ChildModifier(
            _permissionCheck,
            _sessionUser,
            _categoryRepository,
            _userWritingRepo,
            _httpContextAccessor,
            _webHostEnvironment,
            _categoryRelationRepo);

        foreach (var childId in json.childIds)
        {
            var result = await childModifier
                .RemoveParentAsync(json.parentId, childId)
                .ConfigureAwait(false);
            if (result.Success)
                removedChildrenIds.Add(childId);
        }

        return new RemoveTopicsResult
        {
            Success = true,
            Data = removedChildrenIds
        };
    }

    public enum TargetPosition
    {
        Before,
        After,
        Inner,
        None
    }

    public readonly record struct MoveTopicJson(
        int MovingTopicId,
        int TargetId,
        TargetPosition Position,
        int NewParentId,
        int OldParentId);

    public readonly record struct TryMoveTopicResult(
        int OldParentId,
        int NewParentId,
        MoveTopicJson UndoMove);

    private async Task<TryMoveTopicResult> MoveTopicAsync(MoveTopicJson json)
    {
        if (!_sessionUser.IsLoggedIn)
            throw new SecurityException(FrontendMessageKeys.Error.User.NotLoggedIn);

        if (!_permissionCheck.CanMoveTopic(json.MovingTopicId, json.OldParentId, json.NewParentId))
        {
            if (json.NewParentId == RootCategory.RootCategoryId)
                throw new SecurityException(FrontendMessageKeys.Error.Category.ParentIsRoot);

            throw new SecurityException(FrontendMessageKeys.Error.Category.MissingRights);
        }

        if (json.MovingTopicId == json.NewParentId)
            throw new InvalidOperationException(
                FrontendMessageKeys.Error.Category.CircularReference);

        var relationToMove = EntityCache.GetCategory(json.OldParentId)?.ChildRelations
            .FirstOrDefault(r => r.ChildId == json.MovingTopicId);

        if (relationToMove == null)
        {
            Logg.r.Error(
                "CategoryRelations - MoveTopic: no relation found to move - movingTopicId:{0}, parentId:{1}",
                json.MovingTopicId, json.OldParentId);
            throw new InvalidOperationException(FrontendMessageKeys.Error.Default);
        }

        var undoMoveTopicData =
            GetUndoMoveTopicData(relationToMove, json.NewParentId, json.TargetId);

        var modifyRelationsForCategory =
            new ModifyRelationsForCategory(_categoryRepository, _categoryRelationRepo);

        if (json.Position == TargetPosition.Before)
            await TopicOrderer
                .MoveBeforeAsync(relationToMove,
                    json.TargetId,
                    json.NewParentId,
                    _sessionUser.UserId,
                    modifyRelationsForCategory)
                .ConfigureAwait(false);
        else if (json.Position == TargetPosition.After)
            await TopicOrderer
                .MoveAfterAsync(
                    relationToMove,
                    json.TargetId,
                    json.NewParentId,
                    _sessionUser.UserId,
                    modifyRelationsForCategory)
                .ConfigureAwait(false);
        else if (json.Position == TargetPosition.Inner)
            await TopicOrderer
                .MoveInAsync(
                    relationToMove,
                    json.TargetId,
                    _sessionUser.UserId,
                    modifyRelationsForCategory,
                    _permissionCheck)
                .ConfigureAwait(false);
        else if (json.Position == TargetPosition.None)
            throw new InvalidOperationException(FrontendMessageKeys.Error.Default);

        return new TryMoveTopicResult(json.OldParentId, json.NewParentId, undoMoveTopicData);
    }

    public readonly record struct MoveTopicResult(
        bool Success,
        TryMoveTopicResult? Data,
        string Error);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<MoveTopicResult> MoveTopic([FromBody] MoveTopicJson json)
    {
        try
        {
            var result = await MoveTopicAsync(json).ConfigureAwait(false);
            return new MoveTopicResult(true, result, "");
        }
        catch (Exception ex)
        {
            return new MoveTopicResult(false, null, ex.Message);
        }
    }

    private MoveTopicJson GetUndoMoveTopicData(
        CategoryCacheRelation relation,
        int newParentId,
        int targetId)
    {
        if (relation.PreviousId != null &&
            _permissionCheck.CanViewCategory((int)relation.PreviousId))
            return new MoveTopicJson(relation.ChildId, (int)relation.PreviousId,
                TargetPosition.After, relation.ParentId,
                newParentId);

        if (relation.NextId != null && _permissionCheck.CanViewCategory((int)relation.NextId))
            return new MoveTopicJson(relation.ChildId, (int)relation.NextId, TargetPosition.Before,
                relation.ParentId,
                newParentId);

        return new MoveTopicJson(relation.ChildId, relation.ParentId, TargetPosition.Inner,
            targetId, targetId);
    }

    public readonly record struct PersonalWikiResult(
        bool Success,
        ChildModifier.AddChildResult Data,
        string MessageKey);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<PersonalWikiResult> AddToPersonalWiki([FromRoute] int id)
    {
        var personalWiki = EntityCache.GetCategory(_sessionUser.User.StartTopicId);

        if (personalWiki == null)
            return new PersonalWikiResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Default
            };

        if (personalWiki.ChildRelations.Any(r => r.ChildId == id))
        {
            return new PersonalWikiResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Category.IsAlreadyLinkedAsChild
            };
        }

        var childModifier = new ChildModifier(
            _permissionCheck,
            _sessionUser,
            _categoryRepository,
            _userWritingRepo,
            _httpContextAccessor,
            _webHostEnvironment,
            _categoryRelationRepo);
        return new PersonalWikiResult
        {
            Success = true,
            Data = await childModifier
                .AddChildAsync(id, personalWiki.Id)
                .ConfigureAwait(false)
        };
    }

    public readonly record struct RemoveFromPersonalWikiResult(
        bool Success,
        ChildModifier.RemoveParentResult Data,
        string MessageKey);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<RemoveFromPersonalWikiResult> RemoveFromPersonalWiki([FromRoute] int id)
    {
        var personalWiki = EntityCache.GetCategory(_sessionUser.User.StartTopicId);

        if (personalWiki == null)
            return new RemoveFromPersonalWikiResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Default
            };

        if (personalWiki.ChildRelations.Any(r => r.ChildId != id))
        {
            return new RemoveFromPersonalWikiResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Category.IsNotAChild
            };
        }

        var childModifier = new ChildModifier(_permissionCheck,
            _sessionUser,
            _categoryRepository,
            _userWritingRepo,
            _httpContextAccessor,
            _webHostEnvironment,
            _categoryRelationRepo);

        return new RemoveFromPersonalWikiResult
        {
            Success = true,
            Data = await childModifier
                .RemoveParentAsync(personalWiki.Id, id)
                .ConfigureAwait(false)
        };
    }
}