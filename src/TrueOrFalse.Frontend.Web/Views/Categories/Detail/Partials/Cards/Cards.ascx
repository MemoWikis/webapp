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

    <modal-cards-settings inline-template orig-markdown="<%: Model.Markdown %>">
        <div class="modal fade" id="modalContentModuleSettings" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div style="margin:20px">
                        <span>{{_cardSettings.TemplateName}} bearbeiten</span>
                        <br/>
                        <select v-model="selectedCardOrientation">
                            <option>Landscape</option>
                            <option>Portrait</option>
                        </select>
                        <span>Selected: {{selectedCardOrientation}}</span>
                        <br/>
                        <br/>
                        <button @click="showNewMarkdown()">Show New Markdown</button>
                    </div>                    
                </div>
            </div>
        </div>
    </modal-cards-settings>

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperEnd.ascx") %>


  