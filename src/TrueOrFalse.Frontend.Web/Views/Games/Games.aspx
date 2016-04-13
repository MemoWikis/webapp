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
    
    <input type="hidden" id="hddCurrentUserId" value="<%= Model.UserId %>"/>

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
            
            <div class="bs-callout bs-callout-info" id="divGamesInProgressNone"
                 style="margin-top: 0; <%= Html.CssHide(Model.GamesInProgress.Any()) %>">
                <h4>Keine laufenden Spiele</h4>
                <p>
                    <a href="<%= Url.Action("Create", "Game") %>" class="btn btn-sm" style="margin-top: 10px;">
                        <i class="fa fa-plus-circle"></i> &nbsp; Spiel erstellen
                    </a>
                </p>
            </div>
            
            <div id="divGamesInProgress" style="<%= Html.CssHide(!Model.GamesInProgress.Any()) %>">
                <% foreach(var game in Model.GamesInProgress){ %>
                    <% Html.RenderPartial("GameRow", game); %>
                <% } %>
            </div>

            <h3 style="margin-bottom: 10px;">
                <span class="ColoredUnderline Play" style="padding-right: 3px;">Mitspielen:</span>
            </h3>

            <div id="divGamesReadyNone" class="bs-callout bs-callout-info" 
                 style="margin-top: 0; <%= Html.CssHide(Model.GamesReady.Any()) %>">
                <h4>Keine kommenden Spiele</h4>
                <p>
                    <a href="<%= Url.Action("Create", "Game") %>" class="btn btn-sm" style="margin-top: 10px;">
                        <i class="fa fa-plus-circle"></i> &nbsp; Spiel erstellen
                    </a>
                </p>
            </div>
            
            <div id="divGamesReady" style="<%= Html.CssHide(!Model.GamesReady.Any()) %>">
                <% foreach(var game in Model.GamesReady){ %>
                    <% Html.RenderPartial("GameRow", game); %>
                <% } %>
            </div>
            
        </div>
        <div class="col-md-3">
            <div class="panel panel-default" style="border: none;">
                <div class="panel-heading">Spiel-Empfehlungen</div>
                <div class="panel-body">
                    <% foreach(var set in Model.SuggestedGames) { %>
                        <div class="row" style="margin-bottom: 10px;">
                            <div class="col-xs-3">
                                <%= ImageFrontendData.Create(set).RenderHtmlImageBasis(200, true, ImageType.QuestionSet) %>
                            </div>
                            <div class="col-xs-9" style="">
                                <a href="<%= Links.GameCreateFromSet(set.Id) %>"><%= set.Name %></a>
                            </div>
                        </div>
                    <% } %>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    
                </div>
            </div>
        </div>
    </div>
</asp:Content>