using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TrueOrFalse.Web;
using TrueOrFalse.Web.Uris;

namespace TrueOrFalse.Frontend.Web.Code
{
    public static class Links
    {

        /**/
        public const string WelcomeController = "Welcome";

        public const string VariousController = "VariousPublic";
        public const string Impressum = "Impressum";
        public const string TermsAndConditions = "AGB";
        public const string WelfareCompany = "Gemeinwohlökonomie";

        public const string KnowledgeController = "Knowledge";
        public const string Knowledge = "Knowledge";

        public const string HelpController = "Help";
        public const string HelpFAQ = "FAQ";
        public const string HelpWillkommen = "Willkommen";
        public const string HelpWunschwissen = "Willkommen";

        public const string AccountController = "Account";
        public const string Register = "Register";
        public const string RegisterSuccess = "RegisterSuccess";
        public const string Login = "Login";
        public const string Logout = "Logout";
        public const string Membership = "Membership";

        public static UrlHelper GetUrlHelper()
        {
            return new UrlHelper(HttpContext.Current.Request.RequestContext);
        }


        /*Users*/
        public const string UserController = "User";
        public const string User = "User";

        public const string UserSettings = "UserSettings";
        public const string UserSettingsController = "UserSettings";

        public static string UserLoginAs(UrlHelper url, int userId){
            return url.Action("LoginAs", "Users", new {userId = userId});
        }

        public static string UserDetail(User user)
        {
            return UserDetail(user.Name, user.Id);
        }

        public static string UserDetail(string userName, int userId)
        {
            return GetUrlHelper().Action(User, UserController,
                new { name = UriSegmentFriendlyUser.Run(userName), id = userId }, null);
        }

        public static string UserDetailBadges(User user)
        {
            return GetUrlHelper().Action("Badges", UserController,
                new { name = UriSegmentFriendlyUser.Run(user.Name), id = user.Id }, null);
        }

        public static string LoginUrl()
        {
            return GetUrlHelper().Action(Login, VariousController);
        }

        public static string RegisterUrl()
        {
            return GetUrlHelper().Action(Login, VariousController);
        }



        /*Question*/
        public const string Questions = "Questions";
        public const string QuestionsController = "Questions";
        public const string QuestionsMineAction = "QuestionsMine";
        public const string QuestionsWishAction = "QuestionsWish";
        public const string EditQuestionController = "EditQuestion";

        public const string AnswerQuestionController = "AnswerQuestion";


        public static string QuestionsAll() { return GetUrlHelper().Action(Questions, QuestionsController); }
        public static string QuestionsMine() { return GetUrlHelper().Action(QuestionsMineAction, QuestionsController); }
        public static string QuestionsWish() { return GetUrlHelper().Action(QuestionsWishAction, QuestionsController); }

        public static string QuestionWithCategoryFilter(UrlHelper url, MenuModelCategoryItem modelCategoryItem){
            return modelCategoryItem.SearchUrl + "Kat__" + modelCategoryItem.Category.Name + "__";
        }

        public static string QuestionWithCategoryFilter(UrlHelper url, Category category){
            return "/Fragen/Suche/Kategorie/" + UriSanitizer.Run(category.Name) + "/" + category.Id;
        }

        public static string QuestionWithCategoryFilter(UrlHelper url, string categoryName, int categoryId)
        {
            return "/Fragen/Suche/Kategorie/" + UriSanitizer.Run(categoryName) + "/" + categoryId;
        }

        public static string QuestionWish_WithCategoryFilter(Category category){
            return "/Fragen/Wunschwissen/Suche/Kategorie/" + UriSanitizer.Run(category.Name) + "/" + category.Id;
        }

        public static string QuestionWithCreatorFilter(UrlHelper url, User user){
            return "/Fragen/Suche/" + "Ersteller__" + user.Name + "__";
        }

        public static string AnswerQuestion(UrlHelper url, Question question, Set set){
            return url.Action("Answer", AnswerQuestionController, 
                new { text = UriSegmentFriendlyQuestion.Run(question.Text), questionId = question.Id, setId = set.Id });
        }
        public static string AnswerQuestion(UrlHelper url, string questionText, int questionId, int setId)
        {
            return url.Action("Answer", AnswerQuestionController,
                new { text = UriSegmentFriendlyQuestion.Run(questionText), questionId, setId});
        }

        public static string AnswerQuestion(UrlHelper url, QuestionSearchSpec searchSpec){
            return "/AnswerQuestion/Answer?pager=" + searchSpec.Key;
        }

        public static string AnswerQuestion(UrlHelper url, Question question, int paramElementOnPage = 1, string pagerKey = "", string categoryFilter = ""){
            return AnswerQuestion(url, question.Text, question.Id, paramElementOnPage, pagerKey, categoryFilter);
        }

        public static string AnswerQuestion(Question question)
        {
            return AnswerQuestion(GetUrlHelper(), question, -1);
        }

      

        public static string AnswerQuestion(UrlHelper url, string questionText, int questionId, int paramElementOnPage = 1, string pagerKey = "", string categoryFilter = "")
        {
            if (paramElementOnPage == -1)
            {
                return url.Action("Answer", AnswerQuestionController,
                    new
                    {
                        text = UriSegmentFriendlyQuestion.Run(questionText),
                        id = questionId
                    }, null);
            }
            return url.Action("Answer", AnswerQuestionController,
                new {
                    text = UriSegmentFriendlyQuestion.Run(questionText), 
                    id = questionId, 
                    elementOnPage = paramElementOnPage, 
                    pager = pagerKey, 
                    category = categoryFilter
                }, null);
        }

        public static string CreateQuestion(UrlHelper url, int categoryId = -1)
        {
            if (categoryId != -1)
                return url.Action("Create", EditQuestionController, new { categoryId = categoryId });

            return url.Action("Create", EditQuestionController);
        }

        public static string EditQuestion(string questionText, int questionId)
        {
            return EditQuestion(GetUrlHelper(), questionText, questionId);
        }
        public static string EditQuestion(UrlHelper url, string questionText, int questionId)
        {
            return url.Action("Edit", EditQuestionController, new { text = UriSanitizer.Run(questionText), id = questionId });
        }


        public static string SendAnswer(UrlHelper url, Question question){
            return url.Action("SendAnswer", AnswerQuestionController, new { id = question.Id }, null);
        }

        public static string SendAnswer(UrlHelper url, Question question, 
            Game game, Player player, Round round)
        {
            return url.Action("SendAnswerGame", "Play", 
                new{
                    questionId = question.Id, 
                    gameId = game.Id,
                    playerId = player.Id,
                    roundId = round.Id
                }, null);
        }

        public static string SendAnswer(UrlHelper url, Question question,
            LearningSession learningSession, LearningSessionStep learningSessionStep)
        {
            return url.Action("SendAnswerLearningSession", AnswerQuestionController,
                new { id = question.Id, learningSessionId = learningSession.Id, stepGuid = learningSessionStep.Guid }, null);
        }

        public static string GetSolution(UrlHelper url, Question question)
        {
            return url.Action("GetSolution", AnswerQuestionController, new { id = question.Id }, null);
        }

        public static string GetSolution(UrlHelper url, Question question, Round round)
        {
            return url.Action("GetSolution", AnswerQuestionController, new { id = question.Id, roundId = round.Id }, null);
        }

        public static string CountLastAnswerAsCorrect(UrlHelper url, Question question)
        {
            return url.Action("CountLastAnswerAsCorrect", AnswerQuestionController, new { id = question.Id }, null);
        }

        public static string CountUnansweredAsCorrect(UrlHelper url, Question question)
        {
            return url.Action("CountUnansweredAsCorrect", AnswerQuestionController, new { id = question.Id }, null);
        }


        /*Dates*/
        public const string DatesController = "Dates";

        public static string Dates()
        {
            return GetUrlHelper().Action("Dates", "Dates");
        }

        public static object DateEdit(int dateId)
        {
            return GetUrlHelper().Action("Edit", "EditDate", new { dateId = dateId });
        }

        public static object DateCreate(int setId)
        {
            return GetUrlHelper().Action("Create", "EditDate", new { setId = setId });
        }


        /*Learn*/

        public const string LearningSessionResultController = "LearningSessionResult";

        public static string LearningSession(LearningSession learningSession)
        {
            return GetUrlHelper().Action("Learn", AnswerQuestionController,
                new
                {
                    learningSessionId = learningSession.Id,
                    learningSessionName = learningSession.UrlName,
                });
        }

        public static string StartLearningSession(LearningSession learningSession)
        {
            if (learningSession.IsSetSession)
                return StartSetLearningSession(learningSession.SetToLearn.Id);

            if (learningSession.IsDateSession)
                return GetUrlHelper().Action("StartLearningSession", "Dates", new { dateId = learningSession.DateToLearn.Id });

            throw new Exception("unknown type");
        }

        public static string StartSetLearningSession(int setId)
        {
            return GetUrlHelper().Action("StartLearningSession", SetController, new { setId = setId});
        }


        /*Questionsets / Sets*/
        public const string SetsController = "Sets";
        public const string SetsAction = "Sets";
        public const string SetsWishAction = "SetsWish";
        public const string SetsMineAction = "SetsMine";
        public static string Sets() { return GetUrlHelper().Action(SetsAction, SetsController); }
        public static string SetsWish() { return GetUrlHelper().Action(SetsWishAction, SetsController); }
        public static string SetsMine() { return GetUrlHelper().Action(SetsMineAction, SetsController); }
        public static string SetCreate() { return GetUrlHelper().Action(SetCreateAction, SetEditController); }
        public const string SetController = "Set";
        public const string SetCreateAction = "Create";
        public const string SetEditController = "EditSet";


        public static string SetDetail(UrlHelper url, SetMini setMini){
            return SetDetail(url, setMini.Name, setMini.Id);
        }

        public static string SetDetail(UrlHelper url, Set set){
            return SetDetail(url, set.Name, set.Id);
        }

        public static string SetDetail(UrlHelper url, string name, int id){
            return url.Action("QuestionSet", "Set",
                new { text = UriSanitizer.Run(name), id = id}, null);            
        }

        public static string SetDetail(string name, int id)
        {
            return GetUrlHelper().Action("QuestionSet", "Set", new {text = UriSanitizer.Run(name), id = id});
        }

        public static string QuestionSetEdit(string name, int questionSetId)
        {
            return QuestionSetEdit(GetUrlHelper(), name, questionSetId);
        }
        public static string QuestionSetEdit(UrlHelper url, string name, int questionSetId)
        {
            return url.Action("Edit", "EditSet", new { text = UriSanitizer.Run(name), id = questionSetId });
        }



        /* Messages */

        public static string Messages(UrlHelper url){
            return url.Action("Messages","Messages");
        }

        public static string MessageSetRead(UrlHelper url){
            return url.Action("SetMessageRead", "Messages");
        }

        public static object MessageSetUnread(UrlHelper url){
            return url.Action("SetMessageUnread", "Messages");
        }



        /* Games */

        public static string Games(UrlHelper url){
            return url.Action("Games", "Games");
        }

        public static string GameCreateFromDate(int dateId){
            return GetUrlHelper().Action("Create", "Game", new {dateId = dateId});
        }

        public static string GameCreateFromSet(int setId){
            return GetUrlHelper().Action("Create", "Game", new { setId = setId});
        }

        public static string GameCreate(){
            return GetUrlHelper().Action("Create", "Game", null);
        }

        public static string GamePlay(UrlHelper url, int gameId){
            return GetUrlHelper().Action("Play", "Play", new { gameId = gameId });
        }


        /*Category*/
        public const string Categories = "Categories";
        public const string CategoriesController = "Categories";

        public const string EditCategoryController = "EditCategory";
        public const string EditCategory = "Edit";

        public static string CategoryDetail(string name, int id, int elementOnpage = 1)
        {
            return GetUrlHelper().Action("Category", "Category",
                new { text = UriSanitizer.Run(name), id = id, elementOnPage = elementOnpage }, null);
        }

        public static string CategoryDetail(Category category)
        {
            return CategoryDetail(category.Name, category.Id);
        }

        public static object CategoryEdit(UrlHelper url, int id)
        {
            return url.Action("Edit", "EditCategory", new { id });
        }

    }
}
