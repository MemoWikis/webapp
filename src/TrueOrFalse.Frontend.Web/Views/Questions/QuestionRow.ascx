<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div class="rowBase question-row" style="position: relative;" data-questionid="<%= Model.QuestionId %>" data-userisowner="<%= Model.IsOwner? "true" : "false" %>">
    <div class="column-Image">
        <div class="image-container">
            <img src="<%= Model.ImageUrl%>" style="position: relative;">
            <%--<label class="checkbox selectQuestion">
                <input type="checkbox"> auswählen
            </label>--%>
           
            <div class="SelectAreaUpper">
                <div id="CheckBoxIconContainer">
                    <i id="Checked-Icon" class="fa fa-check-square-o"></i>
                    <i id="Unchecked-Icon" class="fa fa-square-o"></i>
                    <div class="CheckboxText">Frage auswählen</div>
                </div>
            </div>
            <div class="SelectAreaLower">
                 <div class="HoverMessage">
                    Bildinfos
                </div>
            </div>
        </div>
        <%= Model.ImageFrontendData.RenderImageDetailModalLink("Bild- und Lizenzinfos") %>
    </div>

    <div class="column-MainContent">
        <div class="MainContentUpper">
            <div class="QuestionText">
                <div class="Pin" data-question-id="<%= Model.QuestionId %>">
                    <a href="#" class="noTextdecoration" style="font-size: 22px; height: 10px;">
                        <i class="fa fa-heart show-tooltip iAdded <%= Model.IsInWishknowledge ? "" : "hide2" %>" style="color:#b13a48;" title="Aus deinem Wunschwissen entfernen"></i>
                        <i class="fa fa-heart-o show-tooltip iAddedNot <%= Model.IsInWishknowledge ? "hide2" : "" %>" style="color:#b13a48;" title="Zu deinem Wunschwissen hinzuzufügen"></i>
                        <i class="fa fa-spinner fa-spin hide2 iAddSpinner" style="color:#b13a48;"></i>
                    </a>
                </div>

                <%= Model.QuestionId %>
                <% if(Model.IsPrivate){ %> <i class="fa fa-lock show-tooltip" title="Private Frage"></i><% } %>
                <a href="<%= Model.AnswerQuestionLink(Url) %>"><%=Model.QuestionShort%></a>
            </div>   
            <div>
                <% foreach (var category in Model.Categories){ %>
                    <a href="<%= Links.CategoryDetail(category) %>"><span class="label label-category"><%= category.Name %></span></a>
                <% } %>
            </div>
            <% if(Model.SetCount > 0){ %>
            <div style="margin-top: 3px;">
                <% foreach (var setMini in Model.SetMinis){ %>
                    <a href="<%= Links.SetDetail(Url, setMini) %>"><span class="label label-set"><%: setMini.Name %></span></a>
                <% } %>
            
                <% if (Model.SetCount > 5){ %>
                    <a href="#" popover-all-sets-for="<%= Model.QuestionId %>">+  <%= Model.SetCount -5 %> weitere</a>
                <% } %>
            </div>
            <% } %>
        </div>
        <div class="ImageLower">
        </div>
    </div>

    <div class="column-Additional col-xs-12 col-sm-3 col-lg-2" data-questionId="<%= Model.QuestionId %>" style="height: 100%;" >
        <div class="StatsGroup NumberTimesStats">
            
            <div class="timesAdded StatsRow">
                <span class="show-tooltip" data-original-title="Ist bei <%= Model.TotalRelevancePersonalEntries%> Personen im Wunschwissen">
                    <i class="fa fa-heart"  style="color:silver; display: inline;" ></i>
                    <span class="totalPins NumberTimes"><%= Model.TotalRelevancePersonalEntries %>x</span>                        
                </span>
                    
                &nbsp;
                    
                <span class="show-tooltip" data-original-title="Die Frage wurde <%= Model.Views %>x mal gesehen.">
                    <i class="fa fa-eye" style="color:darkslategray;"></i>                
                    <span class="NumberTimes"><%= Model.Views %>x</span>
                </span>
            </div>
            <% Html.RenderPartial("HistoryAndProbability", Model.HistoryAndProbability); %>
        </div>
        <div class="StatsGroup QuestionAuthor">
            <a href="<%= Model.UserLink(Url)  %>" class="userPopover show-tooltip" rel="popover" data-creater-id="<%= Model.CreatorId %>" 
                data-original-title="Erstellt von <%=Model.CreatorName %> am <%= Model.DateCreated.ToString("D") %>">
                <%=Model.CreatorName %>
            </a>
            
            &nbsp;
            <% if (Model.IsOwner){%>
                <div style="position: relative; top: -1px; display: inline-block">
                    <a data-toggle="modal" data-questionId="<%= Model.QuestionId %>" href="#modalDelete"><i class="fa fa-trash-o"></i></a>&nbsp;
                    <a href="<%= Links.EditQuestion(Url, Model.QuestionId) %>"><i class="fa fa-pencil"></i></a>
                </div>
            <% } %>
        </div>
    </div>
</div>