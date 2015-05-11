<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GameRowModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="rowBase game-row" style="position: relative; padding: 5px;">

    <div class="row">
        <div class="col-xs-3 header" style="padding-bottom: 5px">
            <h4 style="display: inline; margin-right: 15px;">
                Quiz (14 Fragen) 
            </h4>
        </div>
        <div class="col-xs-3 header">
            <div class="progress" >
                <div class="progress-bar <%= Model.InProgress() ? "" : "progress-bar-success" %>" aria-valuemin="0" aria-valuemax="100" 
                    style="width: 100%;">                    
                    <span style="font-size: 10px;">
                        <% if (Model.InProgress()){ %>
                            Wird gerade gespielt
                        <% }else{ %>
                            spät. in
                            <span data-countdown="<%= Model.WillStartAt.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture) %>"></span>                
                        <% } %>
                    </span>
                </div>
            </div>            
        </div>
        <div class="col-xs-6 header">
            <% if(Model.InProgress()){ %>
                <a href="<%= Links.GamePlay(Url, Model.GameId) %>" class="btn btn-primary btn-sm " style="float: right; width: 100px;">
                    <i class="fa fa-eye"></i>&nbsp; Zusehen
                </a>
            <% }else{ %>
                <a href="<%= Links.GamePlay(Url, Model.GameId) %>" class="btn btn-success btn-sm" style="float:right; width: 100px;">
                    <i class="fa fa-play-circle"></i>&nbsp; Mitspielen
                </a>        
            <% } %>
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
            <i class="fa fa-users"></i>
            Spieler:
            <a href="<%= Links.UserDetail(Url, Model.Creator) %>"><%= Model.Creator.Name %></a>
            <%  foreach(var player in Model.Players){ %>
                <a href="<%= Links.UserDetail(Url, player) %>"><%= player.Name %></a>
            <% } %>                             
        </div>        
    </div>

</div>