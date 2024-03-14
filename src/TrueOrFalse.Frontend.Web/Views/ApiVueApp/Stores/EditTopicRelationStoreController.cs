using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class EditTopicRelationStoreController : BaseController
{
    private readonly ImageMetaDataReadingRepo _imageMetaDataReadingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly EditControllerLogic _editControllerLogic;
    private readonly QuestionReadingRepo _questionReadingRepo;

    public EditTopicRelationStoreController(SessionUser sessionUser,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        IHttpContextAccessor httpContextAccessor,
        EditControllerLogic editControllerLogic,
        QuestionReadingRepo questionReadingRepo) : base(sessionUser)
    {
        _imageMetaDataReadingRepo = imageMetaDataReadingRepo;
        _httpContextAccessor = httpContextAccessor;
        _editControllerLogic = editControllerLogic;
        _questionReadingRepo = questionReadingRepo;
    }

    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public JsonResult GetPersonalWikiData([FromRoute] int id)
    {
        if (GraphService.Descendants(id).Any(c => c.Id == _sessionUser.User.StartTopicId))
            return Json(new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Category.LoopLink
            });

        var personalWiki = EntityCache.GetCategory(_sessionUser.User.StartTopicId);
        var personalWikiItem = new SearchHelper(_imageMetaDataReadingRepo,
                _httpContextAccessor,
                _questionReadingRepo)
            .FillSearchCategoryItem(personalWiki, UserId);
        var recentlyUsedRelationTargetTopics = new List<SearchCategoryItem>();

        if (_sessionUser.User.RecentlyUsedRelationTargetTopicIds != null && _sessionUser.User.RecentlyUsedRelationTargetTopicIds.Count > 0)
        {
            foreach (var topicId in _sessionUser.User.RecentlyUsedRelationTargetTopicIds)
            {
                var topicCacheItem = EntityCache.GetCategory(topicId);
                recentlyUsedRelationTargetTopics.Add(new SearchHelper(_imageMetaDataReadingRepo,
                    _httpContextAccessor,
                    _questionReadingRepo)
                    .FillSearchCategoryItem(topicCacheItem, UserId));
            }
        }

        return Json(new RequestResult
        {
            success = true,
            data = new
            {
                personalWiki = personalWikiItem,
                recentlyUsedRelationTargetTopics = recentlyUsedRelationTargetTopics.ToArray()
            }
        });
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

    public readonly record struct MoveTopicJson(int movingTopicId, int targetId, string position, int newParentId, int oldParentId);
    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult MoveTopic([FromBody] MoveTopicJson json)
    {
        var relationToMove = EntityCache.GetCategory(json.oldParentId).ChildRelations
            .Where(r => r.ChildId == json.movingTopicId).FirstOrDefault();

        if (relationToMove != null)
        {
            if (json.position == "before")
                ModifyRelationsEntityCache.MoveBefore(relationToMove, json.targetId, json.newParentId,
                    json.oldParentId, _sessionUser.UserId);
            else if (json.position == "after")
                ModifyRelationsEntityCache.MoveAfter(relationToMove, json.targetId, json.newParentId,
                    json.oldParentId, _sessionUser.UserId);
        }

        return Json(new
        {
            movingTopicId = json.movingTopicId,
            targetId = json.targetId,
            position = json.position,
            newParentId = json.newParentId,
            oldParentId = json.oldParentId,
            relationTomove = relationToMove
        });
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

        if (personalWiki.ChildrenIds.Any(cId => cId == id))
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

        if (personalWiki.ChildrenIds.Any(cId => cId != id))
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