<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionFlashCard>" %>
<%@ Import Namespace="TrueOrFalse.Web" %>


<div id="flashCardContent">
    <div class="front flashCardContentSite" id="flashCard-front">
        <div class="flipClickHint">
            <span class="flipClickHintIcon"><i class="fa fa-repeat" aria-hidden="true"></i></span>
            <span class="flipClickHintText">Zum Umdrehen klicken</span>
        </div>
    </div>
    <div class="back flashCardContentSite body-m" id="flashCard-back">
            <%= MarkdownMarkdig.ToHtml(Model.Text) %>
    <div class="flipClickHint">
        <span class="flipClickHintIcon"><i class="fa fa-repeat" aria-hidden="true"></i></span>
        <span class="flipClickHintText">Zum Umdrehen klicken</span>
    </div>
    </div>
</div>
<script type="text/javascript">
    $('#flashCardContent').flip();
</script>