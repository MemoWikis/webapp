<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TabRepetitionModel>" %>

<div class="row" >
    <div class="col-md-12" style="margin-top:3px; margin-bottom:7px;">
        <h3>Vergleich Wiedervorlage-Strategien</h3>
    </div>
</div>

<table class="table table-striped table-hover">
    <thead>
        <tr>
            <th>Mustername</th>
            <th>Anzahl Treffer</th>
            <th>Mit nächster Frage</th>
            <th>Ø Erfolgsrate
            </th>
        </tr>
    </thead>

<% foreach(var pattern in Model.PatternInfos) {%>
    <tr>
        <td style="padding-right: 15px;" ><%= pattern.Name %></td>
        <td><%= pattern.MatchedAnswersCount %></td>
        <td><%= pattern.NextAnswers.Count %></td>
        <td><%= pattern.NextAnswers.Count > 0 ? Math.Round(pattern.NextAnswerAvgCorrect() * 100, 0) + "%" : "" %></td>
    </tr>
<% } %>
    
</table>