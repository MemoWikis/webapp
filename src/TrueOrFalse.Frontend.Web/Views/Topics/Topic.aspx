<%@ Page Title="Thema" Language="C#"
    MasterPageFile="~/Views/Shared/Site.Sidebar.Master"
    Inherits="ViewPage<TopicModel>" %>
    <%@ Import Namespace="System.Web.Optimization" %>
    <%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    
    <%= Html.Raw(Model.Content) %>

</asp:Content>