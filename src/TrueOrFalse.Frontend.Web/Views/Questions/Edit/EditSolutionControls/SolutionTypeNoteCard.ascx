<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionExact>" %>
    
<%--    <%= Html.LabelFor(m => m.Text, new { @class = "RequiredField columnLabel control-label" })%>
    <div class="columnControlsFull">
        <%= Html.TextBoxFor(m => m.Text, new { @class="form-control", @id = "Answer", @style = "float: left;", placeholder = "Antwort eingeben." })%>
    </div>--%>

<div class="form-group markdown">
    <label class="columnLabel control-label" for="Description">
        <span class="show-tooltip"  title = "Erscheinen nach dem Beantworten der Frage zusammen mit der richtigen Lösung und sollen beim Einordnen und Merken der abgefragten Fakten helfen. Oft wird eine Frage erst durch informative Zusatzangaben so richtig gut." data-placement = "<%= CssJs.TooltipPlacementLabel %>">Ergänzungen</span>
    </label>
    <div class="columnControlsFull">
        <div class="wmd-panel">
            <div id="wmd-button-bar-2"></div>   
            <%= Html.TextAreaFor(m => m.Text, new 
                { @class= "form-control wmd-input", id="wmd-input-2", placeholder = "Erklärungen, Zusatzinfos, Merkhilfen, Abbildungen, weiterführende Literatur und Links etc.", rows = 4 })%>
        </div>
        <div id="wmd-preview-2" class="wmd-panel wmd-preview"></div>
    </div>
</div>
