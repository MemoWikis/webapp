<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<TopicNavigationModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<h1><%: Model.Title %></h1>
<p><%: Model.Text %></p>

<div id="topicNavigation" class="row">
    <% foreach (var topic in Model.TopicList)
        { %>
            <div class="col-xs-6">
                <div class="row">
                    <div class="col-xs-3">
                        <%= Model.GetCategoryImage(topic).RenderHtmlImageBasis(100, false, ImageType.Category) %>
                    </div>
                    <div class="col-xs-9">
                        <a href="<%= Links.GetUrl(topic) %>"><%: topic.Name %></a>
                        <div class="set-question-count"><%: Model.GetTotalSetCount(topic) %> Lernset <%: Model.GetTotalQuestionCount(topic) %> Fragen</div>
                        <%-- HIER PROGRESS BAR REIN --%>
                    </div>
                </div>
            </div>
    <% } %>
</div>