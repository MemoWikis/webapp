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
    
<div class="row">
    <div class="PageHeader col-xs-12">
        <h2 class="pull-left">
            <span class="ColoredUnderline Play">
                Spielen <%= Model.GameId %>
            </span>
        </h2>
        <div class="headerControls pull-right">
            <div>
                <a href="<%= Links.Games(Url) %>" style="font-size: 12px; margin: 0;">
                    <i class="fa fa-list"></i>&nbsp;zur Übersicht
                </a>          
            </div>
        </div>
    </div>
        
</div>

<div class="row">
    <div class="col-xs-6" style="font-size: 17px; vertical-align: bottom; line-height: 48px;">
        Du nimmst an diesem Spiel Teil <i class="fa fa-smile-o"></i>
    </div>
    <div class="col-xs-6" style="text-align: right; font-size: 30px;">
        <i class="fa fa-user" style="color: green"></i>
        <i class="fa fa-user" style="color: blue;"></i>
        <i class="fa fa-user"></i>
        <i class="fa fa-user"></i>
        <i class="fa fa-user"></i>
        <i class="fa fa-user"></i>
    </div>

</div>    
    
<div class="row">
    <div class="col-xs-12" style="padding-top: 60px; font-size: 50px;">
        Start in 1:32m
    </div>            
</div>


</asp:Content>