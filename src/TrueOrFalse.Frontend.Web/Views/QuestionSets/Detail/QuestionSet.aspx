<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<QuestionSetModel>" %>

<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <%--<%= Scripts.Render("~/Views/QuestionSets/QuestionSets.js") %>--%>
    <%= Styles.Render("~/Views/QuestionSets/QuestionSet.css") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="row" style="min-height: 400px;">
        
        <h2><%= Model.Name %></h2>
        
        <h3>Fragen</h3>
        <% var index = 0; foreach(var question in Model.Questions){ index++; %>
            <div>
                <%= index %> <a href="<%= Links.AnswerQuestion(Url, question, 0) %>"><%=question.Text %></a>
            </div>
        <% } %>
    </div>

</asp:Content>