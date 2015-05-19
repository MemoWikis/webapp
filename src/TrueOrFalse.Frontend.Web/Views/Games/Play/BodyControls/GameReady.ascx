<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GameReady>" %>
<%@ Import Namespace="System.Globalization" %>

<div class="row">
    <div class="col-xs-6" style="font-size: 17px; vertical-align: bottom; line-height: 48px;">
        Du nimmst an diesem Spiel Teil <i class="fa fa-smile-o"></i>
    </div>
    <div class="col-xs-6" style="text-align: right; font-size: 30px;">
        <% foreach(var player in Model.Players){ %>
            <i class="fa fa-user show-tooltip" data-original-title="<%= player.Name %>"></i>
        <% } %>
    </div>
</div>    

<div class="row">
    <div class="col-xs-12" style="padding-top: 60px; font-size: 50px;">
        Start in 
        <span data-willStartIn="<%= Model.WillStartAt.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture) %>"></span>
    </div>            
</div>