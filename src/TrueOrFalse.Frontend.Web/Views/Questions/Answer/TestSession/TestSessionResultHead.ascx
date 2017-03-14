<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TestSessionResultModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<h2 style="margin-bottom: 15px; margin-top: 0px;">
    <span class="">Dein Ergebnis</span>
</h2>
<p>
    Du hast dein Wissen 
    <% if (Model.TestSession.IsSetSession) { %>
        zu dem Fragesatz 
        <a href="<%= Links.SetDetail(Url, Model.TestedSet) %>" style="display: inline-block;">
            <span class="label label-set"><%: Model.TestedSet.Name %></span>
        </a>
        <%--mit insgesamt <%=Model.TestedSet.Questions().Count %> Fragen--%>
    <% } else if (Model.TestSession.IsSetsSession) { %>
        zu den Fragesätzen
        <% foreach (var set in Model.TestedSets) { %>
            <a href="<%= Links.SetDetail(set) %>" style="display: inline-block;">
                <span class="label label-set"><%: set.Name %></span>
            </a>
        <% } %>
    <% } else if (Model.TestSession.IsCategorySession) { %>
        zum Thema
        <a href="<%= Links.CategoryDetail(Model.TestedCategory) %>" style="display: inline-block;">
            <span class="label label-category"><%: Model.TestedCategory.Name %></span>
        </a>
        <%--mit insgesamt <%=Model.TestedCategory.CountQuestions %> Fragen--%>
    <% } %>
    getestet und dabei <%= Model.NumberQuestions %> Fragen beantwortet. 
    (<a href="#detailedAnswerAnalysis">Zur Auswertung</a>)
</p>