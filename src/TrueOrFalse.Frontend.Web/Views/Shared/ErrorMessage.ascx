<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TrueOrFalse.View.Web.Views.Shared.ErrorMessageModel>" %>

<div style="border:1px solid red; background-color:#FFDBDD; padding:4px;">
    <%= Model.Message %>
</div>