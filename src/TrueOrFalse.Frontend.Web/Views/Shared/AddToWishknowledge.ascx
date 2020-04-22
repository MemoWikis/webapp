<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AddToWishknowledge>" %>

<span class="iAdded <%= Model.IsWishknowledge ? "" : "hide2" %>"><i class="fa fa-heart show-tooltip" style="color: #FF001F;" title="Aus deinem Wunschwissen entfernen"></i></span>
<span class="iAddedNot show-tooltip <%= Model.IsWishknowledge ? "hide2" : "" %>" title="Zu deinem Wunschwissen hinzuzufügen"><i class="fa fa-heart-o" style="color:#FF001F;"></i>
    <span class="Text">
        <% if (Model.DisplayAdd)
           {
                %>Hinzufügen <% }
            else
            { %>
                &nbsp;                        
         <% } %> 
    </span>
</span>
<span class="iAddSpinner hide2"><i class="fa fa-spinner fa-spin" style="color: #FF001F;"></i></span>