<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TabVariousModel>" %>

<h3>Frage-Klassifizierung</h3>
<table>
    <tr>
        <th>Name</th>
        <th>Anzahl Fragen</th>
    </tr>
    <% foreach(var feature in Model.QuestionFeatures) { %>
        <tr>
            <td style="padding-right: 10px;"><%= feature.Name %></td>
            <td><%= Model.GetQuestionCount(feature.Id) %></td>
        </tr>
    <% } %>
</table>

<% if(Model.IsInstallationAdmin) { %>
    <div class="row">
	    <div class="col-md-12" style="text-align: right; margin-top: 50px;">
		    <a href="<%= Url.Action("ReevaluateQuestionFeatures", "AlgoInsight") %>" class="btn btn-md btn-info">Fragen neu klassifizieren</a>
	    </div>
    </div>
<% } %>