<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="row SchnurpsButtonBar">
    <div class="BoxButtonColumn ">
        <div class="SchnurpsButton ButtonOnHover">
            <div class="ButtonContent">
                <i class="fa fa-gamepad"></i>
                <span> Spiel starten</span>
            </div>
            <a></a>
        </div>
    </div>
    <div class="BoxButtonColumn ">
        <div class="SchnurpsButton ButtonOnHover">
            <div class="ButtonContent">
                <i class="fa fa-calendar"></i>
                <span> Prüfungs- <br/> termin anlegen</span> 
            </div>
                <a href="<%= Links.DateCreateForCategory(Model.Id) %>" rel="nofollow"></a>
        </div>
    </div>
    <div class="BoxButtonColumn ">
        <div class="SchnurpsButton ButtonOnHover">
            <div class="ButtonContent">
                <i class="fa fa-line-chart"></i>
                <span> Üben</span>
           </div>
        </div>
    </div>
    <div class="BoxButtonColumn ">
        <div class="SchnurpsButton ButtonOnHover">
            <div class="ButtonContent">
                <i class="fa fa-play-circle"></i>
                <span> Wissen testen</span>
            </div>
        </div>
    </div>
</div>

<%--<div class="row BoxButtonBar">
    <div class="BoxButtonColumn">
        <div class="BoxButton">
            <div class="BoxButtonIcon"><i class="fa fa-gamepad"></i></div>
            <div class="BoxButtonText">
                <span>Spiel starten</span>
            </div>
            <a></a>
        </div>
    </div>
    <div class="BoxButtonColumn">
        <div class="BoxButton">
            <div class="BoxButtonIcon"><i class="fa fa-calendar"></i></div>
            <div class="BoxButtonText">
                <span>Prüfungstermin anlegen</span> 
            </div>
            <a href="<%= Links.DateCreateForCategory(Model.Id) %>" rel="nofollow"></a>
        </div>
    </div>
    <div class="BoxButtonColumn">
        <div class="BoxButton">
            <div class="BoxButtonIcon"><i class="fa fa-line-chart"></i></div>
            <div class="BoxButtonText">
                <span>Üben</span>
            </div>
        </div>
    </div>
    <div class="BoxButtonColumn">
        <div class="BoxButton">
            <div class="BoxButtonIcon"><i class="fa fa-play-circle"></i></div>
            <div class="BoxButtonText">
                <span>Wissen testen</span>
            </div>
        </div>
    </div>
</div>--%>