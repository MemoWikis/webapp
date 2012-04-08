<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<MaintenanceModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Maintenance</h2>
    
    <br/><br/>
    
    <% Html.Message(Model.Message); %>
    
    <a class="btn btn-large" href="CalcAggregatedValues">Aggregierte Zahlen für Fragen aktualisieren</a>

</asp:Content>

