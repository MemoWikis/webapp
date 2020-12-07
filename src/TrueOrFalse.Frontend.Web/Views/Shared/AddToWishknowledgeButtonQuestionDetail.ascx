<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AddToWishknowledge>" %>

<a class="HeartToAddButton noTextdecoration iconContainer questionBodyPin" href="#/" data-allowed-type="Pin_AnswerQuestion" rel="nofollow">
    <div class="iAdded <%= Model.IsWishknowledge ? "" : "hide2" %>">
        <i class="fa fa-heart show-tooltip" title="Frage befindet sich in deinem Wunschwissen. Klicken zum Entfernen." style="color:#FF001F;"></i>
        <div class="pinLabel hideOnHover">
            Hinzugefügt
        </div>
        <div class="pinLabel showOnHover">
            Entfernen
        </div>
    </div>
    <div class="iAddedNot show-tooltip <%= Model.IsWishknowledge ? "hide2" : "" %>" title="Frage zu deinem Wunschwissen hinzufügen" style="background: none; border:0;">
        <i class="far fa-heart" style="color:#FF001F;"></i>
        <div class="pinLabel">Hinzufügen</div>
    </div>
    <div class="iAddSpinner hide2">
        <i class="fa fa-spinner fa-spin" style="color:#FF001F;"></i>
    </div>
</a>