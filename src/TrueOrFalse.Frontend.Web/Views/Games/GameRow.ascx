<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GameRowModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="rowBase game-row" style="position: relative;">
    
    <div class="progress">
      <div class="progress-bar progress-bar-striped active" role="progressbar" aria-valuenow="45" aria-valuemin="0" aria-valuemax="100" style="width: 45%">
        <span class="sr-only">45% Complete</span>
      </div>
    </div>
    
    <div class="progress">
      <div class="progress-bar progress-bar-success progress-bar-striped" role="progressbar" aria-valuenow="40" aria-valuemin="0" aria-valuemax="100" style="width: 40%">
        <span class="sr-only">40% Complete (success)</span>
      </div>
    </div>

    <div class="row">
        <div class="col-xs-12 header" style="padding-bottom: 5px">
            <h4 style="display: inline; margin-right: 15px;">
                Quiz (14 Fragen) 
            </h4>
            
            Startet in
            <span style="background-color: yellowgreen"
                data-countdown="<%= Model.WillStartAt.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture) %>"></span>
            


        </div>        
    </div>
    
    <div class="row">
        <div class="col-xs-12">
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
        <div class="col-xs-12">
            Spieler:
            <a href="<%= Links.UserDetail(Url, Model.Creator) %>"><%= Model.Creator.Name %></a>
            <%  foreach(var player in Model.Players){ %>
                <a href="<%= Links.UserDetail(Url, player) %>"><%= player.Name %></a>
            <% } %>                             
        </div>        
    </div>

    <div class="row">
        <div class="col-xs-12">
            <% if(Model.Status == GameStatus.InProgress){ %>
                <a href="<%= Links.GamePlay(Url, Model.GameId) %>" class="btn btn-info btn-xs" style="float:right">
                    Zusehen
                </a>
            <% }else{ %>
                <a href="<%= Links.GamePlay(Url, Model.GameId) %>" class="btn btn-success btn-xs" style="float:right">
                    Mitspielen
                </a>        
            <% } %>
        </div>        
    </div>

</div>