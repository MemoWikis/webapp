<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionInSetModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="TrueOrFalse.Web" %>

<li class="questionItem ui-state-default Clearfix" data-id="<%= Model.Id %>">
    <div class="QuestionTools">
        <i class="fa fa-trash-o icon DeleteButton JS-DeleteButton show-tooltip" title="Aus dem Lernset entfernen (die Frage wird nicht gelöscht)"></i><br/>
        <% if (Model.IsCreator) { %>
            <a href="<%= Links.EditQuestion(Url, Model.Text, Model.QuestionId) %>" target="_blank">
                <i class="fa fa-pencil"></i> 
            </a>
        <% } %>

    </div>
    <div class="draggable-panel" style="float: left;"><i class="fa fa-bars" aria-hidden="true"></i></div>
    <div class="QuestionText">
        <a href="#" data-action="open-details"><i class="fa fa-chevron-right"></i></a>
        <a href="#" data-action="close-details" class="hide2"><i class="fa fa-chevron-down"></i></a>
        <%= Model.Text %>
        <%= MarkdownInit.Run().Transform(Model.TextExtended) %>
        <div>
            Richtige Antwort: <b><%= Model.CorrectAnswer %></b>
        </div>
    </div>
    <div style="display: inline-block; float: right; width: 119px;">
        <a class="btn btn-default time-button" style ="float: left; margin-right: 10px; font-size: 20px; line-height: 1em;">
            <i class="fa fa-clock-o"></i>
        </a> 
        <input style="width: 65px;"type="text" class="form-control show-tooltip" value="<%= Timecode.ToString(Model.TimeCode) %>"
               data-in-set-id="<%= Model.Id %>"
               data-input="video-timecode"
               data-original-title="Falls du oben ein Video angegeben hast: Zeitpunkt zu dem das Video pausiert und die Frage gezeigt wird." />
      
    </div>
</li>
  
