<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SetVideoModel>" %>

<div class="row">
    <div class="col-xs-12">
        <h4 style="margin-top:-10px; margin-bottom: 20px;">Video zum Fragesatz mit <%= Model.QuestionCount %> Fragen.</h4>
    </div>
</div>
<div>
    <div class="row">
        <div class="col-md-12">
            <%= Html.Raw(YoutubeVideo.GetIframe(Model.VideoKey)) %>
        </div>
    </div>
</div>


<% Html.RenderPartial("~/Views/Sets/Detail/Video/VideoPager.ascx", Model); %>

<div style="background-color: white;">
    <div id="divBodyAnswer">
        <% Html.RenderPartial("~/Views/Questions/Answer/AnswerBodyControl/AnswerBody.ascx", Model.AnswerBodyModel); %>                 
    </div>
</div>

<script src="/Views/Sets/Detail/Js/SetVideoPlayer.js"></script>
