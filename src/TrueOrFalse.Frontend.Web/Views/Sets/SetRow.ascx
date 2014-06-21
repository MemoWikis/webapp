<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SetRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="rowBase set-row" data-questionSetId="<%= Model.Id %>" style="position: relative">
    <div class="column-Image" style="line-height: 15px; font-size: 90%;">
        <img src="<%= Model.ImageUrl%>" width="105"/>
    </div>
    
    <div class="column-MainContent" style="height: 87px;">
        
        <div class="Pin" data-set-id="<%= Model.Id %>">
            <a href="#" class="noTextdecoration" style="font-size: 22px; height: 10px;">
                <i class="fa fa-heart show-tooltip iAdded <%= Model.IsInWishknowledge ? "" : "hide2" %>" style="color:#b13a48;" title="Aus deinem Wunschwissen entfernen"></i>
                <i class="fa fa-heart-o show-tooltip iAddedNot <%= Model.IsInWishknowledge ? "hide2" : "" %>" style="color:#b13a48;" title="Zu deinem Wunschwissen hinzuzufügen"></i>
                <i class="fa fa-spinner fa-spin hide2 iAddSpinner" style="color:#b13a48;"></i>
            </a>
        </div>

        <div style="font-size:large;">
            <% if(Model.QuestionCount != 0){ %>
                (<%= Model.QuestionCount %>)
            <% }else{ %>
                <span style="color: darkgray">(0)</span>
            <% } %>            
            <a href="<%= Model.DetailLink(Url) %>"><%= Model.Name %></a>
        </div>

        <div>
            <% foreach (var category in Model.Categories){ %>
                <a href="<%= Links.CategoryDetail(Url, category) %>"><span class="label label-category"><%= category.Name %></span></a>    
            <% } %>
        </div>
        
        <%= Model.DescriptionShort %>
    </div>
    
    <div class="column-Additional">
        
        <div style="margin-top: 7px;">
            <span class="show-tooltip" data-original-title="Ist bei <%= Model.TotalRelevancePersonalEntries%> Personen im Wunschwissen">
                <i class="fa fa-heart"  style="color:silver; display: inline;" ></i>
                <span class="totalPins NumberTimes"><%= Model.TotalRelevancePersonalEntries %>x</span>                        
            </span>
        </div>

        <div style="position: absolute; bottom:6px; ">
            <a href="<%= Model.UserLink(Url)  %>" class="userPopover" rel="popover" data-creater-id="<%= Model.CreatorId %>" 
               data-original-title="Erstellt von <%=Model.CreatorName %>">
                <%= Model.CreatorName %>
            </a>
            
            <% if (Model.IsOwner){%>
                <div style="position: relative; top: -1px; display: inline-block">
                    <a data-toggle="modal" data-SetId="<%= Model.Id %>" href="#modalDelete"><i class="fa fa-trash-o"></i></a>&nbsp;
                    <a href="<%= Links.QuestionSetEdit(Url, Model.Id) %>"><i class="fa fa-pencil"></i></a>
                </div>
            <% } %>
        </div>

    </div>
</div>