<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="row BoxButtonBar">
    <div class="BoxButtonColumn">
        <div class="BoxButton">
            <div class="BoxButtonIcon"><i class="fa fa-heart"></i></div>
            <div class="BoxButtonText">
                <span>Spiel starten</span>
            </div>
            <a></a>
        </div>
    </div>
    <div class="BoxButtonColumn">
        <div class="BoxButton">
            <div class="BoxButtonIcon"><i class="fa fa-archive"></i></div>
            <div class="BoxButtonText">
                <span>Prüfungstermin anlegen</span> 
            </div>
            <a></a>
        </div>
    </div>
    <div class="BoxButtonColumn">
        <div class="BoxButton">
            <div class="BoxButtonIcon"><i class="fa fa-archive"></i></div>
            <div class="BoxButtonText">
                <span>Üben</span>
            </div>
        </div>
    </div>
    <div class="BoxButtonColumn">
        <div class="BoxButton">
            <div class="BoxButtonIcon"><i class="fa fa-archive"></i></div>
            <div class="BoxButtonText">
                <span>Wissen testen</span>
            </div>
        </div>
    </div>
</div>