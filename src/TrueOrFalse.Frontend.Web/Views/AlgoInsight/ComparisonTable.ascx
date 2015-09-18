<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="ViewUserControl<ComparisonTableModel>" %>


<% if(Model.IsFeatureView) { %>
    <div class="row">
        <a data-toggle="collapse" href="#<%=Model.ToggleId %>" aria-expanded="false">
            <div class="col-md-11">
                <h4 style="display: inline-block; margin-right: 10px;">                
                    <%= Model.Feature.Name %>
                </h4>        
                ( <span data-original-title="Bester Algorithmus" class="show-tooltip">
                        <i class="fa fa-trophy"></i> 
                    '<%= Model.Winner.Algo.Name %>'
                  </span>
            
                <span data-html="true" data-original-title="<div style='text-align:left'>Antworten gesamt: <%= Model.Winner.TestCount %>. <br> Erfolgsrate: %<%= Model.Winner.SuccessRateInPercent %></div>" 
                      class="show-tooltip">
                    <%= String.Format("(Σ {0} / %{1} )",
                        Model.Winner.TestCount, 
                        Model.Winner.SuccessRateInPercent) %>
                </span>
            
            </div>
        </a>
        <div class="col-md-1" style="text-align: right; margin-top: 11px;">
            <a class="btn btn-xs" data-toggle="collapse" href="#<%=Model.ToggleId %>" aria-expanded="false"><i class="fa fa-caret-square-o-down"></i></a>
        </div>
    </div>
<% } %>

<table class="table table-hover <%= Model.ShowCollapsed ? "collapse" : "" %>" id="<%=Model.ToggleId %>">
    <tr>
        <th>AlgoName</th>
        <th>%&nbsp;Erfolg</th>
        <th>Total</th>
        <th>Erfolge</th>
        <th>&#216;&nbsp;Distanz</th>
    </tr>
	            
    <% foreach(var summary in Model.Summaries.OrderByDescending(s => s.SuccessRate)) { %>
        <tr>
	        <td>
	            <%= summary.Algo.Name %> 
                <i class="fa fa-info-circle show-tooltip" data-original-title="<%= summary.Algo.Details %>"></i>
	        </td>
	        <td><%= summary.SuccessRate %></td>
            <td><%= summary.TestCount %></td>
	        <td><%= summary.SuccessCount %></td>
	        <td><%= summary.AvgDistance %></td>
        </tr>
    <% } %>
</table>