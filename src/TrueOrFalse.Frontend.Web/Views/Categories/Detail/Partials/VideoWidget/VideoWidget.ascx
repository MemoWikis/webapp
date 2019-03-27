<%@ Control Language="C#" AutoEventWireup="true"  Inherits="System.Web.Mvc.ViewUserControl<VideoWidgetModel>" %>

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperStart.ascx") %>

    <content-module-widget 
        :widget-id="widgetId" 
        widget-type="video" 
        src="https://memucho.de/views/widgets/w.js" 
        data-t="setVideo" 
        data-id="<%= Model.SetId %>" 
        data-width="100%">
    </content-module-widget>
     

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperEnd.ascx") %>

 