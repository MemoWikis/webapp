<%@ Page Title="Spielen" Language="C#" 
    MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
    Inherits="ViewPage<GamesModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Scripts.Render("~/bundles/js/Games") %>
    <%= Styles.Render("~/bundles/Games") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="PageHeader col-md-6">
            <h3 style="margin-bottom: 20px; margin-top: 0px;" class="pull-left">
                <span class="ColoredUnderline Play" style="padding-right: 3px;">Laufende Spiele</span>
            </h3> 
        </div>
        <div class="col-md-3">
            <div class="headerControls pull-right">
                <div style="padding-top: 5px;">
                    <a href="<%= Url.Action("Create", "Game") %>" class="btn btn-sm pull-right">
                        <i class="fa fa-plus-circle"></i> &nbsp; Spiel erstellen
                    </a>
                </div>
            </div>
        </div>
    </div>
    
    <div class="row">
        <div class="col-md-9">
            
            <% if(!Model.GamesInProgress.Any()){ %>
                <div class="bs-callout bs-callout-info" style="margin-top: 0;">
                    <h4>Keine laufenden Spiele</h4>
                    <p>
                        <a href="<%= Url.Action("Create", "Game") %>" class="btn btn-sm" style="margin-top: 10px;">
                            <i class="fa fa-plus-circle"></i> &nbsp; Spiel erstellen
                        </a>
                    </p>
                </div>            
            <% } else { %>
                <% foreach(var game in Model.GamesInProgress){ %>
                    <% Html.RenderPartial("GameRow", game); %>
                <% } %>
            <% } %>

            <h3 style="margin-bottom: 10px;">
                <span class="ColoredUnderline Play" style="padding-right: 3px;">Mitspielen:</span>
            </h3>                 
            
            <% if(!Model.GamesReady.Any()){ %>
                <div class="bs-callout bs-callout-info" style="margin-top: 0;">
                    <h4>Keine kommenden Spiele</h4>
                    <p>
                        <a href="<%= Url.Action("Create", "Game") %>" class="btn btn-sm" style="margin-top: 10px;">
                            <i class="fa fa-plus-circle"></i> &nbsp; Spiel erstellen
                        </a>
                    </p>
                </div>            
            <% } else { %>
                <% foreach(var game in Model.GamesReady){ %>
                    <% Html.RenderPartial("GameRow", game); %>
                <% } %>
            <% } %>
        </div>
        <div class="col-md-3"></div>
    </div>
</asp:Content>