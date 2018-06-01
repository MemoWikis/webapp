<%@ Page Title="Spielen" Language="C#" 
    MasterPageFile="~/Views/Shared/Site.Sidebar.Master" 
    Inherits="ViewPage<PlayModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Games/Edit/Game.css" rel="stylesheet" />    
    <%= Scripts.Render("~/bundles/js/GamePlay") %>
    <%= Styles.Render("~/bundles/GamePlay") %>
    <%= Scripts.Render("~/bundles/js/Game") %>
    <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Quiz", Url = Model.Game.Id.ToString(), ImageClass = "fa-gamepad", ToolTipText = "Quiz"});
       Model.TopNavMenu.IsCategoryBreadCrumb = false; %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
<input type="hidden" id="RoundCound" value="<%= Model.RoundCount %>"/>
<input type="hidden" id="GameId" value="<%= Model.Game.Id %>"/>

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

<div id="divGameBody">
<%
    const string path = "~/Views/Games/Play/BodyControls/";
    switch (Model.Game.Status)
    {
        case GameStatus.InProgress:
            Html.RenderPartial(path + "GameInProgressPlayer.ascx", new GameInProgressPlayerModel(Model.Game)); break;
        case GameStatus.Completed:
            Html.RenderPartial(path + "GameCompleted.ascx", new GameCompletedModel(Model.Game)); break;
        case GameStatus.NeverStarted:
            Html.RenderPartial(path + "GameNeverStarted.ascx", new GameNeverStartedModel(Model.Game)); break;
        case GameStatus.Ready:
            Html.RenderPartial(path + "GameReady.ascx", new GameReadyModel(Model.Game)); break;
    }    
 %>  
</div>    
</asp:Content>