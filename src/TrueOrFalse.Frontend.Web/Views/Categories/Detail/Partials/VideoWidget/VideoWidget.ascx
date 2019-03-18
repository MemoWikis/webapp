<%@ Control Language="C#" AutoEventWireup="true"  Inherits="System.Web.Mvc.ViewUserControl<VideoWidgetModel>" %>

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperStart.ascx") %>
                <%-- <div>WIP not working with vue</div> --%>

    <div ref:script-widget :data-id="<%= Model.SetId %>"></div>
    <%-- <script src="https://memucho.de/views/widgets/w.js" data-t="setVideo" data-id="<%= Model.SetId %>" data-width="100%"></script> --%>

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperEnd.ascx") %>

