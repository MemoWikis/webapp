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
                    currentCategory = GetQuestionSetCategory(Convert.ToInt32(httpContextData["id"]));
                }

                if (_isQuestionPage)
                {
                    currentCategory = httpContextData["setId"] != null 
                        ? GetQuestionSetCategory(Convert.ToInt32(httpContextData["setId"])) 
                        : GetQuestionCategory(Convert.ToInt32(httpContextData["id"]));
                }

                if (_isTestSessionPage) { }
                if(_isLearningSessionPage) { }
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

        private Category GetQuestionSetCategory(int setId)
        {
            var currentSet = Sl.SetRepo.GetById(setId);
            var currentSetCategories = currentSet.Categories;

            var visitedCategories = Sl.SessionUiData.VisitedCategories;

            Category currentCategory;
            if (visitedCategories.Any())
            {
                currentCategory = currentSetCategories.All(c => c.Id == visitedCategories.First().Id) 
                    ? Sl.CategoryRepo.GetById(visitedCategories.First().Id) 
                    : currentSetCategories.First();
            }
            else
            {
                currentCategory = currentSetCategories.First();
            }

            return currentCategory;
        }

        private Category GetQuestionCategory(int questionId)
        {
            var currentQuestion = Sl.QuestionRepo.GetById(questionId);
            var currentQuestionCategories = currentQuestion.Categories;

            var visitedCategories = Sl.SessionUiData.VisitedCategories;

            Category currentCategory;
            if (visitedCategories.Any())
            {
                currentCategory = currentQuestionCategories.All(c => c.Id == visitedCategories.First().Id) 
                    ? Sl.CategoryRepo.GetById(visitedCategories.First().Id) 
                    : currentQuestionCategories.First();
            }
            else
            {
                currentCategory = currentQuestionCategories.First();
            }

            return currentCategory;
        }

        private Category GetTestSessionCategory(int testSessionId) { }

        private Category GetLearningSessionCategory(int learningSessionId) { }
    }
}