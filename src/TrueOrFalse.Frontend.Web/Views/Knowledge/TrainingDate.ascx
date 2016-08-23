<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<TrainingDateModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="row">
    <div class="col-md-12" style="font-weight: bold; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">
        <% if(!Model.IsContinousLearning){ %>
            Termin: <a href="<%= Links.Dates() %>"><%= Model.GetTitle() %></a>
        <% }else{
            Response.Write(Model.GetTitle());
        }%>
    </div>
</div>
<div class="row">
    <div class="col-md-6">
        Noch <%= Model.TimeSpanLabel.Full %>
    </div>
    <div class="col-md-6">
        <%= Model.QuestionCount %> <%= "Frage" + StringUtils.Plural(Model.QuestionCount, "n")%> 
    </div>
</div>
<div class="row" style="font-size: 12px; margin-bottom: 13px;">
    <div class="col-md-6">
        <%= Model.DateTime.ToString("dd.MM.yyyy HH:mm") %> Uhr
    </div>
    <div class="col-md-6">
        ca. <%= Model.LearningTimeInMin %> min Aufwand
    </div>
</div>