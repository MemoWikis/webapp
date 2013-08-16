<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<HelpModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="span10" style="">
    <div class="box box-main">
        <h2><a href="<%= Url.Action(Links.HelpWillkommen, Links.HelpController) %>">Hilfe</a> - Reputation</h2>
        <div class="box-content" style="min-height: 120px; clear: both; padding-top: 10px;">
            <h4>Überblick</h4>
        </div>
    </div>
</div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
