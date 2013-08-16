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

            routes.MapRoute("Help", "Hilfe/{action}", new { controller = "Help", action = "Willkommen" });

            routes.MapRoute("UsersSearch", "Nutzer/Suche/{searchTerm}", new { controller = "Users", action = "Search", searchTerm = UrlParameter.Optional });
            routes.MapRoute("UserSettings", "Nutzer/Einstellungen", new { controller = "UserSettings", action = "UserSettings" });
            routes.MapRoute("UserLoginAs", "Nutzer/LoginAs/{userId}", new { controller = "Users", action= "LoginAs" });
            routes.MapRoute("User", "Nutzer/{name}/{id}/{action}", new { controller = "User", action = "User" });
            routes.MapRoute("Users", "Nutzer", new { controller = "Users", action = "Users", page = UrlParameter.Optional });

            routes.MapRoute("Questions", "Fragen", new { controller = "Questions", action = "Questions" });
            routes.MapRoute("Questions_SearchTerm", "Fragen/Suche/{searchTerm}", new { controller = "Questions", action = "QuestionSearch", searchTerm = UrlParameter.Optional });

            routes.MapRoute("Question_Create", "Fragen/Erstelle/", new { controller = "EditQuestion", action = "Create" });
            routes.MapRoute("Question_Edit", "Fragen/Bearbeite/{id}", new { controller = "EditQuestion", action = "Edit" });
            routes.MapRoute("Question_Delete", "Fragen/Loesche/{id}", new { controller = "Questions", action = "Delete" });

            routes.MapRoute("Question_Answer", "Fragen/{text}/{id}/{elementOnPage}", new { controller = "AnswerQuestion", action = "Answer" });

            /* API */ routes.MapRoute("Questions_DeleteDetails", "Questions/DeleteDetails/{questionId}", new { controller = "Questions", action = "DeleteDetails" });
            /* API */ routes.MapRoute("Questions_Delete", "Questions/Delete/{questionId}", new { controller = "Questions", action = "Delete" });
            /* API */ routes.MapRoute("Question_SendAnswer", "Questions/SendAnswer/{id}", new { controller = "AnswerQuestion", action = "SendAnswer" });
            /* API */ routes.MapRoute("Question_GetAnswer", "Questions/GetAnswer/{id}", new { controller = "AnswerQuestion", action = "GetAnswer" });
            /* API */ routes.MapRoute("Question_SaveQuality", "Questions/SaveQuality/{id}/{newValue}", new { controller = "AnswerQuestion", action = "SaveQuality" });
            /* API */ routes.MapRoute("Question_SaveRelevancePersonal", "Questions/SaveRelevancePersonal/{id}/{newValue}", new { controller = "AnswerQuestion", action = "SaveRelevancePersonal" });
            /* API */ routes.MapRoute("Question_SaveRelevanceForAll", "Questions/SaveRelevanceForAll/{id}/{newValue}", new { controller = "AnswerQuestion", action = "SaveRelevanceForAll" });

            routes.MapRoute("Sets_SearchTerm", "FrageSaetze/Suche/{searchTerm}", new { controller = "Sets", action = "Search", searchTerm = UrlParameter.Optional });
            routes.MapRoute("Sets", "FrageSaetze/{action}", new { controller = "Sets", action = "Sets" });

            routes.MapRoute("Set_Detail", "FrageSaetze/{text}/{id}/{elementOnPage}", new { controller = "Set", action = "QuestionSet" });
            routes.MapRoute("Sets_Edit", "FrageSaetze/Bearbeite/{id}", new { controller = "EditSet", action = "Edit" });
            /* API */ routes.MapRoute("Set_ChangeIndicies", "Set/UpdateQuestionsOrder", new { controller = "EditSet", action = "UpdateQuestionsOrder" });
            /* API */ routes.MapRoute("Set_ImageUpload", "Set/UploadImage/{id}", new { controller = "EditSet", action = "UploadImage", id = UrlParameter.Optional });

            routes.MapRoute("Categories_SearchTerm", "Kategorien/Suche/{searchTerm}", new { controller = "Categories", action = "Search", searchTerm = UrlParameter.Optional });
            routes.MapRoute("Categories", "Kategorien", new { controller = "Categories", action = "Categories" });
            routes.MapRoute("Categories_Create", "Kategorien/Erstelle", new { controller = "EditCategory", action = "Create" });
            routes.MapRoute("Categories_Edit", "Kategorien/Bearbeite/{id}", new { controller = "EditCategory", action = "Edit" });
            routes.MapRoute("Categories_Delete", "Kategorien/Loesche/{id}", new { controller = "Categories", action = "Delete" });
            routes.MapRoute("Category_Detail", "Kategorien/{text}/{id}/{elementOnPage}", new { controller = "Category", action = "Category" });
            /* API */ routes.MapRoute("Categories_AddSubCategoryRow", "Categories/AddSubCategoryRow", new { controller = "EditCategory", action = "AddSubCategoryRow" });
            /* API */ routes.MapRoute("Categories_EditSubCategoryItems", "Categories/EditSubCategoryItems/{id}", new { controller = "EditSubCategoryItems", action = "Edit" });
            /* API */ routes.MapRoute("Categories_AddSubCategoryItemRow", "Categories/EditSubCategoryItems/{id}/Add", new { controller = "EditSubCategoryItems", action = "AddSubCategoryItemRow" });

            routes.MapRoute("Knowledge", "Wissen/{action}", new { controller = "Knowledge", action = "Knowledge" });

            routes.MapRoute("Maintenance", "Maintenance/{action}", new { controller = "Maintenance", action = "Maintenance" });
            routes.MapRoute("News", "News/{action}", new { controller = "News", action = "News" });
            routes.MapRoute("Various", "{action}", new { controller = "VariousPublic" });

            routes.MapRoute("ApiExport", "Api/Export/{action}", new { controller = "Export", action = "Export" });
            routes.MapRoute("ApiCategory", "Api/Category/{action}", new { controller = "CategoryApi" });
            routes.MapRoute("ApiUser", "Api/User/{action}", new { controller = "UserApi" });

            routes.MapRoute("ImageUpload", "ImageUpload/{action}", new { controller = "ImageUpload" });

            
            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Welcome", action = "Welcome", id = UrlParameter.Optional });
        }
    }
}