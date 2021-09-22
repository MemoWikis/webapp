<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="CategoryFooter">

    <div class="footerToolbar">
        <div class="wishknowledge">
            <% if (Model.ShowPinButton())
               { %>
                <div class="Pin" data-category-id="<%= Model.Id %>">
                    <%= Html.Partial("AddToWishknowledgeHeartbeat", new AddToWishknowledge(Model.IsInWishknowledge, false, true)) %>
                </div>
                <% } %>

            <div id="PinLabel"><span id="CategoryFooterTotalPins"><%= Model.TotalPins%></span><span> Mal im Wunschwissen</span></div>
        </div>

        <div class="StartLearningSession">
            <a href="<%=Links.LearningSessionFooter(Model.Id, Model.Category.Name) %>" id="LearningFooterBtn" data-tab-id="LearningTab" class="btn btn-lg btn-primary footerBtn memo-button" >Jetzt Lernen</a> 
        </div>
    </div>
    <%Html.RenderPartial("~/Views/Categories/Detail/Partials/CategoryNetwork/CategoryNetwork.ascx", Model); %>
</div>

