<%@ Control Language="C#" AutoEventWireup="true"  Inherits="System.Web.Mvc.ViewUserControl<VideoWidgetModel>" %>

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperStart.ascx") %>


    <div v-if="canBeEdited">
        Platzhalter für Videowidget zum Lernset: <%= Model.SetId %>
    </div>
    <div v-else>
        <content-module-widget 
            :widget-id="widgetId" 
            widget-type="video" 
        src="<%= Settings.CanonicalHost %>/views/widgets/w.js" 
            data-t="setVideo" 
            data-id="<%= Model.SetId %>" 
            data-width="100%">
        </content-module-widget>     
    </div>

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperEnd.ascx") %>

 