<%@ Page Title="Spielen" Language="C#" 
    MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
    Inherits="ViewPage<PlayModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Games/Edit/Game.css" rel="stylesheet" />    
    <%= Scripts.Render("~/bundles/js/GamePlay") %>
    <%= Styles.Render("~/bundles/GamePlay") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
<input type="hidden" id="RoundCound" value="<%= Model.RoundCount %>"/>
    
<div class="row">
    <div class="PageHeader col-xs-12">
        <h3 class="pull-left" style="margin-top: 0px">
            <span class="ColoredUnderline Play">
                Spiel: Quiz 
            </span>
        </h3>
        <div class="headerControls pull-right">
            <div>
                <a href="<%= Links.Games(Url) %>" style="font-size: 12px; margin: 0;">
                    <i class="fa fa-list"></i>&nbsp;zur Übersicht
                </a>
            </div>
        </div>
    </div>        
</div>
    
<%
    const string path = "~/Views/Games/Play/BodyControls/";
    switch (Model.Game.Status)
    {
        case GameStatus.InProgress:
            Html.RenderPartial(path + "GameInProgressPlayer.ascx", new GameInProgressPlayer(Model.Game)); break;
        case GameStatus.Completed:
            Html.RenderPartial(path + "GameCompleted.ascx", new GameCompleted(Model.Game)); break;
        case GameStatus.NeverStarted:
            Html.RenderPartial(path + "GameNeverStarted.ascx", new GameNeverStarted(Model.Game)); break;
        case GameStatus.Ready:
            Html.RenderPartial(path + "GameReady.ascx", new GameReady(Model.Game)); break;
    }    
 %>    
</asp:Content>