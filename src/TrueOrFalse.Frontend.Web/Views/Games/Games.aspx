<%@ Page Title="Spielen" Language="C#" 
    MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
    Inherits="ViewPage<GamesModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Scripts.Render("~/bundles/js/Games") %>
    <%= Styles.Render("~/bundles/Games") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-9">
        
            <a href="<%= Url.Action("Create", "Game") %>" class="btn btn-success btn-sm">
                <i class="fa fa-plus-circle"></i> Spiel erstellen
            </a>

            <h3 style=" margin-bottom: 10px;">
                <span class="ColoredUnderline Play" style="padding-right: 3px;">Laufende Spiele</span>
            </h3> 
            
            <% foreach(var game in Model.GamesInProgress){ %>
                <% Html.RenderPartial("GameRow", game); %>
            <% } %>
                                    
            <h3 style=" margin-bottom: 10px;">
                <span class="ColoredUnderline Play" style="padding-right: 3px;">Mitspielen:</span>
            </h3>                 
            
            <% foreach(var game in Model.GamesReady){ %>
                <% Html.RenderPartial("GameRow", game); %>
            <% } %>

        </div>
        <div class="col-md-3"></div>
    </div>
</asp:Content>