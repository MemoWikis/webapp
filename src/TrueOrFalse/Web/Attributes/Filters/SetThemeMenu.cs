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

                Category currentCategory = null;

                if (_isCategoryPage)
                    currentCategory = Sl.CategoryRepo.GetById(Convert.ToInt32(httpContextData["id"]));

                if (_isQuestionSetPage)
                {
                    currentCategory = ThemeMenuHistoryOps.GetQuestionSetCategory(Convert.ToInt32(httpContextData["id"]));
                }

                if (_isQuestionPage)
                {
                    currentCategory = httpContextData["setId"] != null 
                        ? ThemeMenuHistoryOps.GetQuestionSetCategory(Convert.ToInt32(httpContextData["setId"])) 
                        : ThemeMenuHistoryOps.GetQuestionCategory(Convert.ToInt32(httpContextData["id"]));
                }

                if (_isTestSessionPage)
                {
                    currentCategory = ThemeMenuHistoryOps.GetTestSessionCategory(Convert.ToInt32(httpContextData["testSessionId"]));
                }
                if (_isLearningSessionPage)
                {
                    currentCategory = ThemeMenuHistoryOps.GetLearningSessionCategory(Convert.ToInt32(httpContextData["learningSessionId"]));
                }
                userSession.TopicMenu.ActiveCategory = currentCategory;
            }

            base.OnActionExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var userSession = new SessionUiData();
            userSession.TopicMenu.ActiveCategory = null;

            base.OnResultExecuted(filterContext);
        }
    }
}