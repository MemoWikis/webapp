<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AddToWishknowledge>" %>

<a class="HeartToAddButton noTextdecoration" href="#" data-allowed-type="Pin_AnswerQuestion" rel="nofollow">

    <% if(!Model.IsCircleVersion) { %>
        <div class="iAdded <%= Model.IsWishknowledge ? "" : "hide2" %>">
            <i class="fa fa-heart show-tooltip" title="Befindet sich in deinem Wunschwissen. Klicken zum Entfernen."></i>
        </div>
        <div class="iAddedNot show-tooltip <%= Model.IsWishknowledge ? "hide2" : "" %>" title="Zu deinem Wunschwissen hinzufügen">
            <% if(Model.IsShortVersion) { %>
                <span class="plus">+</span>
            <% } %>
            <i class="fa fa-heart"></i>
            <% if(!Model.IsShortVersion && !Model.IsCircleVersion) { %>
                <span> Hinzufügen</span>
            <% } %>
        </div>
    <% } else { %>
        <div class="show-tooltip <%= Model.IsWishknowledge ? "hide2" : "" %>" style="height: 40px;width: 40px;border-radius: 50%;display: inline-block;border: solid 1px silver">
            <i class="fa fa-heart show-tooltip" title="Befindet sich in deinem Wunschwissen. Klicken zum Entfernen."></i>
        </div>
    <% } %>


    <div class="iAddSpinner hide2">
        <i class="fa fa-spinner fa-spin" style="color:#b13a48;"></i>
    </div>
</a>