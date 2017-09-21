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

    <div class="row">
            
        <div class="col-xs-12">
            
            <div id="MainWrapper">
            
                <% if (string.IsNullOrEmpty(Model.CustomPageHtml)) {

                        Html.RenderPartial("~/Views/Categories/Detail/Partials/MainInfo.ascx", Model);%>
                
                        <%if (Model.CategoriesChildren.Count > 0)
                                Html.RenderPartial("~/Views/Categories/Detail/Partials/TopicNavigation.ascx",
                                                new TopicNavigationModel(Model.Category, "Unterthemen"));%>
                
<%--                        <% if(Model.AggregatedSets.Any())
                                for (var i = 0; i < 2; i++)
                                {
                                    if(Model.AggregatedSets[i] != null)
                                        Html.RenderPartial("~/Views/Categories/Detail/Partials/TestSetWidget.ascx",
                                            new TestSetWidgetModel(Model.AggregatedSets[i].Id));
                                } %>--%>

                        <% if (Model.FeaturedSets.Count > 0){

                            Html.RenderPartial("~/Views/Categories/Detail/Partials/SingleSetCollection.ascx",
                                new SingleSetCollectionModel(Model.FeaturedSets));

                            Html.RenderPartial("~/Views/Categories/Detail/Partials/ContentLists.ascx", Model);

                            Html.RenderPartial("~/Views/Categories/Detail/Partials/RelatedContentLists.ascx", Model);


                        } else {//no featured sets

                            Html.RenderPartial("~/Views/Categories/Detail/Partials/ContentLists.ascx", Model);

                            Html.RenderPartial("~/Views/Categories/Detail/Partials/RelatedContentLists.ascx", Model);
                        }

                } else { 
                    
                    if(!Model.CustomPageHtml.Contains("MainItemInfo")) { %>                  
            
                        <% Html.RenderPartial("~/Views/Categories/Detail/Partials/CategoryHeader.ascx", Model);%>
                        <%--<% Html.RenderPartial("~/Views/Categories/Detail/Partials/MainInfo.ascx", Model); %>--%>
                    
                    <% } %>
            
                    <div class="MarkdownContent">
                        <%= Model.CustomPageHtml %>
                    </div>

                <% } %>
            </div>
        </div>
    </div>
</asp:Content>