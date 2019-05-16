<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<SpacerModel>" %>

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperStart.ascx") %>
                
    <div v-if="canBeEdited" class="spacerModule">
        <img src="/Images/ContentModuleSamples/SVG/Spacer.svg"/>
    </div>
    <div v-else> 
        <% for (var i = 0; i < Model.AmountSpaces; i++) { %>
            <div class="SpacerDiv20<%= i == 0 && Model.AddBorderTop ? " SpacerBorderTop" : "" %>">
            </div>       
        <% } %>
    </div>


<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperEnd.ascx") %>
