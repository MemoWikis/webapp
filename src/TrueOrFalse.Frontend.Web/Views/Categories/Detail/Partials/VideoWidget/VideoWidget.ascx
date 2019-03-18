<%@ Control Language="C#" AutoEventWireup="true"  Inherits="System.Web.Mvc.ViewUserControl<VideoWidgetModel>" %>

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperStart.ascx") %>

    <content-module-widget widget-type="video" :widget-id="widgetId" src="https://memucho.de/views/widgets/w.js" data-t="setVideo" data-id="<%= Model.SetId %>" data-width="100%" data-maxwidth="" data-logoon="" data-hide-knowledge-btn=""></content-module-widget>
    <span :ref="widgetId"></span>    

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperEnd.ascx") %>

 