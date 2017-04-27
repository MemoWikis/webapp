<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GameRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="rowBase game-row" style="position: relative;" 
    data-gameId="<%= Model.GameId %>" 
    data-isCreator="<%= Model.IsCreator %>" 
    data-isPlayer="<%= Model.IsPlayer %>" >
    
    <div class="row">
        <div class="col-sm-4 header" style="padding-bottom: 5px">
            <h4 style="display: inline; margin-right: 15px;">
                <a href="<%= Links.GamePlay(Url, Model.GameId) %>">
                    Quiz <span style="font-size: 13px; padding-left: 7px">(<%= Model.RoundCount %> Runden)</span>
                </a>
            </h4>

            <div class="<%--progress--%>" style="margin-top: 6px;">
                <div class="<%--progress-bar <%= Model.InProgress() ? "" : "progress-bar-warning" %>--%>" <%--aria-valuemin="0" aria-valuemax="100"--%> style="/*width: 100%;*/">                    
                    <span style="/*font-size: 11px;*/white-space: nowrap;">
                        <% if (Model.InProgress()){ %>
                            Wird gerade gespielt: Runde
                            <span data-elem="currentRoundContainer" data-original-title="<%= Model.CurrentRound %> Runden von <%= Model.RoundCount %> gespielt" class="show-tooltip">
                                <span data-elem="currentRound"><%= Model.CurrentRound %></span>/<%= Model.RoundCount %>
                            </span>
                        <% }else{ %>
                            <span class="show-tooltip" data-original-title="Der Ersteller kann die Wartezeit abkürzen.">
                                Start spätestens in
                                <span data-remainingSeconds="<%= Model.RemainingSeconds %>"></span>
                            </span>
                        <% } %>
                    </span>
                </div>
            </div>

        </div>

        <div class="col-sm-3 header">
            <a href="#"
                data-joinGameId="<%= Model.GameId %>" 
                data-elem="joinGame"
                style="float:right; min-width: 100px; <%= Html.CssHide(Model.InProgress() || Model.IsPlayerOrCreator) %>"
                class="btn btn-success btn-sm margin-bottom-sm">
                <i class="fa fa-gamepad"></i>&nbsp; Mitspielen
            </a>
            <% if(!Model.InProgress() && Model.IsCreator ){ %>
                <span class="pull-right margin-bottom-sm">
                    Du bist der Ersteller.
                </span>
                
            <% } %>
            
            <span class="pull-right">
                <span class="margin-bottom-sm spanYouArePlayer"
                      style="<%= Model.IsPlayer && !Model.IsCreator ? "" : "display:none;" %>">
                    Du bist Mitspieler
                </span>

                <a href="#" class="btn btn-sm margin-bottom-sm btn-warning show-tooltip"
                   data-elem="leaveGame"
                   style="<%= Html.CssHide(!Model.IsPlayer) %>"
                   data-original-title="Nicht mehr mitspielen">
                    <i class="fa fa-times-circle"></i>  
                </a>
                
            </span>

        </div>
        <div class="col-sm-5 header" style="text-align: right">
            
            <% if (Model.IsCreator && !Model.InProgress()){ %>
                <a href="#" class="btn btn-sm margin-bottom-sm btn-primary show-tooltip" 
                    data-elem="startGame"
                    style="<%= Html.CssHide(Model.Players.Count <= 1) %>"
                    data-original-title="Startet das Spiel sofort, es können keine weiteren Mitspieler mitmachen.">
                    <i class="fa fa-forward">&nbsp;</i>
                    Spiel sofort starten
                </a>
                <a href="#" class="btn btn-sm margin-bottom-sm btn-warning show-tooltip"
                    data-elem="cancelGame"
                    style="<%= Html.CssHide(Model.Players.Count > 1) %>"
                    data-original-title="Das Spiel ungespielt abbrechen!">
                    <i class="fa fa-times-circle"></i>  
                </a>
            <% } %>
            
            <a href="<%= Links.GamePlay(Url, Model.GameId) %>"
               data-elem="urlGame"
               style="margin-left: 0px"
               class="btn btn-primary btn-sm margin-bottom-sm"> 
                <% if(Model.InProgress()){ %>
                    <i class="fa fa-eye">&nbsp;</i>
                <% }else{ %>
                    <i class="fa fa-eye">&nbsp;</i>
                    <%--<% if(!Model.IsCreator){ %>&nbsp; Ansehen <% } %>--%>
                <% } %>
                Zum Spiel
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
                <a href="<%= Links.UserDetail(player.User) %>" 
                   data-playerUserId="<%= player.User.Id %>" >
                    <%= player.User.Name %>
                </a>
            <% } %>                             
        </div>        
    </div>

</div>