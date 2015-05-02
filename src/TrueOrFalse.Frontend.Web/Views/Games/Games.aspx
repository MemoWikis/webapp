<%@ Page Title="Spielen" Language="C#" 
    MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
    Inherits="ViewPage<GamesModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>


<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Scripts.Render("~/bundles/js/Games") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-9">
        
            <a href="<%= Url.Action("Create", "Game") %>" class="btn btn-success btn-sm">
                <i class="fa fa-plus-circle"></i> Spiel erstellen
            </a>

            <h3 style=" margin-bottom: 20px;">
                <span class="ColoredUnderline Play" style="padding-right: 3px;">Laufende Spiele</span>
            </h3>
            
            <% foreach(var games in Model.Games){ %>
                <div class="row">
                    <div class="msg">
                        <div class="col-xs-12 header">
                            <h4>Quiz (1 / 27) (Startet in spät. 30 sec)</h4>
                        </div>

                        <div class="col-xs-12 body">
                            Gespielt wird mit den Fragesätzen:
                        </div>
                        <div class="col-xs-12 body">
                            Spieler: 
                        </div>
                    </div>
                </div>

            <% } %>        

        </div>
        <div class="col-md-3"></div>
    </div>
</asp:Content>