<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TrainingSettingsDatesModel>" %>

<% 
    var index = 0;
    foreach (var trainingDate in Model.Dates) {
        index++;
%>
    <div class="row" style="margin-top: 10px" data-trainingDateId="<%=trainingDate.Id %>">
        <div class="col-md-4" style="">
            <%= trainingDate.DateTime <= DateTime.Now ? "Jetzt lernen!" : "Noch " + trainingDate.TimeSpanLabel.Full %><br />
            <%= trainingDate.DateTime.ToString("dd.MM.yyyy HH:mm") %>Uhr
        </div>
        <div class="col-md-4">
            <%= trainingDate.QuestionCount %> <%= "Frage" + StringUtils.PluralSuffix(trainingDate.QuestionCount, "n")%> <br />
            ca. <%= trainingDate.LearningTimeInMin %>min Aufwand
        </div>
        <div class="col-xs-2">
            <div data-knowledgeSummary='<%= trainingDate.SummaryBefore.ToJson() %>'></div>
        </div>
        <div class="col-xs-2">
            <div data-knowledgeSummary='<%= trainingDate.SummaryAfter.ToJson() %>' ></div>
        </div>
    </div>
<% } %>