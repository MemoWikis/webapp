<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GameReady>" %>
<%@ Import Namespace="System.Globalization" %>

<div class="row">
    <div class="col-sm-6" style="font-size: 17px; vertical-align: bottom; line-height: 48px;">
        <% if(Model.IsCreatorOfGame){ %>
            Du bist der Ersteller <i class="fa fa-smile-o"></i>
        <% }else if(Model.IsInGame){ %>
            Du nimmst an diesem Spiel Teil <i class="fa fa-smile-o"></i>
        <% } else { %>
            Teilnehmen
        <% } %>
        
    </div>
    <div class="col-sm-6 text-right text-left-sm" style="text-align: right; font-size: 30px;">
        <% foreach(var player in Model.Players){ %>
            <i class="fa fa-user show-tooltip" data-original-title="<%= player.Name %>"></i>
        <% } %>
    </div>
</div>

<div class="row">
    <div class="col-xs-12" style="padding-top: 30px; font-size: 50px;">
        <span style="font-size: 22px; margin-right: 10px;">Start in </span>
        <span style="font-weight: bolder"
            data-willStartIn="<%= Model.WillStartAt.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture) %>"></span>
    </div>            
</div>