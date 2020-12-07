<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AddToWishknowledge>" %>

<% if (Model.IsHeader) { %> 
    <div class="iAdded pinButton <%= Model.IsWishknowledge ? "" : "hide2" %>">
        <i class="fa fa-heart show-tooltip" title="Aus deinem Wunschwissen entfernen"></i>
        <div class="Text">Hinzugefügt</div>
    </div>
    <div class="iAddedNot show-tooltip pinButton <%= Model.IsWishknowledge ? "hide2" : "" %>" title="Zu deinem Wunschwissen hinzuzufügen">
        <i class="fa fa-heart-o"></i>
        <div class="Text">Hinzufügen</div>
    </div>
    <div class="iAddSpinner hide2"><i class="fa fa-spinner fa-spin"></i></div>
<%} else { %>
    <span class="iAdded <%= Model.IsWishknowledge ? "" : "hide2" %>"><i class="fa fa-heart show-tooltip" style="color: #FF001F;" title="Aus deinem Wunschwissen entfernen"></i></span>
    <span class="iAddedNot show-tooltip <%= Model.IsWishknowledge ? "hide2" : "" %>" title="Zu deinem Wunschwissen hinzuzufügen"><i class="fa fa-heart-o" style="color:#FF001F;"></i>
        <% if (Model.DisplayAdd){%> 
            <span class="Text">Hinzufügen</span> 
        <% }%>
    </span>
    <span class="iAddSpinner hide2"><i class="fa fa-spinner fa-spin" style="color: #FF001F;"></i></span>
<%}%>