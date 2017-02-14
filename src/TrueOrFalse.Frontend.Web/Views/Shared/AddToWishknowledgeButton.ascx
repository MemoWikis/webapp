<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AddToWishknowledge>" %>

<a class="Pin HeartToAddButton noTextdecoration" href="#" data-allowed="logged-in" data-allowed-type="Pin_AnswerQuestion" 
    <%= Model.QuestionId > 0 ? "data-question-id='" + Model.QuestionId + "'" : ""%>>
    <div class="iAdded <%= Model.IsWishknowledge ? "" : "hide2" %>">
        <i class="fa fa-heart show-tooltip" title="Befindet sich in deinem Wunschwissen. Klicken zum Entfernen."></i>
    </div>
    <div class="iAddedNot show-tooltip <%= Model.IsWishknowledge ? "hide2" : "" %>" title="Zu deinem Wunschwissen hinzufügen">
        <i class="fa fa-heart"></i>
        <span> Hinzufügen</span>
    </div>
    <div class="iAddSpinner hide2">
        <i class="fa fa-spinner fa-spin" style="color:#b13a48;"></i>
    </div>
</a>