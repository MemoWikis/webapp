<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<WelcomeBoxTopQuestionsModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<ul>
    <% foreach (var question in Model.Questions) {%>
            <li>
                <a href="<%= Links.AnswerQuestion(question) %>"><%: question.Text %></a> 
            </li>
    <%} %>
</ul>

