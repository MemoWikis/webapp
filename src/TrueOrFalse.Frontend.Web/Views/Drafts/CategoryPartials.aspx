<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="System.Web.Mvc.ViewPage<CategoryModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    
    <link href="/Style/site.css" rel="stylesheet" type="text/css" />
    <link href="/Views/Categories/Detail/Category.css" rel="stylesheet" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <%=  Model.CustomPageHtml%>
        
    

</asp:Content>
