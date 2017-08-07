<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<TopicNavigationModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<h1><%: Model.Title %></h1>
<p><%: Model.Text %></p>

<div id="topicNavigation" class="row">
    <% foreach (var category in Model.CategoryList)
        { %>
            <div class="col-xs-6 topic">
                <div class="row">
                    <div class="col-xs-3">
                        <div class="ImageContainer">
                            <%= Model.GetCategoryImage(category).RenderHtmlImageBasis(128, true, ImageType.Category, linkToItem: Links.CategoryDetail(category.Name, category.Id)) %>
                        </div>
                    </div>
                    <div class="col-xs-9">
                        <a class="topic-name" href="<%= Links.GetUrl(category) %>">
                            <div class="topic-name">
                                <%: category.Name %>
                            </div>
                        </a>
                        <div class="set-question-count"><%: Model.GetTotalSetCount(category) %> Lernset <%: Model.GetTotalQuestionCount(category) %> Fragen</div>
                        <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(category)); %>
                    </div>
                </div>
            </div>
    <% } %>
</div>