<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AddToWishknowledge>" %>

<div class="RoundHeartButton show-tooltip iAdded <%= Model.IsWishknowledge ? "" : "hide2" %>" title="Aus deinem Wunschwissen entfernen">
    <div class="HeartInCircle">
        <span><i class="fa fa-heart" style="color: white;"></i></span>
        <span><i class="fa fa-times hide2" style="color: #b13a48;"></i></span>
    </div>
</div>

<div class="RoundHeartButton show-tooltip iAddedNot <%= Model.IsWishknowledge ? "hide2" : "" %>" title="Zu deinem Wunschwissen hinzuzufügen">
    <div class="HeartInCircle">
        <span><i class="fa fa-heart-o" style="color:#b13a48;"></i></span>
    </div>
</div>

<div class="RoundHeartButton iAddSpinner hide2 show-tooltip" title="<%= Model.IsWishknowledge ? "Aus deinem Wunschwissen entfernen" : "Zu deinem Wunschwissen hinzuzufügen" %>">
    <div class="HeartInCircle">
        <span><i class="fa fa-spinner fa-spin" style="color:#b13a48;"></i></span>
    </div>
</div>