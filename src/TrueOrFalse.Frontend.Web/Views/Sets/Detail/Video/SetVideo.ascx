<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SetVideoModel>" %>

<input type="hidden" id="hddHideAddToKnowledge" value="<%= Model.HideAddToKnowledge %>"/>

<div class="SetVideo">
    <% if(!Model.IsInWidget) { %>
        <div class="row video-header">
            <div class="col-xs-12">
                <h4>Video zum Fragesatz mit <%= Model.QuestionCount %> Fragen.</h4>
            </div>
        </div>
    <% } %>
    <div class="">
        <div class="row">
            <div class="col-md-12">
                <%= Html.Raw(YoutubeVideo.GetIframe(Model.VideoKey)) %>
            </div>
        </div>
    </div>

    <% Html.RenderPartial("~/Views/Sets/Detail/Video/VideoPager.ascx", Model); %>

    <div id="divBodyAnswer">
        <% Html.RenderPartial("~/Views/Questions/Answer/AnswerBodyControl/AnswerBody.ascx", Model.AnswerBodyModel); %>                 
    </div>
</div>

<script src="/Views/Sets/Detail/Js/SetVideoPlayer.js"></script>
