<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<CategoryModel>"%>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <% Title = "Kategorie: " + Model.Name; %>
    <link rel="canonical" href="<%= Settings.CanonicalHost + Links.CategoryDetail(Model.Name, Model.Id) %>">
    <meta name="description" content="<%= Model.Name.Replace("\"", "'").Replace("„", "'").Replace("“", "'").Truncate(25, true) %> (<%=Model.CountQuestions %> Fragen) <%= String.IsNullOrEmpty(Model.Description) ? "" : ": "+Model.Description.Replace("\"", "'").Replace("„", "'").Replace("“", "'").Truncate(89, true) %> - Lerne mit memucho!"/>
    
    <meta property="og:url" content="<%= Settings.CanonicalHost + Links.CategoryDetail(Model.Name, Model.Id) %>" />
    <meta property="og:type" content="article" />
    <meta property="og:image" content="<%= Model.ImageFrontendData.GetImageUrl(350, false, imageTypeForDummy: ImageType.Category).Url %>" />
</asp:Content>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Categories/Detail/Category.css" rel="stylesheet" /> 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row" >
        <div class="col-xs-12 col-md-2 col-md-push-10">
            <div class="navLinks">
                <a href="<%= Url.Action(Links.CategoriesAction, Links.CategoriesController) %>" style="font-size: 12px;"><i class="fa fa-list"></i>&nbsp;zur Übersicht</a>
                <% if(Model.IsOwnerOrAdmin){ %>
                    <a href="<%= Links.CategoryEdit(Url, Model.Name, Model.Id) %>" style="font-size: 12px;"><i class="fa fa-pencil"></i>&nbsp;bearbeiten</a> 
                <% } %>
                <a href="<%= Links.CreateQuestion(Url, Model.Id) %>" style="font-size: 12px;"><i class="fa fa-plus-circle"></i>&nbsp;Frage hinzufügen</a>
            </div>
        </div>
        <div class="col-xs-12 col-md-10 col-md-pull-2">
            
            <% if (string.IsNullOrEmpty(Model.CustomPageHtml)) {

                    Html.RenderPartial("~/Views/Categories/Detail/Partials/MainInfo.ascx", Model);

                    <% if (Model.FeaturedSets.Count > 0){

                        Html.RenderPartial("~/Views/Categories/Detail/Partials/SingleSetCollection.ascx",
                            new SingleSetCollectionModel(Model.FeaturedSets, "Ausgewählte Fragesätze"));

                        Html.RenderPartial("~/Views/Categories/Detail/Partials/CategoryNetwork.ascx", Model);

                        Html.RenderPartial("~/Views/Categories/Detail/Partials/ContentLists.ascx", Model);

                        Html.RenderPartial("~/Views/Categories/Detail/Partials/RelatedContentLists.ascx", Model);


                    } else {//no featured sets

                        Html.RenderPartial("~/Views/Categories/Detail/Partials/ContentLists.ascx", Model);

                        Html.RenderPartial("~/Views/Categories/Detail/Partials/CategoryNetwork.ascx", Model);

                        Html.RenderPartial("~/Views/Categories/Detail/Partials/RelatedContentLists.ascx", Model);
                    }

            } else { 
                    
                if(!Model.CustomPageHtml.Contains("MainItemInfo")) { %>                  
            
                    <% Html.RenderPartial("~/Views/Categories/Detail/Partials/MainInfo.ascx", Model); %>
                    
                <% } %>
            
                    <%= Model.CustomPageHtml %>

            <% } %>

        </div>
    </div>
</asp:Content>