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
        public JsonResult ValidateName([FromBody] NameHelper nameHelper)
        {
            var data = _editControllerLogic.ValidateName(nameHelper.Name);
            return Json(data);
        }

        [AccessOnlyAsLoggedIn]
        [HttpPost]
        public JsonResult QuickCreate([FromBody] QuickCreatHelper quickCreatHelper)
        {
            var data = _editControllerLogic.QuickCreate(quickCreatHelper.Name,
                    quickCreatHelper.ParentTopicId, 
                    _sessionUser);

            return Json(data);
        }

        [AccessOnlyAsLoggedIn]
        [HttpPost]
        public async Task<JsonResult> SearchTopic(string term, int[] topicIdsToFilter = null)
        {
            var data = _editControllerLogic.SearchTopic(term, topicIdsToFilter);
            return Json(data);
        }

        [AccessOnlyAsLoggedIn]
        [HttpPost]
        public async Task<JsonResult> SearchTopicInPersonalWiki(string term, int[] topicIdsToFilter = null)
        {
            var data = _editControllerLogic.SearchTopicInPersonalWiki(term, topicIdsToFilter);
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
    public class NameHelper
    {
        public string Name { get; set; }
    }

    public class QuickCreatHelper
    {
        public string Name { get; set; }
        public int ParentTopicId { get; set; } 
    }
}