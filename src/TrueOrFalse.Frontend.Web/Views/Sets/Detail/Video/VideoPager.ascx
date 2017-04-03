<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SetVideoModel>" %>

<div class="Box" id="video-pager">
    <div class="row">
        <div class="col-xs-12">
                                    <div class="pause-buttons" style="float: right;">
                                    <a href="#">
                                        <i class="fa fa-pause-circle-o show-tooltip hide2" 
                                            id="stopPausingVideo"
                                            data-original-title="Nicht mehr automatisch pausieren"
                                            aria-hidden="true" style="font-size: 22px;"></i>
                                    </a>
            
                                    <a href="#">
                                        <i class="fa fa-play-circle-o show-tooltip hide2" 
                                            id="startPausingVideo"
                                            data-original-title="Automatisch pausieren"
                                            aria-hidden="true" style="font-size: 22px; padding:13px;"></i>
                                    </a>
                                </div>
            <div style="margin-right: 7px; float: left; font-size: 18px;">Fragen</div>

            <% 
            var i = 0;
            foreach (var questionInSet in Model.QuestionsInSet)
            {
                i++;
                var question = questionInSet.Question;
                var classCurrent = question.Id == Model.CurrentQuestion ? "btn-default btn-info current" : "btn-default";
            %>
                <a href="#" class="btn btn-sm <%= classCurrent %>" data-video-question-id="<%= question.Id %>" data-video-pause-at="<%= questionInSet.Timecode %>"><%= i %></a>
            <% } %>
        </div>
        <%--<div class="col-xs-1 pause-buttons">
            <a href="#">
                <i class="fa fa-pause-circle-o show-tooltip hide2" 
                    id="stopPausingVideo"
                    data-original-title="Nicht mehr automatisch pausieren"
                    aria-hidden="true" style="font-size: 22px; padding:13px;"></i>
            </a>
            
            <a href="#">
                <i class="fa fa-play-circle-o show-tooltip hide2" 
                    id="startPausingVideo"
                    data-original-title="Automatisch pausieren"
                    aria-hidden="true" style="font-size: 22px; padding:13px;"></i>
            </a>
        </div>--%>
    </div>
    <div class="Divider" style="margin-top: 15px;"></div>
</div>