<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SetRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="rowBase set-row" data-setId="<%= Model.Id %>" style="position: relative">
    <div class="column-Image">
        <div class="ImageContainer">
            <%= Model.ImageFrontendData.RenderHtmlImageBasis(128, true, ImageType.QuestionSet, linkToItem: Model.DetailLink(Url)) %>
        </div>
    </div>
    
    <div class="column-MainContent">
        <div class="MainContentUpper">
            <div class="TitleText">
                <div class="Pin" data-set-id="<%= Model.Id %>">
                    <a href="#" class="noTextdecoration" style="font-size: 22px; height: 10px;">
                        <%= Html.Partial("AddToWishknowledge", new AddToWishknowledge(Model.IsInWishknowledge)) %>
                    </a>
                </div>
                <div>
                    <% if(Model.QuestionCount != 0){ %>
                        <span class="label label-question show-tooltip" title="" data-placement="top" data-original-title="<%= Model.QuestionCount %> Fragen im Fragesatz">
                            <%= Model.QuestionCount %>
                        </span>
                    <% }else{ %>
                        <span style="color: darkgray">(0)</span>
                    <% } %>            
                    <a href="<%= Model.DetailLink(Url) %>"><%= Model.Name %></a>
                </div>
            </div>
            <div style="margin-bottom: 5px;">
                <%= Model.DescriptionShort %>
            </div>
            <div style="margin-bottom: 5px;">
                <% foreach (var category in Model.Categories){ %>
                    <a href="<%= Links.CategoryDetail(category) %>"><span class="label label-category"><%= category.Name %></span></a>    
                <% } %>
            </div>
        
        </div>
    </div>
    
    <div class="column-Additional">
        
        <div class="StatsGroup SetInfo">
            <a href="<%= Model.UserLink(Url)  %>" class="StatsRow show-tooltip" style="padding-right: 5px;" rel="popover" data-creater-id="<%= Model.CreatorId %>" 
               data-original-title="Erstellt von <%=Model.CreatorName %>">
                <%= Model.CreatorName %></a>
            
            <% if (Model.IsOwner){%>
                <div  class="StatsRow" style="position: relative; top: -1px; display: inline-block">
                    <a data-toggle="modal" data-SetId="<%= Model.Id %>" href="#modalDelete"><i class="fa fa-trash-o"></i></a>&nbsp;
                    <a href="<%= Links.QuestionSetEdit(Url, Model.Name, Model.Id) %>"><i class="fa fa-pencil"></i></a>
                </div>
            <% } %>
            
            <span class="StatsRow show-tooltip totalPinsTooltip" style="white-space: nowrap;" data-original-title="Ist bei <%= Model.TotalPins%> Personen im Wunschwissen">
                <i class="fa fa-heart greyed fa-fw"  style="display: inline;"></i>
                <span class="totalPins NumberTimes"><%= Model.TotalPins %>x</span>                        
            </span>
        </div>
        
        <div class="Divider"></div>
        
        <div class="StatsGroup ActionLinks">
            <% if (Model.QuestionCount > 0)
               { %>
                <a class="StatsRow" style="display: block" href="<%= Links.TestSessionStartForSet(Model.Name, Model.Id) %>" rel="nofollow">
                    <i class="fa fa-play-circle fa-fw">&nbsp;</i>Wissen testen
                </a>
                <a class="StatsRow" style="display: block" data-allowed="logged-in" data-allowed-type="learning-session" href="<%= Links.StartLearningSesssionForSet(Model.Id) %>" rel="nofollow">
                    <i class="fa fa-line-chart fa-fw">&nbsp;</i>Jetzt lernen
                </a>
                <a class="StatsRow" style="display: block;" href="<%= Links.GameCreateFromSet(Model.Id) %>" class="show-tooltip" rel="nofollow" data-original-title="Spiel mit Fragen aus diesem Fragesatz starten." >
                    <i class="fa fa-gamepad fa-fw" style="font-size: 15px;">&nbsp;</i>Spiel starten
                </a>
                <a class="StatsRow" style="display: block;" href="<%= Links.DateCreateForSet(Model.Id) %>" class="show-tooltip" rel="nofollow" data-original-title="Termin mit diesem Fragesatz erstellen." >
                    <i class="fa fa-calendar fa-fw" style="font-size: 13px;">&nbsp;</i>Termin lernen
                </a>
            <% }
               else { %>
                <div class="StatsRow greyed show-tooltip" data-original-title="Nicht möglich, da noch keine Fragen enthalten.">
                    <i class="fa fa-play-circle">&nbsp;</i>Wissen testen
                </div>
                <div class="StatsRow greyed show-tooltip" data-original-title="Nicht möglich, da noch keine Fragen enthalten.">
                    <i class="fa fa-line-chart">&nbsp;</i>Jetzt lernen
                </div>
                <div class="StatsRow greyed show-tooltip" data-original-title="Nicht möglich, da noch keine Fragen enthalten.">
                    <i class="fa fa-gamepad" style="font-size: 15px;">&nbsp;</i>Spiel starten
                </div>
                <div class="StatsRow greyed show-tooltip" data-original-title="Nicht möglich, da noch keine Fragen enthalten.">
                    <i class="fa fa-calendar" style="font-size: 13px;">&nbsp;</i>Termin lernen
                </div>
               <% }%>
        </div>
    </div>
</div>