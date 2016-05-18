<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TrueOrFalse.CorrectnessProbabilityModel>" %>

<span class="show-tooltip" data-html="true"
    title="
        <div style='text-align:left;'>
            <b><%: Model.CPPersonal %>%</b> Wahrscheinlichkeit, dass du die Frage korrekt beantwortest<br /><br />
                        
            Alle Nutzer: <%: Model.CPAll %>%<br />
            Deine Abweichung: <%: Model.CPDerivationSign %> <%: Model.CPDerivation %>%
        </div>">
    <i class="fa fa-tachometer" style="color:#69D069;"></i> 
        <%: Model.CPPersonal %>% 
        <span style="color:silver"><%: Model.CPDerivationSign %><%: Model.CPDerivation %></span>
</span>