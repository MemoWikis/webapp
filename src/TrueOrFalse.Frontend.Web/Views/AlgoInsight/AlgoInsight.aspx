<%@ Page Title="Algorithmus-Einblick" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<AlgoInsightModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="NHibernate.Util" %>

<asp:Content runat="server" ID="header" ContentPlaceHolderID="Head">
    
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
    
    <% Html.RenderPartial("TabForecast", new TabForecastModel()); %>
    
    <% if(Model.IsInstallationAdmin) { %>
        <div class="row">
	        <div class="col-md-12" style="text-align: right; margin-top: 50px;">
		        <a href="<%= Url.Action("Reevaluate", "AlgoInsight") %>" class="btn btn-md btn-info">Teste Algorithmen (dauert mehrere Minuten)</a>
	        </div>
        </div>
    <% } %>

</asp:Content>
