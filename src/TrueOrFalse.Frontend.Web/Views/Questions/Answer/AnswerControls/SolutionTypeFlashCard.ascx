<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionFlashCard>" %>
<%@ Import Namespace="TrueOrFalse.Web" %>


<div id="flashCardContent">
    <div class="front" id="flashCard-front">
        <div id="flashCard-frontContnent">Überlege dir die richtige Lösung und decke dann auf!</div>
    </div>
    <div class="back" id="flashCard-back">
        <div id="flashCard-backContent"><%= MarkdownInit.Run().Transform(Model.FlashCardContent) %></div>
    </div>
</div>
<script>
    $("#flashCardContent").flip();
</script>