using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TrueOrFalse
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // das hier später per Konvention, siehe: http://mvccontrib.codeplex.com/SourceControl/changeset/view/351a6de404cb#src%2fMVCContrib%2fSimplyRestful%2fSimplyRestfulRouteHandler.cs

            routes.MapRoute("Questions", "Questions", new { controller = "Questions", action = "Questions" });
            routes.MapRoute("Questions_SearchTerm", "Questions/Search/{searchTerm}", new { controller = "Questions", action = "QuestionSearch", searchTerm = UrlParameter.Optional });
            routes.MapRoute("Questions_DeleteDetails", "Questions/DeleteDetails/{questionId}", new { controller = "Questions", action = "DeleteDetails" });
            routes.MapRoute("Questions_Delete", "Questions/Delete/{questionId}", new { controller = "Questions", action = "Delete" });
            routes.MapRoute("Question_Create", "Questions/Create/", new { controller = "EditQuestion", action = "Create" });
            routes.MapRoute("Question_Edit", "Questions/Edit/{id}", new { controller = "EditQuestion", action = "Edit" });
            routes.MapRoute("Question_Delete", "Questions/Delete/{id}", new { controller = "Questions", action = "Delete" });

            routes.MapRoute("Question_Answer", "Questions/Answer/{text}/{id}/{elementOnPage}", new { controller = "AnswerQuestion", action = "Answer" });
            routes.MapRoute("Question_SendAnswer", "Questions/SendAnswer/{id}", new { controller = "AnswerQuestion", action = "SendAnswer" });
            routes.MapRoute("Question_GetAnswer", "Questions/GetAnswer/{id}", new { controller = "AnswerQuestion", action = "GetAnswer" });
            routes.MapRoute("Question_SaveQuality", "Questions/SaveQuality/{id}/{newValue}", new { controller = "AnswerQuestion", action = "SaveQuality" });
            routes.MapRoute("Question_SaveRelevancePersonal", "Questions/SaveRelevancePersonal/{id}/{newValue}", new { controller = "AnswerQuestion", action = "SaveRelevancePersonal" });
            routes.MapRoute("Question_SaveRelevanceForAll", "Questions/SaveRelevanceForAll/{id}/{newValue}", new { controller = "AnswerQuestion", action = "SaveRelevanceForAll" });

            routes.MapRoute("QuestionSets", "QuestionSets", new { controller = "QuestionSets", action = "QuestionSets" });
            routes.MapRoute("QuestionSets_Create", "QuestionSet/Create", new { controller = "EditQuestionSet", action = "Create" });
            
            routes.MapRoute("QuestionSets_Edit", "QuestionSet/Edit/{id}", new { controller = "EditQuestionSet", action = "Edit" });
            routes.MapRoute("QuestionSet_ImageUpload", "QuestionSet/UploadImage/{id}", new { controller = "EditQuestionSet", action = "UploadImage", id = UrlParameter.Optional });

            routes.MapRoute("QuestionSet_Detail", "QuestionSet/{text}/{id}/{elementOnPage}", new { controller = "QuestionSet", action = "QuestionSet" });
            
            routes.MapRoute("Categories", "Categories", new { controller = "Categories", action = "Categories" });
            routes.MapRoute("Categories_Create", "Categories/Create", new { controller = "EditCategory", action = "Create" });
            routes.MapRoute("Categories_Edit", "Categories/Edit/{id}", new { controller = "EditCategory", action = "Edit" });
            routes.MapRoute("Categories_Delete", "Categories/Delete/{id}", new { controller = "Categories", action = "Delete" });
            routes.MapRoute("Categories_AddSubCategoryRow", "Categories/AddSubCategoryRow", new { controller = "EditCategory", action = "AddSubCategoryRow" });
            routes.MapRoute("Categories_EditSubCategoryItems", "Categories/EditSubCategoryItems/{id}", new { controller = "EditSubCategoryItems", action = "Edit" });
            routes.MapRoute("Categories_AddSubCategoryItemRow", "Categories/EditSubCategoryItems/{id}/Add", new { controller = "EditSubCategoryItems", action = "AddSubCategoryItemRow" });

            routes.MapRoute("Knowledge", "Knowledge/{action}", new { controller = "Knowledge", action = "Knowledge" });
            routes.MapRoute("Maintenance", "Maintenance/{action}", new { controller = "Maintenance", action = "Maintenance" });
            routes.MapRoute("News", "News/{action}", new { controller = "News", action = "News" });
            routes.MapRoute("Various", "{action}", new { controller = "VariousPublic" });

            routes.MapRoute("ApiExport", "Api/Export/{action}", new { controller = "Export", action = "Export" });
            routes.MapRoute("ApiCategory", "Api/Category/{action}", new { controller = "CategoryApi" });
            routes.MapRoute("ApiUser", "Api/User/{action}", new { controller = "UserApi" });

            routes.MapRoute("ImageUpload", "ImageUpload/{action}", new { controller = "ImageUpload" });

            routes.MapRoute("User", "User/{name}/{id}/{action}", new { controller = "UserProfile", action = "Profile" });

            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Welcome", action = "Welcome", id = UrlParameter.Optional });
        }
    }
}