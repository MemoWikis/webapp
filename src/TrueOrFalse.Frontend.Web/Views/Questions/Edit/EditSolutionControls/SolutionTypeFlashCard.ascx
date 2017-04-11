<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionFlashCard>" %>
    
<%--    <%= Html.LabelFor(m => m.Text, new { @class = "RequiredField columnLabel control-label" })%>
    <div class="columnControlsFull">
        <%= Html.TextBoxFor(m => m.Text, new { @class="form-control", @id = "Answer", @style = "float: left;", placeholder = "Antwort eingeben." })%>
    </div>--%>

<div class="form-group markdown">
    <div class="columnControlsFull">
        <div class="wmd-panel">
            <div id="wmd-button-bar-FlashCard"></div>   
            <%= Html.TextAreaFor(m => m.FlashCardContent, new 
                { @class= "form-control wmd-input", id="wmd-input-FlashCard", placeholder = "Inhalt der Karteikarte", rows = 4 })%>
        </div>
        <div id="wmd-preview-FlashCard" class="wmd-panel wmd-preview"></div>
    </div>
</div>
<br>

<script src="/Views/Questions/Edit/EditSolutionControls/SolutionTypeFlashCard.js" type="text/javascript"></script>