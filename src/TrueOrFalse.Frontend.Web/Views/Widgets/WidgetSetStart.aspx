<%@ Page Title="Spielen" Language="C#" 
    MasterPageFile="~/Views/Shared/Site.Widget.Master" 
    Inherits="ViewPage<WidgetSetStartModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

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
    
    <%= Scripts.Render("~/bundles/js/AwesomeIframe") %>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    

    <div id="mainDiv">
        <p class="titleP">
            <%= Model.SetName %>
        </p>
        <p class="description">
            <%= Model.SetText %>
        </p>
        <p class="buttonP">
            <a href="<%= Model.StartSessionUrl %>" class="btn btn-lg btn-primary">
                <i class="fa fa-play-circle">&nbsp;&nbsp;</i>Teste Dein Wissen
            </a>
        </p>
        
    </div>

</asp:Content>