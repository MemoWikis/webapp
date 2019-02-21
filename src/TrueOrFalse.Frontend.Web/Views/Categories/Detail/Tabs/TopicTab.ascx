<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
    
<% if (Model.Category.IsHistoric) { %>
    <div class="alert alert-info" role="alert">
        <b>Revision vom <%= Model.CategoryChange.DateCreated %></b>
        <br/>
        <% if (Model.NextRevExists) { %>
            Diese Seite zeigt einen <b>früheren Stand</b> des Themas.
        <% } else {%>
            Dies ist die <b>aktuelle Revision</b> des Themas.
        <% } %>
        
        <br />
        <br />

        In dieser Revisionsansicht gibt es nur <b>eingeschränkte Möglichkeiten, mit dem Thema 
        zu interagieren</b>, bspw. eine Lernsitzung zu starten. Bitte gehe dazu am besten zur 
        Liveansicht des Themas:
        <a href="<%= Links.CategoryDetail(Model.Category.Name, Model.Category.Id) %>">
            <%= Model.Name %>
        </a>

        <div class="dropdown pull-right" style="margin-top: 1em">
            <a class="btn btn-primary" href="<%= Links.CategoryHistoryDetail(Model.Id, Model.CategoryChange.Id) %>">
                <i class="fa fa-code-fork"></i> &nbsp; Änderungen anzeigen
            </a>
            <a class="btn btn-default" href="<%= Links.CategoryHistory(Model.Id) %>">
                <i class="fa fa-list-ul"></i> &nbsp; Bearbeitungshistorie
            </a>
            <% var buttonSetId = Guid.NewGuid(); %>
            <a href="#" id="<%= buttonSetId %>" class="dropdown-toggle btn btn-link btn-sm ButtonEllipsis" 
               type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                <i class="fa fa-ellipsis-v" style="font-size: 18px; margin-top: 2px;"></i>
            </a>
            <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="<%= buttonSetId %>">
                <li>
                    <% if (new SessionUser().IsLoggedIn) {
                           if (Model.NextRevExists) { %>
                            <a id="restoreButton" data-allowed="logged-in" onclick="$('#alertConfirmRestore').show();">
                                <i class="fa fa-undo"></i> &nbsp; Wiederherstellen
                            </a>
                        <% } else { %>
                            <a id="editButton" data-allowed="logged-in" href="<%= Links.CategoryEdit(Model.Category) %>">
                                <i class="fa fa-edit"></i> &nbsp; Thema bearbeiten
                            </a>
                        <% } %>
                    <% } %>
                </li>
                <li>
                    <a href="<%= Links.CategoryChangesOverview(1) %>">
                        <i class="fa fa-list"></i> &nbsp; Bearbeitungshistorie aller Themen
                    </a>
                </li>
            </ul>
        </div>
        
        <br/>
        <br/>
        &nbsp;

    </div>
        
    <% if (new SessionUser().IsLoggedIn && Model.NextRevExists) { %>
        <div id="alertConfirmRestore" class="row" style="display: none">
            <br/>
            <div class="alert alert-warning" role="alert">
                <div class="col-12">
                    Der aktuelle Stand wird durch diese Version ersetzt. Wollen Sie das wirklich?
                </div>
                <br/>
                <div class="col-12">
                    <nav>
                        <a class="btn btn-default navbar-btn" href="<%= Links.CategoryRestore(Model.Category.Id, Model.CategoryChange.Id) %>">
                            <i class="fa fa-undo"></i> Ja, Wiederherstellen
                        </a>
                        <a class="btn btn-primary navbar-btn" onclick="$('#alertConfirmRestore').hide();">
                            <i class="fa fa-remove"></i> Nein, Abbrechen
                        </a>
                    </nav>
                </div>
            </div>
        </div>
    <% } %>

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

<div class="MarkdownContent">
    <ul id="MarkdownContentComponent"class="module" v-sortable="options" style="list-style-type: none;">
        <% if (string.IsNullOrEmpty(Model.CustomPageHtml)) {
        
               if (Model.CategoriesChildren.Any(c => c.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Standard))
                   Html.RenderPartial("~/Views/Categories/Detail/Partials/TopicNavigation/TopicNavigation.ascx",new TopicNavigationModel(Model.Category, "Unterthemen"));
                        
        
               if (Model.AggregatedSetCount > 0 && Model.AggregatedSetCount <= 5){
                   foreach (var set in Model.AggregatedSets)
                   {
                       Html.RenderPartial("~/Views/Categories/Detail/Partials/SingleSetFullWidth/SingleSetFullWidth.ascx", new SingleSetFullWidthModel(set.Id));
                   }
               }
               else if (Model.AggregatedSetCount == 0 && Model.AggregatedQuestionCount > 0)
               {
                   Html.RenderPartial("~/Views/Categories/Detail/Partials/SingleQuestionsQuiz/SingleQuestionsQuiz.ascx", new SingleQuestionsQuizModel(Model.Category,5));
               }
        
               if (Model.CategoriesChildren.Any(c => c.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Education))
                   Html.RenderPartial("~/Views/Categories/Detail/Partials/EducationOfferList/EducationOfferList.ascx", new EducationOfferListModel(Model.Category));
        
               if (Model.CategoriesChildren.Any(c => c.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Media))
                   Html.RenderPartial("~/Views/Categories/Detail/Partials/MediaList/MediaList.ascx", new MediaListModel(Model.Category));
        
               Html.RenderPartial("~/Views/Categories/Detail/Partials/ContentLists/ContentLists.ascx", Model);
        
               Html.RenderPartial("~/Views/Categories/Detail/Partials/RelatedContentLists.ascx", Model);
        
               Html.RenderPartial("~/Views/Categories/Detail/Partials/CategoryNetwork/CategoryNetwork.ascx", Model);
        
           } else { %>
                            
            <%= Model.CustomPageHtml %>
        
        <% } %>
    </ul>
</div>
