<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TrueOrFalse.AnswerHistoryModel>" %>

<span class="show-tooltip" title="Insgesamt <%=Model.TimesAnsweredTotal%>x beantwortet."><%=Model.TimesAnsweredTotal%>x </span>
<span id="sparklineTrueOrFalseUser" data-answersTrue="<%= Model.TimesAnsweredUserTrue  %>" data-answersFalse="<%= Model.TimesAnsweredUserWrong %>"></span>
<span class="show-tooltip" title="Von Dir <%=Model.TimesAnsweredUser%>x beantwortet."> ich <%= Model.TimesAnsweredUser%>x </span>
<span id="sparklineTrueOrFalseTotals" data-answersTrue="<%= Model.TimesAnsweredCorrect %>" data-answersFalse="<%= Model.TimesAnsweredWrongTotal %>"></span>