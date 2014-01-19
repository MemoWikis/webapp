<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TrueOrFalse.CorrectnessProbabilityModel>" %>

<span class="show-tooltip" data-html="true"
    title="
        <div style='text-align:left;'>
            <b><%: Model.CP %>%</b> Wahrscheinlichkeit, dass Du die Frage korrekt beantwortest<br /><br />
                        
            Alle Nutzer: <%: Model.CP + Model.CPDerivation %>%<br />
            Deine Abweichung: <%: Model.CPDerivationSign %> <%: -Model.CPDerivation %>%
        </div>">
    <i class="fa fa-tachometer" style="color:green;"></i> 
        <%: Model.CP %>% 
        <%: Model.CPDerivationSign %>
        <%: Math.Abs(Model.CPDerivation) %>
</span>