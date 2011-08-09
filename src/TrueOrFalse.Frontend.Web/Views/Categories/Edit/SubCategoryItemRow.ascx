<%@ Control Language="C#" Inherits="ViewUserControl<SubCategoryItemRowModel>" %>
<%@ Import Namespace="ListBinding.Helpers" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% using(Html.BeginCollectionItem("row")) { %>
   <li><%= Model.Name %></li>
<% } %>