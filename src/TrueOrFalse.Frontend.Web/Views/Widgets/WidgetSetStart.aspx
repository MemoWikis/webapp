<%@ Page Title="Spielen" Language="C#" 
    MasterPageFile="~/Views/Shared/Site.Widget.Master" 
    Inherits="ViewPage<WidgetSetStartModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Head" runat="server">
    <style type="text/css">
        html { height: auto;}
    </style>
    
    <%= Scripts.Render("~/bundles/js/WidgetSet") %>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    
    <%
        var hideAddToKnowledge = "";
        if(Model.HideAddToKnowledge)
            hideAddToKnowledge = "?hideAddToKnowledge=true";
    %>

    <a href="/widget/fragesatz/<%= Model.SetId + hideAddToKnowledge %>" class="btn btn-default">Teste Dein Wissen</a>

</asp:Content>