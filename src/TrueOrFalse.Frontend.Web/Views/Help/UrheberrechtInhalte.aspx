<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<HelpModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="box box-main">
    <h2><a href="<%= Url.Action(Links.HelpWillkommen, Links.HelpController) %>">Hilfe</a> - Urheberrechte</h2>
    <div class="box-content" style="min-height: 120px; clear: both; padding-top: 10px;">
        <h4>Überblick</h4>
            
        Themen<br/>
        <ul>
            <li><a href="#">Unter welcher Lizenz stehen öffentliche Fragen?</a></li>
            <li><a href="#">Unter welcher Lizenz stehen private Fragen?</a></li>
            <li><a href="#">Was muss ich beachten</a></li>
        </ul>

    </div>
</div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
