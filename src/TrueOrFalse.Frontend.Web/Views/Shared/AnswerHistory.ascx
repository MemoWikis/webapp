<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TrueOrFalse.AnswerHistoryModel>" %>

<span class="show-tooltip" title="Insgesamt <%=Model.TimesAnsweredTotal%>x beantwortet."><%=Model.TimesAnsweredTotal%>x </span>
<span class="sparklineTotals" data-answersTrue="<%= Model.TimesAnsweredCorrect %>" data-answersFalse="<%= Model.TimesAnsweredWrongTotal %>"></span>
<span class="show-tooltip" title="Von dir <%=Model.TimesAnsweredUser%>x beantwortet.">  ich: <%= Model.TimesAnsweredUser%>x </span>
<span class="sparklineTotalsUser" data-answersTrue="<%= Model.TimesAnsweredUserTrue  %>" data-answersFalse="<%= Model.TimesAnsweredUserWrong %>"></span>