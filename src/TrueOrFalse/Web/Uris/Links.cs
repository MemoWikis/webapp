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
        public const string UserProfileController = "UserProfile";
        public const string UserProfile = "Profile";

        /*Question*/
        public const string Questions = "Questions";
        public const string QuestionsController = "Questions";

        public static string AnswerQuestion(UrlHelper url, Question question, int paramElementOnPage){
            return AnswerQuestion(url, question.Text, question.Id, paramElementOnPage);
        }

        public static string AnswerQuestion(UrlHelper url, string questionText, int questionId, int paramElementOnPage = 1){
            return url.Action("Answer", AnswerQuestionController,
                new { text = UriSegmentFriendlyQuestion.Run(questionText), id = questionId, elementOnPage = paramElementOnPage }, null);
        }

        public static string Profile(UrlHelper url, User user){
            return Profile(url, user.Name, user.Id);
        }

        public static string Profile(UrlHelper url, string userName, int userId){
            return url.Action(UserProfile, UserProfileController, 
                new { name = UriSegmentFriendlyUser.Run(userName), id = userId }, null);
        }

        public static string SendAnswer(UrlHelper url, Question question){
            return url.Action("SendAnswer", AnswerQuestionController, new { id = question.Id }, null);
        }   

        public static string GetAnswer(UrlHelper url, Question question){
            return url.Action("GetAnswer", AnswerQuestionController, new { id = question.Id }, null);
        }

        public static string QuestionSetDetail(UrlHelper url, QuestionSet set, int elementOnPage){
            return QuestionSetDetail(url, set.Name, set.Id, elementOnPage);
        }

        public static string QuestionSetDetail(UrlHelper url, string name, int id, int elementOnpage = 1){
            return url.Action("QuestionSet", "QuestionSet",
                new { text = UriSanitizer.Run(name), id = id, elementOnPage = elementOnpage}, null);            
        }

        public static string CategoryDetail(UrlHelper url, string name, int id, int elementOnpage = 1){
            return url.Action("Category", "Category",
                new { text = UriSanitizer.Run(name), id = id, elementOnPage = elementOnpage }, null);
        }

        public static object CategoryEdit(UrlHelper url, int id){
            return url.Action("Edit", "EditCategory", new { id });
        }

        public static string QuestionSetEdit(UrlHelper url, int questionSetId){
            return url.Action("Edit", "QuestionSet", new {id = questionSetId});
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

        public const string AccountController = "Account";
        public const string Logout = "Logout";
    }
}
