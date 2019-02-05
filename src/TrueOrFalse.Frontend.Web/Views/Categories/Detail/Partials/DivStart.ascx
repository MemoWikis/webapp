<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<DivStartModel>" %>

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperStart.ascx") %>
                
    <div class="<%: Model.CssClass %>">
    <ul class="module" v-sortable="options" style="list-style-type: none;">
    