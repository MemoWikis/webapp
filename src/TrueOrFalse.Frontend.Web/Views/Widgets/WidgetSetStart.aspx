<%@ Page Title="Spielen" Language="C#" 
    MasterPageFile="~/Views/Shared/Site.Widget.Master" 
    Inherits="ViewPage<WidgetSetStartModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Head" runat="server">
    <style type="text/css">
        html {
            height: auto;
        }
        #mainDiv {
            padding: 45px 19px 55px 19px;
            text-align: center;
            color: #203256;
        }
        .titleP {
            margin-bottom: 10px;
            font-weight: bold;
            font-size: 160%;
        }
        .description {
            margin: 10px 5px;
            font-size: 110%;
        }
        .buttonP {
            margin-top: 30px;
        }
    </style>
    
    <% if(Model.IncludeCustomCss){ %>
        <link href="<%= Model.CustomCss %>" rel="stylesheet" />
    <% } %>
    
    <%= Scripts.Render("~/bundles/js/AwesomeIframe") %>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    

    <div id="mainDiv">
        <p class="titleP">
            Der Support für unsere Widgets läuft aus. 
        </p>
        <p class="description">
            Der Support für unsere Widgets endet am 23.2.2020. Die eingebundenen Widgets können nach diesem Datum nicht mehr aufgerufen werden.
            <b>Alle Inhalte wurden gesichert und migriert</b>, natürlich bleiben auch alle Fragen erhalten.
            <br/>
            Wir Danken Ihnen für Ihr Interesse an diesem Feature und die mit uns verbrachte Zeit.<br/>
            <br/>
            Du hast Fragen? Sprich uns einfach an, wir freuen uns über deine Nachricht: 
            <a href="mailto:christof@memucho.de?Subject=Widgets" target="_top">christof@memucho.de</a>
        </p>

    </div>

</asp:Content>