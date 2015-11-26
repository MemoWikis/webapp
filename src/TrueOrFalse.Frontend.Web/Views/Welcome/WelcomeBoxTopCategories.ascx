<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<WelcomeBoxTopCategoriesModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<p style="padding-left: 0px; margin-top: 12px; margin-left: 0px; margin-bottom: 0px;">
    <% foreach (var category in Model.Categories) {%>
            <p style="margin-bottom: 0px; padding-left: 0px; padding-bottom: 5px; line-height: 12px;">
                <a href="<%= Links.CategoryDetail(category) %>"><span class="label label-category"><%: category.Name %></span></a> 
                <% if (category.CountQuestions == 0) { %> 
                    (0 Fragen)
                <% } else if (category.CountQuestions == 1) { %> 
                    <a href="<%: Links.QuestionWithCategoryFilter(Url, category) %>">(1 Frage)</a>
                <% } else {%>
                    <a href="<%: Links.QuestionWithCategoryFilter(Url, category) %>">(<%: category.CountQuestions %> Fragen)</a>
                <%} %>
            </p>
    <%} %>
</p>

