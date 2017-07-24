<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<TopicNavigationModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<%if(Model.HasUsedOrderListWithLoadList) { %>

    <h2 style="color: red; font-weight: bold">FEHLER (TopicNavigationTemplate): <br/> Bitte "Load:" und "Order:" nicht gleichzeitig mit Id-Listen verwenden!</h2>

<% } else { %>

    <h1><%: Model.Title %></h1>
    <p><%: Model.Text %></p>

    <div id="topicNavigation" class="row">
        <% foreach (var category in Model.CategoryList)
            { %>
                <div class="col-xs-6 topic">
                    <div class="row">
                        <div class="col-xs-3">
                            <a href="<%= Links.GetUrl(category) %>">
                                <%= Model.GetCategoryImage(category).RenderHtmlImageBasis(100, true, ImageType.Category) %>
                            </a>
                        </div>
                        <div class="col-xs-9">
                            <a href="<%= Links.GetUrl(category) %>"><%: category.Name %></a>
                            <div class="set-question-count"><%: Model.GetTotalSetCount(category) %> Lernset <%: Model.GetTotalQuestionCount(category) %> Fragen</div>
                            <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(category)); %>
                        </div>
                    </div>
                </div>
        <% } %>
    </div>

<% } %>