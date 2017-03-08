<%@ Page Title="Spielen" Language="C#" 
    MasterPageFile="~/Views/Shared/Site.PureContent.Master" 
    Inherits="ViewPage<WidgetSetModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Scripts.Render("~/bundles/js/WidgetSet") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Widget set</h1>    
    <div style="width: 300px; height: 500px; background-color: blue;"></div>
</asp:Content>