<%@ Page Title="Algorithmus-Einblick" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<AlgoInsightModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

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

    <div class="row" >
        <div class="col-md-12" style="margin-top: -20px; margin-bottom: 10px;">
            <h3>Vergleich Algorithmen</h3>
            <p>
                Gezeigt wird, wie gut unterschiedliche Algorithmen
                bisher gegebene Antworten korrekt vorhergesagt hätten.
                Getestet wird jede bisher gegebene Antwort, jeweils mit
                allen vorherigen Antworten. 
            </p> 
            <p>
                Die berechnete Antwortwahrscheinlichkeit wird 
                mit der tatsächlich gegebenen Antwort verglichen. 
                Hieraus ergibt sich die durchschnittliche Erfolgsrate (% Erfolg). 
            </p>
        </div>
    </div>

    <div class="row">
        <div class="col-md-6">
            <div class="row">
                <div class="col-md-12">
                    <h3>Alle Antworten</h3>
                </div>
            </div>
            <% Html.RenderPartial("ComparisonTable", new  ComparisonTableModel(Model.Summaries.ToList())); %>
        </div>
        <div class="col-md-6"></div>
    </div>
    
    <div class="row">
        <div class="col-md-12">
            <h3>Feature: Vorherige Wiederholungen</h3>
        </div>
    </div>
    
    <% foreach(var repetitionFeature in Model.RepetitionFeatureModels) { %>
        <% if (!repetitionFeature.Summaries.Any()){continue;} %>
        <div class="row">
            <div class="col-md-6">
                <% Html.RenderPartial("ComparisonTable", new  ComparisonTableModel(repetitionFeature)); %>
            </div>
            <div class="col-md-6"></div>
        </div>
    <% } %>
    
    <% if(Model.IsInstallationAdmin) { %>
        <div class="row">
	        <div class="col-md-12" style="text-align: right">
		        <a href="<%= Url.Action("Reevaluate", "AlgoInsight") %>" class="btn btn-md btn-info">Teste Algorithmen (dauert mehrere Minuten)</a>
	        </div>
        </div>
    <% } %>

</asp:Content>
