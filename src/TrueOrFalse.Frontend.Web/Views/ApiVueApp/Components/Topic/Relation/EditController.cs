using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VueApp
{
    public class TopicRelationEditController : BaseController
    {
        private readonly EditControllerLogic _editControllerLogic;

        public TopicRelationEditController(IGlobalSearch search,
            SessionUser sessionUser,
            EditControllerLogic editControllerLogic) : base(sessionUser)
        {
            _sessionUser = sessionUser;
            _editControllerLogic = editControllerLogic;
        }

        public readonly record struct ValidateNameParam(string Name);
        [HttpPost]
        public JsonResult ValidateName([FromBody] ValidateNameParam param)
        {
            var data = _editControllerLogic.ValidateName(param.Name);
            return Json(data);
        }

        public readonly record struct QuickCreateParam(string Name, int ParentTopicId);
        [AccessOnlyAsLoggedIn]
        [HttpPost]
        public JsonResult QuickCreate([FromBody] QuickCreateParam param)
        {
            var data = _editControllerLogic.QuickCreate(param.Name,
                    param.ParentTopicId, 
                    _sessionUser);

            return Json(data);
        }

        public readonly record struct SearchParam(string term, int[] topicIdsToFilter);
        [AccessOnlyAsLoggedIn]
        [HttpPost]
        public async Task<JsonResult> SearchTopic([FromBody] SearchParam param)
        {
            var data = _editControllerLogic.SearchTopic(param.term, param.topicIdsToFilter);
            return Json(data);
        }

        [AccessOnlyAsLoggedIn]
        [HttpPost]
        public async Task<JsonResult> SearchTopicInPersonalWiki([FromBody] SearchParam param)
        {
            var data = _editControllerLogic.SearchTopicInPersonalWiki(param.term, param.topicIdsToFilter);
            return Json(data);
        }

        [AccessOnlyAsLoggedIn]
        [HttpPost]
        public JsonResult MoveChild(int childId, int parentIdToRemove, int parentIdToAdd)
        {
            var data = _editControllerLogic.MoveChild(childId, parentIdToRemove, parentIdToAdd);
            return Json(data);
        }

        public readonly record struct AddChildParam(int ChildId, int ParentId, int ParentIdToRemove, bool AddIdToWikiHistory);

        [AccessOnlyAsLoggedIn]
        [HttpPost]
        public JsonResult AddChild([FromBody] AddChildParam param)
        {
            var data = _editControllerLogic
                .AddChild(param.ChildId,
                    param.ParentId,
                    param.ParentIdToRemove,
                    param.AddIdToWikiHistory);

            return Json(data);
        }

        [AccessOnlyAsLoggedIn]
        [HttpPost]
        public JsonResult RemoveParent(int parentIdToRemove, int childId, int[] affectedParentIdsByMove = null)
        {
            var data = _editControllerLogic.RemoveParent(parentIdToRemove, childId, affectedParentIdsByMove);
            return Json(data);
        }
    }
}