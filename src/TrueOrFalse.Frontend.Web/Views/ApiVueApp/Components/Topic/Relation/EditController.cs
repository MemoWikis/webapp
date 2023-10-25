using System.Threading.Tasks;
using HelperClassesControllers;
using Microsoft.AspNetCore.Mvc;

namespace VueApp
{
    public class TopicRelationEditController : Controller
    {
        private readonly SessionUser _sessionUser;
        private readonly EditControllerLogic _editControllerLogic;

        public TopicRelationEditController(IGlobalSearch search,
            SessionUser sessionUser,
            EditControllerLogic editControllerLogic)
        {
            _sessionUser = sessionUser;
            _editControllerLogic = editControllerLogic;
        }

        [HttpPost]
        public JsonResult ValidateName([FromBody] ValidateNameJson json)
        {
            var data = _editControllerLogic.ValidateName(json.Name);
            return Json(data);
        }

        [AccessOnlyAsLoggedIn]
        [HttpPost]
        public JsonResult QuickCreate([FromBody] QuickCreateJson quickCreateJson)
        {
            var data = _editControllerLogic.QuickCreate(quickCreateJson.Name,
                    quickCreateJson.ParentTopicId, 
                    _sessionUser);

            return Json(data);
        }

        [AccessOnlyAsLoggedIn]
        [HttpPost]
        public async Task<JsonResult> SearchTopic([FromBody] SearchJson json)
        {
            var data = _editControllerLogic.SearchTopic(json.term, json.topicIdsToFilter);
            return Json(data);
        }

        [AccessOnlyAsLoggedIn]
        [HttpPost]
        public async Task<JsonResult> SearchTopicInPersonalWiki([FromBody] SearchJson json)
        {
            var data = _editControllerLogic.SearchTopicInPersonalWiki(json.term, json.topicIdsToFilter);
            return Json(data);
        }

        [AccessOnlyAsLoggedIn]
        [HttpPost]
        public JsonResult MoveChild(int childId, int parentIdToRemove, int parentIdToAdd)
        {
            var data = _editControllerLogic.MoveChild(childId, parentIdToRemove, parentIdToAdd);
            return Json(data);
        }

        [AccessOnlyAsLoggedIn]
        [HttpPost]
        public JsonResult AddChild(int childId, int parentId, int parentIdToRemove = -1,
            bool addIdToWikiHistory = false)
        {
            var data = _editControllerLogic.AddChild(childId, parentId, parentIdToRemove, addIdToWikiHistory);
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

namespace HelperClassesControllers
{
    public class ValidateNameJson
    {
        public string Name { get; set; }
    }

    public class QuickCreateJson
    {
        public string Name { get; set; }
        public int ParentTopicId { get; set; } 
    }

    public class SearchJson
    {
        public string term { get; set; }
        public int[] topicIdsToFilter { get; set; } = null;
    }
}