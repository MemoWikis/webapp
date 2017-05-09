<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<CategoryModel>"%>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <% Title = Model.MetaTitle; %>
    <link rel="canonical" href="<%= Settings.CanonicalHost + Links.CategoryDetail(Model.Name, Model.Id) %>">
    <meta name="description" content="<%= Model.MetaDescription %>"/>
    

    <meta property="og:title" content="<%: Model.Name %>" />
    <meta property="og:url" content="<%= Settings.CanonicalHost + Links.CategoryDetail(Model.Name, Model.Id) %>" />
    <meta property="og:type" content="article" />
    <meta property="og:image" content="<%= Model.ImageFrontendData.GetImageUrl(350, false, imageTypeForDummy: ImageType.Category).Url %>" />
    
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
</asp:Content>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/Category") %>
    <%= Scripts.Render("~/bundles/js/Category") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <input type="hidden" id="hhdCategoryId" value="<%= Model.Category.Id %>"/>

    <div class="row" >
        <div class="col-xs-12 col-md-2 col-md-push-10">
            <div class="navLinks">
                <a href="<%= Url.Action(Links.CategoriesAction, Links.CategoriesController) %>" style="font-size: 12px;"><i class="fa fa-list"></i>&nbsp;zur Übersicht</a>
                <% if(Model.IsOwnerOrAdmin){ %>
                    <a href="<%= Links.CategoryEdit(Url, Model.Name, Model.Id) %>" style="font-size: 12px;"><i class="fa fa-pencil"></i>&nbsp;bearbeiten</a> 
                <% } %>
                <a href="<%= Links.CreateQuestion(categoryId: Model.Id) %>" style="font-size: 12px;"><i class="fa fa-plus-circle"></i>&nbsp;Frage hinzufügen</a>
                <% if(Model.IsInstallationAdmin) { %>
                    <a href="#" class="show-tooltip" data-placement="right" data-original-title="Nur von admin sichtbar">
                        <i class="fa fa-user-secret">&nbsp;</i><%= Model.GetViews() %> views
                    </a>    
                <% } %>
            </div>
        </div>
        <div class="col-xs-12 col-md-10 col-md-pull-2">
            
            <% if (string.IsNullOrEmpty(Model.CustomPageHtml)) {

                    Html.RenderPartial("~/Views/Categories/Detail/Partials/MainInfo.ascx", Model);%>


                    <% if (Model.FeaturedSets.Count > 0){

                        Html.RenderPartial("~/Views/Categories/Detail/Partials/SingleSetCollection.ascx",
                            new SingleSetCollectionModel(Model.FeaturedSets));

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
            
                    <% Html.RenderPartial("~/Views/Categories/Detail/Partials/MainInfo.ascx", Model);%>
                    
                <% } %>
            
                <div class="MarkdownContent">
                    <%= Model.CustomPageHtml %>
                </div>

            <% } %>

        </div>
    </div>
</asp:Content>