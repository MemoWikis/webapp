using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Web;
using TrueOrFalse.Core.Web.Uris;

namespace TrueOrFalse.Frontend.Web.Code
{
    public static class Links
    {
        public const string UserProfileController = "UserProfile";
        public const string UserProfile = "Profile";

        /*Question*/
        public const string Questions = "Questions";
        public const string QuestionsController = "Questions";
        public const string FilterQuestions = "Filter";

        public const string AnswerQuestionSubmit = "AnswerSubmit";

        public static string AnswerQuestion(UrlHelper url, Question question)
        {
            return url.Action("Answer", Links.AnswerQuestionController, new { text = UriSegmentFriendlyQuestion.Run(question.Text), id = question.Id }, null);
        }

        public static string Profile(UrlHelper url, string userName, int userId)
        {
            return url.Action(Links.UserProfile, Links.UserProfileController, new { name = UriSegmentFriendlyUser.Run(userName), id = userId }, null);
        }

        public static string SendAnswer(UrlHelper url, Question question)
        {
            return url.Action("SendAnswer", Links.AnswerQuestionController, new { id = question.Id }, null);
        }

        public static string GetAnswer(UrlHelper url, Question question)
        {
            return url.Action("GetAnswer", Links.AnswerQuestionController, new { id = question.Id }, null);
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
        public const string CreateCategory = "Create";
        public const string EditCategory = "Edit";
        public const string DeleteCategory = "Delete";

        /* Category/Edit/Subcategories */
        public const string AddSubCategoryRow = "AddSubCategoryRow";
        public const string EditSubCategoryItems = "Edit";
        public const string EditSubCategoryItemsController = "EditSubCategoryItems";
        public const string AddSubCategoryItemRow = "AddSubCategoryItemRow";
    
        /**/
        public const string WelcomeController = "Welcome";
        public const string Register = "Register";
        public const string RegisterSuccess = "RegisterSuccess";
        public const string Login = "Login";

        public const string VariousController = "VariousPublic";
        public const string NotDoneYet = "NotDoneYet";
        public const string Impressum = "Imprint";
        public const string WelfareCompany = "WelfareCompany";

        public const string KnowledgeController = "Knowledge";
        public const string Knowledge = "Knowledge";

        public const string News = "News";
        public const string NewsController = "News";

        public const string AccountController = "Account";
        public const string Logout = "Logout";

    }
}
