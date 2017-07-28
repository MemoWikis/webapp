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

                var currentCategoies = new List<Category>();

                if (_isCategoryPage)
                    currentCategoies.Add(Sl.CategoryRepo.GetById(Convert.ToInt32(httpContextData["id"])));

                if (_isQuestionSetPage)
                {
                    var currentSet = Sl.SetRepo.GetById(Convert.ToInt32(httpContextData["id"]));
                    currentCategoies.AddRange(currentSet.Categories);
                }

                if (_isQuestionPage)
                {
                    currentCategoies.AddRange(httpContextData["setId"] != null
                        ? Sl.SetRepo.GetById(Convert.ToInt32(httpContextData["setId"])).Categories
                        : ThemeMenuHistoryOps.GetQuestionCategories(Convert.ToInt32(httpContextData["id"])));
                }

                if (_isTestSessionPage)
                {
                    var testSession = GetTestSession.Get(Convert.ToInt32(httpContextData["testSessionId"]));
                    if(testSession.CategoryToTest != null)
                        currentCategoies.Add(testSession.CategoryToTest);
                    else
                        currentCategoies.AddRange(testSession.SetToTest.Categories);
                }

                if (_isLearningSessionPage)
                {
                    var learningSession = Sl.LearningSessionRepo.GetById(Convert.ToInt32(httpContextData["learningSessionId"]));
                    if(learningSession.CategoryToLearn != null)
                        currentCategoies.Add(learningSession.CategoryToLearn);
                    else
                        currentCategoies.AddRange(learningSession.SetToLearn.Categories);
                }
                userSession.TopicMenu.ActiveCategories = currentCategoies;
            }

            base.OnActionExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var userSession = new SessionUiData();
            userSession.TopicMenu.ActiveCategories = null;

            base.OnResultExecuted(filterContext);
        }
    }
}