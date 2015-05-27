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
                        (<%= Model.QuestionCount %>)
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
            <span class="show-tooltip" data-original-title="Ist bei <%= Model.TotalPins%> Personen im Wunschwissen">
                <i class="fa fa-heart"  style="color:silver; display: inline;" ></i>
                <span class="totalPins NumberTimes"><%= Model.TotalPins %>x</span>                        
            </span>
        </div>

        <div class="StatsGroup">
            <a href="<%= Model.UserLink(Url)  %>" class="userPopover" rel="popover" data-creater-id="<%= Model.CreatorId %>" 
               data-original-title="Erstellt von <%=Model.CreatorName %>">
                <%= Model.CreatorName %>
            </a>
            
            &nbsp;
            &nbsp;
            <% if (Model.IsOwner){%>
                <div style="position: relative; top: -1px; display: inline-block">
                    <a data-toggle="modal" data-SetId="<%= Model.Id %>" href="#modalDelete"><i class="fa fa-trash-o"></i></a>&nbsp;
                    <a href="<%= Links.QuestionSetEdit(Url, Model.Id) %>"><i class="fa fa-pencil"></i></a>
                </div>
            <% } %>
        </div>

    </div>
</div>