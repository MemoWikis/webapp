<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<TopicNavigationModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<h1><%: Model.Title %></h1>
<p><%: Model.Text %></p>

<div id="topicNavigation" class="row">
    <% foreach (var category in Model.CategoryList)
        { %>
            <div class="col-xs-6">
                <div class="row">
                    <div class="col-xs-3">
                        <%= Model.GetCategoryImage(category).RenderHtmlImageBasis(100, false, ImageType.Category) %>
                    </div>
                    <div class="col-xs-9">
                        <a href="<%= Links.GetUrl(category) %>"><%: category.Name %></a>
                        <div class="set-question-count"><%: Model.GetTotalSetCount(category) %> Lernset <%: Model.GetTotalQuestionCount(category) %> Fragen</div>
                        <%-- HIER PROGRESS BAR REIN --%>
                    </div>
                </div>
            </div>
    <% } %>
</div>