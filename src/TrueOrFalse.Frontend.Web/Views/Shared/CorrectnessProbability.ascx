<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TrueOrFalse.CorrectnessProbabilityModel>" %>


<span class="show-tooltip" data-html="true"
    title="
        <div style='text-align:left;'>

<% if (Model.UserHasAnswerHistory) { %>
            <b><%: Model.CPPersonal %>%</b> Wahrscheinlichkeit, dass du die Frage korrekt beantwortest<br /><br />
                        
            Alle Nutzer: <%: Model.CPAll %>%<br />
            Deine Abweichung: <%= Model.CPDerivationSign %> <%: Model.CPDerivation %>%
        </div>">
        <i class="fa fa-tachometer"></i> 
            <%: Model.CPPersonal %>% 
            <span class="greyed derivation"><%= Model.CPDerivationSign %><%: Model.CPDerivation %></span>
<% } else if (Model.QuestionHasAnswerHistory) { %>
            <b><%: Model.CPAll %>%</b> beträgt die durchschnittliche Wahrscheinlichkeit einer korrekten Antwort (Basis: Alle memucho-Nutzer).<br/><br/>
            Du hast diese Frage <b>noch nie</b> beantwortet.
        </div>">
        <i class="fa fa-tachometer greyed"></i> 
            <span class="greyed"><%: Model.CPAll %>%</span>
<% } else { %>
            Diese Frage wurde <b>noch nie</b> beantwortet.<br/>
            Wir wissen daher noch nicht die Wahrscheinlichkeit einer korrekten Antwort.
        </div>">
        <i class="fa fa-tachometer greyed"></i> 
        <span class="greyed">?</span>
<% }%>

</span>