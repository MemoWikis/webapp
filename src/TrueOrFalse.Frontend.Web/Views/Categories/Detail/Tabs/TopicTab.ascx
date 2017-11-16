<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="DescriptionSection">
    <%if (Model.ImageFrontendData.ImageMetaDataExists) { %>
        <div class="ImageContainer">
            <%= Model.ImageFrontendData.RenderHtmlImageBasis(350, false, ImageType.Category, "ImageContainer") %>
        </div>
        <% } %>   
    
    <div class="TextColumn">
        <% if (Model.Type != "Standard") { %>
            <div>                    
                <% Html.RenderPartial("Reference", Model.Category); %>
            </div>
        <% } %>
                
        <div class="Description"><span><%= Model.Description %></span></div>
                
        <% if (!String.IsNullOrEmpty(Model.Url)){ %>
            <div class="WikiLink">
                <a href="<%= Model.Url %>" target="_blank" class="" title="" data-placement="left" data-html="true">
                    <i class='fa fa-external-link'>&nbsp;&nbsp;</i><%= string.IsNullOrEmpty(Model.Category.UrlLinkText) ? Model.Url : Model.Category.UrlLinkText %>
                </a>
            </div>
        <% } %>
        <% if (!String.IsNullOrEmpty(Model.WikipediaURL)){ %>
            <div class="WikiLink">
                <a href="<%= Model.WikipediaURL %>" target="_blank" class="show-tooltip" title="<%= Links.IsLinkToWikipedia(Model.WikipediaURL) ? "Link&nbsp;auf&nbsp;Wikipedia" : "" %>" data-placement="left" data-html="true">
                    <% if(Links.IsLinkToWikipedia(Model.WikipediaURL)){ %>
                        <i class="fa fa-wikipedia-w">&nbsp;</i><% } %><%= Model.WikipediaURL %>
                </a>
            </div>
        <% } %>
    </div>
</div>

<% if (string.IsNullOrEmpty(Model.CustomPageHtml)) {

    if (Model.CategoriesChildren.Count > 0)
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

   } else { %>
                    
    <div class="MarkdownContent">
        <%= Model.CustomPageHtml %>
    </div>

<% } %>