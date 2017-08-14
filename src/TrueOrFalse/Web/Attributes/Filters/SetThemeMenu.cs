using System.Collections.Generic;
using System.Linq;

namespace System.Web.Mvc
{
    public class SetThemeMenu : ActionFilterAttribute
    {
        private readonly bool _isCategoryPage;
        private readonly bool _isQuestionSetPage;
        private readonly bool _isQuestionPage;
        private readonly bool _isTestSessionPage;
        private readonly bool _isLearningSessionPage;

        public SetThemeMenu(bool isCategoryPage = false, bool isQuestionSetPage = false, bool isQuestionPage = false, bool isTestSessionPage = false, bool isLearningSessionPage = false)
        {
            _isCategoryPage = isCategoryPage;
            _isQuestionSetPage = isQuestionSetPage;
            _isQuestionPage = isQuestionPage;
            _isTestSessionPage = isTestSessionPage;
            _isLearningSessionPage = isLearningSessionPage;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userSession = new SessionUiData();
            userSession.TopicMenu.IsActive = true;

            if (_isCategoryPage || _isQuestionSetPage || _isQuestionPage || _isTestSessionPage || _isLearningSessionPage)
            {
                var httpContextData = HttpContext.Current.Request.RequestContext.RouteData.Values;

                var activeCategories = new List<Category>();

                if (_isCategoryPage)
                    activeCategories.Add(Sl.CategoryRepo.GetByIdEager(Convert.ToInt32(httpContextData["id"])));

                if (_isQuestionSetPage)
                {
                    var currentSet = Sl.SetRepo.GetById(Convert.ToInt32(httpContextData["id"]));
                    activeCategories.AddRange(currentSet.Categories);
                }

                if (_isQuestionPage)
                {
                    activeCategories.AddRange(httpContextData["setId"] != null
                        ? Sl.SetRepo.GetById(Convert.ToInt32(httpContextData["setId"])).Categories
                        : ThemeMenuHistoryOps.GetQuestionCategories(Convert.ToInt32(httpContextData["id"])));
                }

                if (_isTestSessionPage)
                {
                    var testSession = GetTestSession.Get(Convert.ToInt32(httpContextData["testSessionId"]));
                    if(testSession.CategoryToTest != null)
                        activeCategories.Add(EntityCache.GetCategory(testSession.CategoryToTest.Id));
                    else
                        activeCategories.AddRange(
                            //get eager loaded categories from cache
                            EntityCache.GetCategories(testSession.SetToTest.Categories.GetIds())
                        );
                }

                if (_isLearningSessionPage)
                {
                    var learningSession = Sl.LearningSessionRepo.GetById(Convert.ToInt32(httpContextData["learningSessionId"]));
                    if(learningSession.CategoryToLearn != null)
                        activeCategories.Add(learningSession.CategoryToLearn);
                    else if (learningSession.SetToLearn != null)
                        activeCategories.AddRange(learningSession.SetToLearn.Categories);
                    else
                    {
                        if (learningSession.CurrentLearningStepIdx() != -1)
                            activeCategories.AddRange(learningSession.Steps[learningSession.CurrentLearningStepIdx()]
                                .Question.Categories);
                    }
                        

                }
                userSession.TopicMenu.PageCategories = activeCategories;
            }

            base.OnActionExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var userSession = new SessionUiData();
            userSession.TopicMenu.PageCategories = null;

            base.OnResultExecuted(filterContext);
        }
    }
}