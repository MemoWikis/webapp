<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<SpacerModel>" %>

<% for (var i = 0; i < Model.AmountSpaces; i++) { %>
    <div class="SpacerDiv20<%= i == 0 && Model.AddBorderTop ? " SpacerBorderTop" : "" %>">
    </div>       
<% } %>
