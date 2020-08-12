<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AddToWishknowledge>" %>

<a class="HeartToAddButton noTextdecoration iconContainer" href="#/" data-allowed-type="Pin_AnswerQuestion" rel="nofollow">
    <div class="iAdded <%= Model.IsWishknowledge ? "" : "hide2" %>">
        <i class="fa fa-heart show-tooltip" title="Befindet sich in deinem Wunschwissen. Klicken zum Entfernen."></i>
    </div>
    <div class="iAddedNot show-tooltip <%= Model.IsWishknowledge ? "hide2" : "" %>" title="Zu deinem Wunschwissen hinzufügen" style="background-color: #fff; border:0;">
        <i class="far fa-heart" style="color:#FF001F;"></i>
    </div>
    <div class="iAddSpinner hide2">
        <i class="fa fa-spinner fa-spin" style="color:#FF001F;"></i>
    </div>
</a>