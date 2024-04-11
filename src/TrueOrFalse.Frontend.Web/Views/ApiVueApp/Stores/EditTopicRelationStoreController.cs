﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Exception = System.Exception;

namespace VueApp;

public class EditTopicRelationStoreController : BaseController
{
    private readonly ImageMetaDataReadingRepo _imageMetaDataReadingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly EditControllerLogic _editControllerLogic;
    private readonly QuestionReadingRepo _questionReadingRepo;
    private readonly PermissionCheck _permissionCheck;
    private readonly CategoryRepository _categoryRepository;
    private readonly CategoryRelationRepo _categoryRelationRepo;

    public EditTopicRelationStoreController(SessionUser sessionUser,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        IHttpContextAccessor httpContextAccessor,
        EditControllerLogic editControllerLogic,
        QuestionReadingRepo questionReadingRepo,
        PermissionCheck permissionCheck, CategoryRepository categoryRepository, CategoryRelationRepo categoryRelationRepo) : base(sessionUser)
    {
        _imageMetaDataReadingRepo = imageMetaDataReadingRepo;
        _httpContextAccessor = httpContextAccessor;
        _editControllerLogic = editControllerLogic;
        _questionReadingRepo = questionReadingRepo;
        _permissionCheck = permissionCheck;
        _categoryRepository = categoryRepository;
        _categoryRelationRepo = categoryRelationRepo;
    }

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

        if (_sessionUser.User.RecentlyUsedRelationTargetTopicIds != null && _sessionUser.User.RecentlyUsedRelationTargetTopicIds.Count > 0)
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
    public readonly record struct MoveTopicJson(int movingTopicId, int targetId, TargetPosition position, int newParentId, int oldParentId);
    public readonly record struct MoveTopicResult(int oldParentId, int newParentId, MoveTopicJson undoMove);
    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public MoveTopicResult MoveTopic([FromBody] MoveTopicJson json)
    {
        if (!_sessionUser.IsLoggedIn)
            throw new SecurityException("NotLoggedIn");

        if (!_permissionCheck.CanMoveTopic(json.movingTopicId, json.oldParentId))
            throw new SecurityException("NoRights");

        if (json.movingTopicId == json.newParentId)
            throw new InvalidOperationException("CircularReference");

        var relationToMove = EntityCache.GetCategory(json.oldParentId)?.ChildRelations.FirstOrDefault(r => r.ChildId == json.movingTopicId);

        if (relationToMove == null)
        {
            Logg.r.Error("CategoryRelations - MoveTopic: no relation found to move - movingTopicId:{0}, parentId:{1}", json.movingTopicId, json.oldParentId);
            throw new InvalidOperationException("NoRelationToMove");
        }

        var undoMoveTopicData = GetUndoMoveTopicData(relationToMove, json.newParentId, json.targetId);

        var modifyRelationsForCategory = new ModifyRelationsForCategory(_categoryRepository, _categoryRelationRepo);

        if (json.position == TargetPosition.Before)
            ModifyRelationsEntityCache.MoveBefore(relationToMove, json.targetId, json.newParentId, _sessionUser.UserId,
                modifyRelationsForCategory);
        else if (json.position == TargetPosition.After)
            ModifyRelationsEntityCache.MoveAfter(relationToMove, json.targetId, json.newParentId, _sessionUser.UserId, modifyRelationsForCategory);
        else if (json.position == TargetPosition.Inner)
            ModifyRelationsEntityCache.MoveIn(relationToMove, json.targetId, _sessionUser.UserId, modifyRelationsForCategory, _permissionCheck);
        else if (json.position == TargetPosition.None)
                throw new InvalidOperationException("NoPosition");

        return new MoveTopicResult(json.oldParentId, json.newParentId, undoMoveTopicData);
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