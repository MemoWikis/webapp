<%@ Page Title="Mein Wissensstand" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<AlgoInsightModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content runat="server" ID="header" ContentPlaceHolderID="Head">
    
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>

    <script>
        $(function () {
        });
    </script>
    <style>
    </style>

    <%= Styles.Render("~/bundles/AlgoInsight") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <% if(Model.Message != null) { %>
        <div class="row">
            <div class="col-xs-12 xxs-stack">
                <% Html.Message(Model.Message); %>
            </div>
        </div>        
    <% } %>

    <h2 style="color: black; margin-bottom: 5px; margin-top: 0px;">
        <span class="ColoredUnderline Knowledge">Algorithmus-Einblick</span>
    </h2>
    
    <div class="alert alert-info col-md-12" style="margin-top:9px">
        <p>
            Hier erhältst du Einblick, in die Algorithmen die die <b>Antwortwahrscheinlichkeit</b> 
            und den optimalen Wiedervorlage-Zeitpunkt berechnen. MEMuchO ist Open Source<a href="https://github.com/TrueOrFalse/TrueOrFalse"> (auf Github)</a>. 
            Wir freuen uns über Verbesserungsvorschläge.
        </p>        
    </div>
    
    <% if(Model.IsInstallationAdmin) { %>
        <div class="row">
	        <div class="col-md-12" style="text-align: right">
		        <a href="<%= Url.Action("Reevaluate", "AlgoInsight") %>" class="btn btn-md btn-info">Teste Algorithmen (dauert mehrere Minuten)</a>
	        </div>
        </div>
    <% } %>
    
    <div class="row">
        <div class="col-md-6">
            <table class="table table-hover">
                <tr>
                    <th>AlgoName</th>
                    <th>%&nbsp;Erfolg</th>
                    <th>Total</th>
                    <th>Erfolge</th>
                    <th>&#216;&nbsp;Distance</th>
                </tr>
	            
                <% foreach(var summary in Model.Summaries) { %>
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
        </div>
        <div class="col-md-6">
            
        </div>
    </div>


</asp:Content>
