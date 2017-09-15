<%@ Page Title="Spielen" Language="C#" 
    MasterPageFile="~/Views/Shared/Site.Widget.Master" 
    Inherits="ViewPage<WidgetSetVideoModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <%var canonicalUrl = Settings.CanonicalHost + Links.SetDetail(Model.SetName, Model.SetId); %>    
    <link rel="canonical" href="<%= canonicalUrl %>">

    <% Title = Model.MetaTitle; %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/Set") %>
    <%= Styles.Render("~/bundles/AnswerQuestion") %>
    <%= Scripts.Render("~/bundles/js/WidgetSetVideo") %>
    
    <style type="text/css">
        div.video-header h4 { margin-top:0px; }
        html { height: auto;}
    </style>
    
    <% if(Model.IncludeCustomCss){ %>
        <link href="<%= Model.CustomCss %>" rel="stylesheet" />
    <% } %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">    
    <% 
        if (Model.Set.HasVideo){
            Html.RenderPartial("/Views/Sets/Detail/Video/SetVideo.ascx", new SetVideoModel(Model.Set, Model.HideAddToKnowledge, isInWidget: true));
        }else{
           %><h2 style="padding-bottom: 30px;">Diesem Lernset ist kein Video zugeordnet</h2><%
       } 
    %>
</asp:Content>