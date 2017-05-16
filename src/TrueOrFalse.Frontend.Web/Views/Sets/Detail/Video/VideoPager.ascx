<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SetVideoModel>" %>

<div class="Box" id="video-pager" style="padding-top: 10px;">
    <div class="row">
        <div class="col-xs-12 pager-block">            
            <table style="float: left;" id="tablePager">
                <tr>
                    <td>
                        <a id="videoPreviousQuestion" href="#" data-original-title="Vorherige Frage" class="show-tooltip"><i class="fa fa-arrow-circle-left" aria-hidden="true" style=""></i></a>
                    </td>
                    <td>
                        <div class="pages" id="videoPages" data-question-count="<%= Model.QuestionCount %>">
                            <% 
                            var i = 0;
                            foreach (var questionInSet in Model.QuestionsInSet)
                            {
                                i++;
                                var question = questionInSet.Question;
                                var isCurrent = question.Id == Model.CurrentQuestion;
                                var classLink = isCurrent ? "current" : "";
                                var classSymbol = isCurrent ? "fa-circle" : "fa-circle-thin";
                            %>
                    
                                <a href="#" class="page <%= classLink %> show-tooltip" data-index="<%= i %>" 
                                   data-original-title="Zeige Frage (bei <%= questionInSet.FormatedTimecode()  %>)"
                                   data-video-question-id="<%= question.Id %>" data-video-pause-at="<%= questionInSet.Timecode %>" >
                                    <i class="fa <%= classSymbol %>" aria-hidden="true"></i>
                                </a>
                            <% } %>
                        </div>                                        
                    </td>
                    <td><a id="videoNextQuestion" href="#" data-original-title="Nächste Frage" class="show-tooltip"><i class="fa fa-arrow-circle-right" aria-hidden="true"></i></a></td>
                </tr>
            </table>
            <div class="pause-buttons" style="display: inline-block; float:right">
                <a href="#" id="syncVideo">
                    <i class="fa fa-refresh show-tooltip" 
                        id="syncVideoWithQuestion"
                        data-original-title="Springe zur Stelle im Video"
                        aria-hidden="true"></i>
                </a>
                <a href="#">
                    <i class="fa fa-pause-circle-o show-tooltip hide2" 
                        id="stopPausingVideo"
                        data-original-title="Nicht mehr automatisch pausieren"
                        aria-hidden="true"></i>
                </a>
                <a href="#">
                    <i class="fa fa-play-circle-o show-tooltip hide2" 
                        id="startPausingVideo"
                        data-original-title="Automatisch pausieren"
                        aria-hidden="true"></i>
                </a>
            </div>
        </div>
    </div>
    
    <div class="Divider" style="margin-top: 15px;"></div>
</div>