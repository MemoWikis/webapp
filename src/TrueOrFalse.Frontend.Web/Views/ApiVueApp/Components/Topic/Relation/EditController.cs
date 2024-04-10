using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Antlr.Runtime;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static VueApp.ChildModifier;

namespace VueApp
{
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
        public JsonResult ValidateName([FromBody] ValidateNameParam param)
        {
            var data = ValidateName(param.Name);
            return Json(data);
        }

        public readonly record struct QuickCreateParam(string Name, int ParentTopicId);

        [AccessOnlyAsLoggedIn]
        [HttpPost]
        public JsonResult QuickCreate([FromBody] QuickCreateParam param)
        {
            var data = _categoryCreator.Create(param.Name, param.ParentTopicId, _sessionUser);
            return Json(data);
        }

        public readonly record struct SearchParam(string term, int[] topicIdsToFilter);

        [AccessOnlyAsLoggedIn]
        [HttpPost]
        public async Task<JsonResult> SearchTopic([FromBody] SearchParam param)
        {
            var data = SearchTopic(param.term, param.topicIdsToFilter);
            return Json(data);
        }

        [AccessOnlyAsLoggedIn]
        [HttpPost]
        public async Task<JsonResult> SearchTopicInPersonalWiki([FromBody] SearchParam param)
        {
            var data = SearchTopicInPersonalWiki(param.term, param.topicIdsToFilter);
            return Json(data);
        }

        public readonly record struct MoveChildParam(
            int childId,
            int parentIdToRemove,
            int parentIdToAdd);

        [AccessOnlyAsLoggedIn]
        [HttpPost]
        public JsonResult MoveChild([FromBody] MoveChildParam param)
        {
            var data = MoveChild(param.childId, param.parentIdToRemove,
                param.parentIdToAdd);
            return Json(data);
        }

        public readonly record struct AddChildParam(
            int ChildId,
            int ParentId,
            int ParentIdToRemove,
            bool AddIdToWikiHistory);

        [AccessOnlyAsLoggedIn]
        [HttpPost]
        public JsonResult AddChild([FromBody] AddChildParam param)
        {
            var data =
                new ChildModifier(_permissionCheck,
                        _sessionUser,
                        _categoryRepository,
                        _userWritingRepo,
                        _httpContextAccessor,
                        _webHostEnvironment,
                        _categoryRelationRepo)
                    .AddChild(
                        param.ChildId,
                        param.ParentId,
                        param.ParentIdToRemove,
                        param.AddIdToWikiHistory);

            return Json(data);
        }

        public readonly record struct RemoveParentParam(
            int parentIdToRemove,
            int childId,
            int[] affectedParentIdsByMove = null);

        [AccessOnlyAsLoggedIn]
        [HttpPost]
        public JsonResult RemoveParent([FromBody] RemoveParentParam param)
        {
            var data = new ChildModifier(_permissionCheck,
                    _sessionUser,
                    _categoryRepository,
                    _userWritingRepo,
                    _httpContextAccessor,
                    _webHostEnvironment,
                    _categoryRelationRepo)
                .RemoveParent(
                    param.parentIdToRemove,
                    param.childId,
                    param.affectedParentIdsByMove);
            return Json(data);
        }

        public readonly record struct SearchTopicResult(
            int TotalCount,
            List<SearchTopicItem> Topics);

        private async Task<SearchTopicResult> SearchTopic(
            string term,
            int[] topicIdsToFilter = null)
        {
            var items = new List<SearchTopicItem>();
            var elements = await _search.GoAllCategories(term, topicIdsToFilter);

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

        private async Task<SearchTopicInPersonalWikiResult> SearchTopicInPersonalWiki(
            string term,
            int[] topicIdsToFilter = null)
        {
            var items = new List<SearchTopicItem>();
            var elements = await _search.GoAllCategories(term, topicIdsToFilter);

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
            TinyTopicItem Data);

        public readonly record struct TinyTopicItem(bool CategoryNameAllowed, string name);

        private ValidateNameResult ValidateName(string name)
        {
            var nameValidator = new TopicNameValidator();

            if (nameValidator.IsForbiddenName(name))
            {
                return new ValidateNameResult
                {
                    Success = false,
                    MessageKey = FrontendMessageKeys.Error.Category.NameIsForbidden,
                    Data = new TinyTopicItem
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

        private ChildModifierResult MoveChild(int childId, int parentIdToRemove, int parentIdToAdd)
        {
            if (childId == parentIdToRemove || childId == parentIdToAdd)
                return new ChildModifierResult
                {
                    Success = false,
                    MessageKey = FrontendMessageKeys.Error.Category.LoopLink
                };

            if (parentIdToRemove == RootCategory.RootCategoryId &&
                !_sessionUser.IsInstallationAdmin || parentIdToAdd == RootCategory.RootCategoryId &&
                !_sessionUser.IsInstallationAdmin)
                return new ChildModifierResult
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

            var json = childmodifier
                .AddChild(childId,
                    parentIdToAdd,
                    parentIdToRemove);

            childmodifier.RemoveParent(parentIdToRemove,
                childId,
                new int[] { parentIdToAdd, parentIdToRemove });
            return json;
        }
    }
}