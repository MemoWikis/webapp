<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionFlashCard>" %>
<%@ Import Namespace="TrueOrFalse.Web" %>


<div id="flashCardContent">
    <div class="front" id="flashCard-front">
        <div class="flashCard-Card">
            <small>Überlege dir die richtige Lösung und decke dann auf!</small>
        </div>
    </div>
    <div class="back" id="flashCard-back">
        <div class="flashCard-Card">        
            <%= MarkdownInit.Run().Transform(Model.FlashCardContent) %>
        </div>
    </div>
</div>
<script>
    $("#flashCardContent").flip();
</script>