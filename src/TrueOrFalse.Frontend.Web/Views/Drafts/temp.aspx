<%@ Page Title="temp" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<WelcomeModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Questions/Questions.css" rel="stylesheet" />
    <%= Scripts.Render("~/bundles/questions") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-sm-9">
        
    </div>
</asp:Content>