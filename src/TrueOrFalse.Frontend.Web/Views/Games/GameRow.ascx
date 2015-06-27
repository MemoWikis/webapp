<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GameRowModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="rowBase game-row" style="position: relative; padding: 5px;" 
    data-gameId="<%= Model.GameId %>" 
    data-isCreator="<%= Model.IsCreator %>" 
    data-isPlayer="<%= Model.IsPlayer %>" >
    
    <div class="row">
        <div class="col-md-3 col-sm-3 header" style="padding-bottom: 5px">
            <h4 style="display: inline; margin-right: 15px;">
                <a href="<%= Links.GamePlay(Url, Model.GameId) %>">
                    Quiz  <span class="show-tooltip" data-original-title="<%= Model.RoundCount %> Runden" style="font-size: 13px; padding-left: 7px">(<i class="fa fa-retweet"></i> <%= Model.RoundCount %> )</span>
                </a>
            </h4>
        </div>
        <div class="col-md-3 col-sm-5 col-xs-12 header">
            <div class="progress" >
                <div class="progress-bar <%= Model.InProgress() ? "" : "progress-bar-success" %>" aria-valuemin="0" aria-valuemax="100" 
                    style="width: 100%;">                    
                    <span style="font-size: 11px;">
                        <% if (Model.InProgress()){ %>
                            Wird gerade gespielt
                            <span data-elem="currentRoundContainer" data-original-title="<%= Model.CurrentRound %> Runden von <%= Model.RoundCount %> gespielt" class="show-tooltip">
                                <i class="fa fa-retweet"></i> <span data-elem="currentRound"><%= Model.CurrentRound %></span>/<%= Model.RoundCount %>
                            </span>
                        <% }else{ %>
                            Start spät. in
                            <span data-countdown="<%= Model.WillStartAt.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture) %>"></span>
                        <% } %>
                    </span>
                </div>
            </div>
        </div>
        <div class="col-md-offset-0 col-md-3 col-sm-2 col-xs-12 header ">
            <a href="#"
                data-joinGameId="<%= Model.GameId %>" 
                data-elem="joinGame"
                style="float:right; min-width: 100px; <%= Html.CssHide(Model.InProgress() || Model.IsPlayerOrCreator) %>"
                class="btn btn-success btn-sm margin-bottom-sm linkJoin">
                <i class="fa fa-play-circle"></i>&nbsp; Mitspielen
            </a>
            <% if(!Model.InProgress() && Model.IsCreator ){ %>
                <span class="pull-right margin-bottom-sm">
                    <i class="fa fa-smile-o"></i>&nbsp;Du bist der Ersteller
                </span>
                
            <% } %>
            
            <span class="pull-right margin-bottom-sm spanYouArePlayer"
                  style="<%= Model.IsPlayer && !Model.IsCreator ? "" : "display:none" %>">
                    <i class="fa fa-smile-o"></i>&nbsp;Du bist Mitspieler
            </span>
        </div>
        <div class="col-md-3 col-sm-2 col-xs-12 header" style="text-align: right">
            
            <% if (Model.IsCreator && !Model.InProgress()){ %>
                <a href="#" class="btn btn-sm margin-bottom-sm btn-primary show-tooltip" 
                    data-elem="startGame"
                    style="<%= Html.CssHide(Model.Players.Count <= 1) %>"
                    data-original-title="Das Spiel sofort starten!">
                    <i class="fa fa-rocket"></i>
                    Start
                </a>
                <a href="#" class="btn btn-sm margin-bottom-sm btn-warning show-tooltip"
                    data-elem="cancelGame"
                    style="<%= Html.CssHide(Model.Players.Count > 1) %>"
                    data-original-title="Das Spiel ungespielt abbrechen!">
                    <i class="fa fa-times-circle"></i>  
                </a>
            <% } %>
            
            <a href="#" class="btn btn-sm margin-bottom-sm btn-warning show-tooltip"
                data-elem="leaveGame"
                style="<%= Html.CssHide(!Model.IsPlayer) %>"
                data-original-title="Nicht mehr mitspielen">
                <i class="fa fa-times-circle"></i>  
            </a>

            <a href="<%= Links.GamePlay(Url, Model.GameId) %>"
               data-elem="urlGame"
               style="margin-left: 0px"
               class="btn btn-primary btn-sm margin-bottom-sm show-tooltip"
               data-original-title="Zum Spiel">
                <% if(Model.InProgress()){ %>
                    <i class="fa fa-eye"></i>
                    <%--<% if(!Model.IsCreator){ %> &nbsp; Zusehen <% } %>--%>
                <% }else{ %>
                    <i class="fa fa-eye"></i>
                    <%--<% if(!Model.IsCreator){ %>&nbsp; Ansehen <% } %>--%>
                <% } %>
            </a>
            
        </div>            
    </div>
    
    <div class="row">
        <div class="col-md-12">
            Gespielt wird mit:
            <div style="display: inline; position: relative; top: -2px;" >
                <%  foreach(var set in Model.Sets){ %>
                    <a href="<%= Links.SetDetail(Url, set) %>">
                        <span class="label label-set"><%= set.Name %></span>
                    </a>
                <% } %>
            </div>
        </div>        
    </div>

    <div class="row">
        <div class="col-md-12 players" data-row-type="players">
            Spieler:
            <%  foreach(var player in Model.Players){ %>
                <i class="fa fa-user" data-playerUserId="<%= player.User.Id %>"></i>
                <a href="<%= Links.UserDetail(Url, player.User) %>" 
                   data-playerUserId="<%= player.User.Id %>" >
                    <%= player.User.Name %>
                </a>
            <% } %>                             
        </div>        
    </div>

</div>