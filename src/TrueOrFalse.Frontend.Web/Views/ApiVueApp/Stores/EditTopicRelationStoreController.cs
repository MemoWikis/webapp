﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Exception = System.Exception;

namespace VueApp;

public class EditTopicRelationStoreController(
    SessionUser _sessionUser,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    IHttpContextAccessor _httpContextAccessor,
    EditControllerLogic _editControllerLogic,
    QuestionReadingRepo _questionReadingRepo,
    PermissionCheck _permissionCheck,
    CategoryRepository _categoryRepository,
    CategoryRelationRepo _categoryRelationRepo)
    : BaseController(_sessionUser)
{
    public record struct PersonalWikiData(
        SearchTopicItem PersonalWiki,
        SearchTopicItem[] RecentlyUsedRelationTargetTopics);

    public record struct GetPersonalWikiDataResult(bool Success, string MessageKey, PersonalWikiData? Data);
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
            .FillSearchTopicItem(personalWiki, UserId);
        var recentlyUsedRelationTargetTopics = new List<SearchTopicItem>();

        if (_sessionUser.User.RecentlyUsedRelationTargetTopicIds.Count > 0)
        {
            foreach (var topicId in _sessionUser.User.RecentlyUsedRelationTargetTopicIds)
            {
                var topicCacheItem = EntityCache.GetCategory(topicId);
                recentlyUsedRelationTargetTopics.Add(new SearchHelper(_imageMetaDataReadingRepo,
                    _httpContextAccessor,
                    _questionReadingRepo)
                    .FillSearchTopicItem(topicCacheItem, UserId));
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
    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult RemoveTopics([FromBody] RemoveTopicsJson json)
    {
        var removedChildrenIds = new List<int>();

        foreach (var childId in json.childIds)
        {
            var result = _editControllerLogic.RemoveParent(json.parentId, childId);
            if (result.success)
                removedChildrenIds.Add(childId);
        }

        return Json(new RequestResult
        {
            success = true,
            data = removedChildrenIds
        });
    }

    public enum TargetPosition
    {
        Before,
        After,
        Inner,
        None
    }
    public readonly record struct MoveTopicJson(int MovingTopicId, int TargetId, TargetPosition Position, int NewParentId, int OldParentId);
    public readonly record struct TryMoveTopicResult(int OldParentId, int NewParentId, MoveTopicJson UndoMove);
    private TryMoveTopicResult TryMoveTopic(MoveTopicJson json)
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
            throw new InvalidOperationException(FrontendMessageKeys.Error.Category.CircularReference);

        var relationToMove = EntityCache.GetCategory(json.OldParentId)?.ChildRelations.FirstOrDefault(r => r.ChildId == json.MovingTopicId);

        if (relationToMove == null)
        {
            Logg.r.Error("CategoryRelations - MoveTopic: no relation found to move - movingTopicId:{0}, parentId:{1}", json.MovingTopicId, json.OldParentId);
            throw new InvalidOperationException(FrontendMessageKeys.Error.Default);
        }

        var undoMoveTopicData = GetUndoMoveTopicData(relationToMove, json.NewParentId, json.TargetId);

        var modifyRelationsForCategory = new ModifyRelationsForCategory(_categoryRepository, _categoryRelationRepo);

        if (json.Position == TargetPosition.Before)
            TopicOrderer.MoveBefore(relationToMove, json.TargetId, json.NewParentId, _sessionUser.UserId,
                modifyRelationsForCategory);
        else if (json.Position == TargetPosition.After)
            TopicOrderer.MoveAfter(relationToMove, json.TargetId, json.NewParentId, _sessionUser.UserId, modifyRelationsForCategory);
        else if (json.Position == TargetPosition.Inner)
            TopicOrderer.MoveIn(relationToMove, json.TargetId, _sessionUser.UserId, modifyRelationsForCategory, _permissionCheck);
        else if (json.Position == TargetPosition.None)
                throw new InvalidOperationException(FrontendMessageKeys.Error.Default);

        return new TryMoveTopicResult(json.OldParentId, json.NewParentId, undoMoveTopicData);
    }

    public readonly record struct MoveTopicResult(bool Success, TryMoveTopicResult? Data, string Error);
    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public MoveTopicResult MoveTopic([FromBody] MoveTopicJson json)
    {
        try
        {
            var result = TryMoveTopic(json);
            return new MoveTopicResult(true, result, "");
        }
        catch (Exception ex)
        {
            return new MoveTopicResult(false, null, ex.Message);
        }
    }

    private MoveTopicJson GetUndoMoveTopicData(CategoryCacheRelation relation, int newParentId, int targetId)
    {
        if (relation.PreviousId != null && _permissionCheck.CanViewCategory((int)relation.PreviousId))
            return new MoveTopicJson(relation.ChildId, (int)relation.PreviousId, TargetPosition.After, relation.ParentId,
                newParentId);

        if (relation.NextId != null && _permissionCheck.CanViewCategory((int)relation.NextId))
            return new MoveTopicJson(relation.ChildId, (int)relation.NextId, TargetPosition.Before, relation.ParentId,
                newParentId);
        
        return new MoveTopicJson(relation.ChildId, relation.ParentId, TargetPosition.Inner, targetId, targetId);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult AddToPersonalWiki([FromRoute] int id)
    {
        var personalWiki = EntityCache.GetCategory(_sessionUser.User.StartTopicId);

        if (personalWiki == null)
            return Json(new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Default
            });

        if (personalWiki.ChildRelations.Any(r => r.ChildId == id))
        {
            return Json(new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Category.IsAlreadyLinkedAsChild
            });
        }

        return Json(new RequestResult
        {
            success = true,
            data = _editControllerLogic.AddChild(id, personalWiki.Id)
        });
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult RemoveFromPersonalWiki([FromRoute] int id)
    {
        var personalWiki = EntityCache.GetCategory(_sessionUser.User.StartTopicId);

        if (personalWiki == null)
            return Json(new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Default
            });

        if (personalWiki.ChildRelations.Any(r => r.ChildId != id))
        {
            return Json(new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Category.IsNotAChild
            });
        }

        return Json(new RequestResult
        {
            success = true,
            data = _editControllerLogic.RemoveParent(personalWiki.Id, id)
        });
    }
}