<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
    
<% if (Model.Category.IsHistoric) { %>
    <div class="alert alert-info" role="alert">
        <b>Revision vom <%= Model.CategoryChange.DateCreated %></b>
        <br/>
        <%= (Model.NextRevExists) 
                ? "Diese Seite zeigt einen <b>früheren Stand</b> des Themas."
                : "Dies ist die <b>aktuelle Revision</b> des Themas."
        %>
        <nav>
            <a class="btn btn-primary navbar-btn" href="<%= Links.CategoryHistoryDetail(Model.Id, Model.CategoryChange.Id) %>">
                <i class="fa fa-code-fork"></i> &nbsp; Änderungen anzeigen
            </a>
            <a class="btn btn-default navbar-btn" href="<%= Links.CategoryHistory(Model.Id) %>">
                <i class="fa fa-list-ul"></i> &nbsp; Zur Bearbeitungshistorie
            </a>
            <a class="btn btn-default navbar-btn" href="<%= Links.HistoryOfEverything(1) %>">
                <i class="fa fa-list"></i> &nbsp; Zur Bearbeitungshistorie aller Themen
            </a>
        </nav>
    </div>
    <br/>
<% } %>

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

       if (Model.CategoriesChildren.Any(c => c.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Standard))
           Html.RenderPartial("~/Views/Categories/Detail/Partials/TopicNavigation.ascx",new TopicNavigationModel(Model.Category, "Unterthemen"));
                

       if (Model.AggregatedSetCount > 0 && Model.AggregatedSetCount <= 5){
           foreach (var set in Model.AggregatedSets)
           {
               Html.RenderPartial("~/Views/Categories/Detail/Partials/SingleSetFullWidth.ascx", new SingleSetFullWidthModel(set.Id));
           }
       }
       else if (Model.AggregatedSetCount == 0 && Model.AggregatedQuestionCount > 0)
       {
           Html.RenderPartial("~/Views/Categories/Detail/Partials/SingleQuestionsQuiz.ascx", new SingleQuestionsQuizModel(Model.Category,5));
       }

       if (Model.CategoriesChildren.Any(c => c.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Education))
           Html.RenderPartial("~/Views/Categories/Detail/Partials/EducationOfferList.ascx", new EducationOfferListModel(Model.Category));

       if (Model.CategoriesChildren.Any(c => c.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Media))
           Html.RenderPartial("~/Views/Categories/Detail/Partials/MediaList.ascx", new MediaListModel(Model.Category));

       Html.RenderPartial("~/Views/Categories/Detail/Partials/Spacer.ascx", new SpacerModel(1, true));

       Html.RenderPartial("~/Views/Categories/Detail/Partials/ContentLists.ascx", Model);

       Html.RenderPartial("~/Views/Categories/Detail/Partials/RelatedContentLists.ascx", Model);

       Html.RenderPartial("~/Views/Categories/Detail/Partials/CategoryNetwork.ascx", Model);

   } else { %>
                    
    <div class="MarkdownContent">
        <%= Model.CustomPageHtml %>
    </div>

<% } %>
