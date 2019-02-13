<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<CardsModel>" %>

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperStart.ascx") %>

    <% if(!string.IsNullOrEmpty(Model.Title)) { %>
        <h4><%= Model.Title %></h4>
    <% } %>
    <div class="row Cards<%= Model.CardOrientation %>">
        <% foreach (var set in Model.Sets){ %>
            <%: Html.Partial("~/Views/Categories/Detail/Partials/SingleSet/SingleSet.ascx", new SingleSetModel(set)) %>
        <% } %>
    </div>

    <modal-cards-settings inline-template markdown="<%: Model.Markdown %>">
        <div class="modal fade" id="modalContentModuleSettings" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <span>"Cards" bearbeiten</span>
                    
                    {{markdown}}
                </div>
            </div>
        </div>
    </modal-cards-settings>

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperEnd.ascx") %>


  