<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CategoryRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<p>
    <%=Model.CategoryName%>
    <span style="float: right"><%= Html.ActionLink("Bearbeiten", "action?", "controller?")%></span>
</p>