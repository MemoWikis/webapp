using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Web;
using TrueOrFalse.Web.Uris;

namespace TrueOrFalse.Frontend.Web.Code
{
    public static class Links
    {
        /*Users*/
        public const string UserController = "User";
        public const string User = "User";

        public const string UserSettings = "UserSettings";
        public const string UserSettingsController = "UserSettings";

        public static string UserLoginAs(UrlHelper url, int userId){
            return url.Action("LoginAs", "Users", new {userId = userId});
        }

        /*Question*/
        public const string Questions = "Questions";
        public const string QuestionsController = "Questions";
        public const string QuestionsMineAction = "QuestionsMine";
        public const string QuestionsWishAction = "QuestionsWish";

        public static string QuestionsAll(UrlHelper url) { return url.Action(Questions, QuestionsController); }
        public static string QuestionsMine(UrlHelper url) { return url.Action(QuestionsMineAction, QuestionsController); }
        public static string QuestionsWish(UrlHelper url) { return url.Action(QuestionsWishAction, QuestionsController); }

        public static string AnswerQuestion(UrlHelper url, Question question, int paramElementOnPage = 1){
            return AnswerQuestion(url, question.Text, question.Id, paramElementOnPage);
        }

        public static string AnswerQuestion(UrlHelper url, string questionText, int questionId, int paramElementOnPage = 1){
            return url.Action("Answer", AnswerQuestionController,
                new { text = UriSegmentFriendlyQuestion.Run(questionText), id = questionId, elementOnPage = paramElementOnPage }, null);
        }

        public static string UserDetail(UrlHelper url, User user){
            return UserDetail(url, user.Name, user.Id);
        }

        public static string UserDetail(UrlHelper url, string userName, int userId){
            return url.Action(User, UserController, 
                new { name = UriSegmentFriendlyUser.Run(userName), id = userId }, null);
        }

        public static string SendAnswer(UrlHelper url, Question question){
            return url.Action("SendAnswer", AnswerQuestionController, new { id = question.Id }, null);
        }   

        public static string GetAnswer(UrlHelper url, Question question){
            return url.Action("GetAnswer", AnswerQuestionController, new { id = question.Id }, null);
        }

        /*Set*/
        public const string SetsController = "Sets";
        public const string SetsAction = "Sets";
        public const string SetsWishAction = "SetsWish";
        public const string SetsMineAction = "SetsMine";
        public static string Sets(UrlHelper url) { return url.Action(SetsAction, SetsController); }
        public static string SetsWish(UrlHelper url) { return url.Action(SetsWishAction, SetsController); }
        public static string SetsMine(UrlHelper url) { return url.Action(SetsMineAction, SetsController); }

        public static string SetDetail(UrlHelper url, Set set, int elementOnPage = 1){
            return SetDetail(url, set.Name, set.Id, elementOnPage);
        }

        public static string SetDetail(UrlHelper url, string name, int id, int elementOnpage = 1){
            return url.Action("QuestionSet", "Set",
                new { text = UriSanitizer.Run(name), id = id, elementOnPage = elementOnpage}, null);            
        }

        public static string CategoryDetail(UrlHelper url, string name, int id, int elementOnpage = 1){
            return url.Action("Category", "Category",
                new { text = UriSanitizer.Run(name), id = id, elementOnPage = elementOnpage }, null);
        }

        public static string CategoryDetail(UrlHelper url, Category category){
            return CategoryDetail(url, category.Name, category.Id);
        }

        public static object CategoryEdit(UrlHelper url, int id){
            return url.Action("Edit", "EditCategory", new { id });
        }

        public static string QuestionSetEdit(UrlHelper url, int questionSetId){
            return url.Action("Edit", "EditSet", new {id = questionSetId});
        }

        public const string EditQuestionController = "EditQuestion"; 
        public const string CreateQuestion = "Create";
        public const string EditQuestion = "Edit";

        public const string AnswerQuestionController = "AnswerQuestion";

        /*Category*/
        public const string Categories = "Categories";
        public const string CategoriesController = "Categories";

        /* Category/Edit */
        public const string EditCategoryController = "EditCategory";
        public const string EditCategory = "Edit";

        /**/
        public const string WelcomeController = "Welcome";
        public const string Register = "Register";
        public const string RegisterSuccess = "RegisterSuccess";
        public const string Login = "Login";

        public const string VariousController = "VariousPublic";
        public const string Impressum = "Imprint";
        public const string WelfareCompany = "WelfareCompany";

        public const string KnowledgeController = "Knowledge";
        public const string Knowledge = "Knowledge";

        public const string HelpController = "Help";
        public const string HelpWillkommen = "Willkommen";

        public const string AccountController = "Account";
        public const string Logout = "Logout";
    }
}
