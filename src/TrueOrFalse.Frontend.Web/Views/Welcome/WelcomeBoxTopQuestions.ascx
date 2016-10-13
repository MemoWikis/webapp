<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<WelcomeBoxTopQuestionsModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% foreach (var question in Model.Questions) {%>
    <div class="LabelItem LabelItem-Question">
        <a href="<%= Links.AnswerQuestion(question) %>"><%: question.Text %></a> 
    </div>
<%} %>

