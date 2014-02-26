<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TrueOrFalse.Web.UIMessage>" %>

<div class="<%= Model.CssClass %> fade in">
    <a class="close" data-dismiss="alert" href="#">×</a>
    <%= Model.Text %>
</div>