 <%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<SegmentationCategoryCardModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

    <div class="col-xs-6 topic segmentCategoryCard">
        <div class="row">
            <div class="col-xs-3">
                <div class="ImageContainer">
                    <%= Model.GetCategoryImage(Model.Category).RenderHtmlImageBasis(128, true, ImageType.Category, linkToItem: Links.CategoryDetail(Model.Category.Name, Model.Category.Id)) %>
                </div>
            </div>
            <div class="col-xs-9">
                <a class="topic-name" href="<%= Links.CategoryDetail(Model.Category) %>">
                    <div class="topic-name">
                        <%= Model.Category.Type.GetCategoryTypeIconHtml() %><%: Model.Category.Name %>
                        <% if (Model.Category.Visibility == CategoryVisibility.Owner) { %>
                            <i class="fas fa-lock"></i>
                        <% } %>
                    </div>
                </a>

                <div class="set-question-count">
                    <span>
                        <pin-category-component :category-id="<%= Model.Category.Id %>"/>
                    </span>

                        <% if (Model.Category.CachedData.ChildrenIds.Count == 1)
                       { %>1 Unterthema <% } %>
                        <% if(Model.Category.CachedData.ChildrenIds.Count > 1)
                           { %><%= Model.Category.CachedData.ChildrenIds.Count  %> Unterthemen <% } 
                           else { %><% } %><%=Model.TotalQuestionCount %> Frage<% if(Model.TotalQuestionCount != 1){ %>n<% } %>
                </div>
                    <%if(Model.TotalQuestionCount > 0) {%>
                    <div class="KnowledgeBarWrapper">
                        <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(Model.Category)); %>
                        <div class="KnowledgeBarLegend">Dein Wissensstand</div>
                    </div>
                <% }%>

            </div>
        </div>
    </div>

           