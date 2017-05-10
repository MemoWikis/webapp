<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionFlashCard>" %>

<div class="form-group markdown">
    <div class="columnControlsFull">
        <div class="wmd-panel">
            <div id="wmd-button-bar-FlashCard"></div>   
            <%= Html.TextAreaFor(m => m.Text, new 
                { @class= "form-control wmd-input", id="wmd-input-FlashCard", Name = "FlashCardContent", placeholder = "Rückseite der Karteikarte", rows = 4 })%>
        </div>
        <div id="wmd-preview-FlashCard" class="wmd-panel wmd-preview"></div>
    </div>
</div>
<br>

<script src="/Views/Questions/Edit/EditSolutionControls/SolutionTypeFlashCard.js" type="text/javascript"></script>