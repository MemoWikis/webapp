<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TrainingSettingsDatesModel>" %>

<% 
    var index = 0;
    foreach (var trainingDate in Model.Dates) {
        index++;
%>
    <div class="row" style="margin-top: 10px">
        <div class="col-md-4" style="">
            Noch <%= trainingDate.TimeSpanLabel.Full %><br />
            <%= trainingDate.DateTime.ToString("dd.mm.yyyy HH:MM") %>Uhr
        </div>
        <div class="col-md-4">
            <%= trainingDate.QuestionCount %> <%= "Frage".Plural(trainingDate.QuestionCount, "n")%> <br />
            ca. <%= trainingDate.Minutes %>min Aufwand
        </div>
        <div class="col-xs-2">
            <div id="chartKnowledgeDate<%= index %>Before"></div>
        </div>
        <div class="col-xs-2">
            <div id="chartKnowledgeDate<%= index %>After"></div>
        </div>
    </div>
<% } %>