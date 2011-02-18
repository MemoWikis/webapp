<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="menu">
    <ul >              
        <li><%= Html.ActionLink("Willkommen", "Welcome", Links.WelcomeController)%></li>
        <li><%= Html.ActionLink("Frage erstellen", Links.QuestionCreate, Links.QuestionController)%></li>
        
    </ul>
</div>

