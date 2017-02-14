<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<SingleSetCollectionModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% if(!string.IsNullOrEmpty(Model.Title)) { %>
    <h4><%= Model.Title %></h4>
<% } %>
<div class="row Cards<%= Model.CardOrientation %>">
    <% foreach (var set in Model.Sets){ %>
        <% Html.RenderPartial("~/Views/Categories/Detail/Partials/SingleSet.ascx", new SingleSetModel(set)); %>
    <% } %>
</div>