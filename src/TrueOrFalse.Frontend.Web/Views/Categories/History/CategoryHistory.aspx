<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<CategoryHistoryModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <%= Model.CategoryName %>
</asp:Content>
