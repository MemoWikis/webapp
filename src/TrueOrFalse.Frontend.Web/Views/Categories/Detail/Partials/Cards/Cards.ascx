<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<CardsModel>" %>

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperStart.ascx") %>

    <% if(!string.IsNullOrEmpty(Model.Title)) { %>
        <h4><%= Model.Title %></h4>
    <% } %>
    <div class="row Cards<%= Model.CardOrientation %>">
        <% foreach (var set in Model.Sets){ %>
            <%: Html.Partial("~/Views/Categories/Detail/Partials/SingleSet/SingleSet.ascx", new SingleSetModel(set)) %>
        <% } %>
    </div>


<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperEnd.ascx") %>


  