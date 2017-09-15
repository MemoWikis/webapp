<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TestSessionResultModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<h2 style="margin-bottom: 15px; margin-top: 0px;">
    <span class="">Dein Ergebnis</span>
</h2>
<p class="ResultDescription" style="margin-top: 30px;">
    Du hast dein Wissen 
    <% if (Model.TestSession.IsSetSession) { %>
        zum Lernset 
        <a href="<%= Links.SetDetail(Url, Model.TestedSet) %>" <%= Model.IsInWidget ? "target='_blank'" : "" %> style="display: inline-block; margin: 0 2px;">
            <span class="label label-set fontSizeNormal show-tooltip" <%= Model.IsInWidget ? "data-original-title='Zum Lernset auf memucho.de'" : ""%>><%: Model.TestedSet.Name %></span>
        </a>
    <% } else if (Model.TestSession.IsSetsSession) { %>
        zu den Lernsets
        <% foreach (var set in Model.TestedSets) { %>
            <a href="<% = Links.SetDetail(set)%>" <%= Model.IsInWidget ? "target='_blank'" : "" %> style="display: inline-block; margin: 0 2px;">
                <span class="label label-set fontSizeNormal"><%: set.Name %></span>
            </a>
        <% } %>
    <% } else if (Model.TestSession.IsCategorySession) { %>
        zum Thema
        <a href="<%= Links.CategoryDetail(Model.TestedCategory) %>" <%= Model.IsInWidget ? "target='_blank'" : "" %> style="display: inline-block;  margin: 0 2px;">
            <span class="label label-category"><%: Model.TestedCategory.Name %></span>
        </a>
    <% } %>
    getestet und dabei <%= Model.NumberQuestions %> von insgesamt <%=Model.TotalPossibleQuestions %> Frage<%= StringUtils.PluralSuffix(Model.TotalPossibleQuestions, "n") %> beantwortet.
    <% if (!Model.IsInWidget){ %>
        (<a href="#detailedAnswerAnalysis">Zur&nbsp;Auswertung</a>)
    <% } %>
</p>