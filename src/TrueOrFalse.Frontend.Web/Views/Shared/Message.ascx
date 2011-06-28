<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TrueOrFalse.Core.Web.Message>" %>

<div style="<%= Model.Style %> padding:4px;">
    <%= Model.Text %>
</div>