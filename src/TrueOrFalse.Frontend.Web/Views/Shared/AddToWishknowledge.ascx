<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AddToWishknowledge>" %>

<span class="iAdded <%= Model.IsWishknowledge ? "" : "hide2" %>"><i class="fa fa-heart show-tooltip" style="color: #b13a48;" title="Aus deinem Wunschwissen entfernen"></i></span>
<span class="iAddedNot <%= Model.IsWishknowledge ? "hide2" : "" %>"><i class="fa fa-heart-o show-tooltip" style="color:#b13a48;" title="Zu deinem Wunschwissen hinzuzufügen"></i><span class="Text" style="display: none;">Hinzufügen</span></span>
<span class="iAddSpinner hide2"><i class="fa fa-spinner fa-spin" style="color:#b13a48;"></i></span>