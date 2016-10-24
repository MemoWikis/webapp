<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SetRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="rowBase set-row" data-setId="<%= Model.Id %>" style="position: relative">
    <div class="column-Image">
        <div class="ImageContainer">
            <%= Model.ImageFrontendData.RenderHtmlImageBasis(128, true, ImageType.QuestionSet) %>
        </div>
    </div>
    
    <div class="column-MainContent">
        <div class="MainContentUpper">
            <div class="TitleText">
                <div class="Pin" data-set-id="<%= Model.Id %>">
                    <a href="#" class="noTextdecoration" style="font-size: 22px; height: 10px;">
                        <i class="fa fa-heart show-tooltip iAdded <%= Model.IsInWishknowledge ? "" : "hide2" %>" style="color:#b13a48;" title="Aus deinem Wunschwissen entfernen"></i>
                        <i class="fa fa-heart-o show-tooltip iAddedNot <%= Model.IsInWishknowledge ? "hide2" : "" %>" style="color:#b13a48;" title="Zu deinem Wunschwissen hinzuzufügen"></i>
                        <i class="fa fa-spinner fa-spin hide2 iAddSpinner" style="color:#b13a48;"></i>
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
            <div>
                <% foreach (var category in Model.Categories){ %>
                    <a href="<%= Links.CategoryDetail(category) %>"><span class="label label-category"><%= category.Name %></span></a>    
                <% } %>
            </div>
        
            <%= Model.DescriptionShort %>
        </div>
    </div>
    
    <div class="column-Additional">
        
        <div class="StatsGroup">
            <span class="show-tooltip totalPinsTooltip" data-original-title="Ist bei <%= Model.TotalPins%> Personen im Wunschwissen">
                <i class="fa fa-heart"  style="color:silver; display: inline;" ></i>
                <span class="totalPins NumberTimes"><%= Model.TotalPins %>x</span>                        
            </span>
            
            <div style="margin-top: 10px;" class="actions">
                <% if (Model.QuestionCount>0) { %>
                    <a style="display: block" href="<%= Links.TestSessionStartForSet(Model.Name, Model.Id) %>" rel="nofollow">
                        <i class="fa fa-play-circle">&nbsp;</i>Jetzt testen
                    </a>
                    <a style="display: block" data-allowed="logged-in"  href="/Set/StartLearningSession?setId=<%=Model.Id %>" rel="nofollow">
                        <i class="fa fa-line-chart">&nbsp;</i>Jetzt üben
                    </a>
                    <a style="display: block;" href="<%= Links.GameCreateFromSet(Model.Id) %>" class="show-tooltip" rel="nofollow" data-original-title="Spiel mit Fragen aus diesem Fragesatz starten." >
                        <i class="fa fa-gamepad" style="font-size: 15px;">&nbsp;</i>Spiel starten
                    </a>
                    <a style="display: block;" href="<%= Links.DateCreate(Model.Id) %>" class="show-tooltip" rel="nofollow" data-original-title="Termin mit diesem Fragesatz erstellen." >
                        <i class="fa fa-calendar" style="font-size: 13px;">&nbsp;</i>Termin lernen
                    </a>
                <% } %>
            </div>

        </div>
        
        <div class="StatsGroup">
            <a href="<%= Model.UserLink(Url)  %>" class="userPopover show-tooltip" rel="popover" data-creater-id="<%= Model.CreatorId %>" 
               data-original-title="Erstellt von <%=Model.CreatorName %>">
                <%= Model.CreatorName %></a>
            
            &nbsp;
            &nbsp;
            <% if (Model.IsOwner){%>
                <div style="position: relative; top: -1px; display: inline-block">
                    <a data-toggle="modal" data-SetId="<%= Model.Id %>" href="#modalDelete"><i class="fa fa-trash-o"></i></a>&nbsp;
                    <a href="<%= Links.QuestionSetEdit(Url, Model.Name, Model.Id) %>"><i class="fa fa-pencil"></i></a>
                </div>
            <% } %>
        </div>

    </div>
</div>