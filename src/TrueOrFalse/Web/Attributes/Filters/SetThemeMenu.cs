using System.Linq;

namespace System.Web.Mvc
{
    public class SetThemeMenu : ActionFilterAttribute
    {
        private readonly bool _isCategoryPage;
        private readonly bool _isQuestionSetPage;
        private readonly bool _isQuestionPage;

        public SetThemeMenu(bool isCategoryPage = false, bool isQuestionSetPage = false, bool isQuestionPage = false)
        {
            _isCategoryPage = isCategoryPage;
            _isQuestionSetPage = isQuestionSetPage;
            _isQuestionPage = isQuestionPage;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userSession = new SessionUiData();
            userSession.TopicMenu.IsActive = true;

            if (_isCategoryPage || _isQuestionSetPage || _isQuestionPage)
            {
                var httpContextData = HttpContext.Current.Request.RequestContext.RouteData.Values;

                Category currentCategory = null;

                if (_isCategoryPage)
                    currentCategory = Sl.CategoryRepo.GetById(Convert.ToInt32(httpContextData["id"]));

                if (_isQuestionSetPage)
                {
                    var currentSet = Sl.SetRepo.GetById(Convert.ToInt32(httpContextData["id"]));
                    currentCategory = currentSet.Categories.First();
                }

                if (_isQuestionPage)
                {
                    var currentQuestion = Sl.QuestionRepo.GetById(Convert.ToInt32(httpContextData["id"]));
                    currentCategory = currentQuestion.Categories.First();
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