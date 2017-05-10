<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div class="rowBase question-row" data-questionid="<%= Model.QuestionId %>" data-userisowner="<%= Model.IsCreator? "true" : "false" %>">
    <div class="column-Image">
        <div class="ImageContainer">
            <%= Model.ImageFrontendData.RenderHtmlImageBasis(128, /*asSquare*/ true, ImageType.Question, linkToItem: Model.AnswerQuestionLink(Url)) %>
        </div>
    </div>

    <div class="column-MainContent">
        <div class="MainContentUpper">
            <div class="QuestionText TitleText">
                <div class="Pin" data-question-id="<%= Model.QuestionId %>">
                    <a href="#" class="noTextdecoration" style="font-size: 22px; height: 10px;">
                        <%= Html.Partial("AddToWishknowledge", new AddToWishknowledge(Model.IsInWishknowledge)) %>
                    </a>
                </div>
                <% if(Model.IsPrivate){ %> <i class="fa fa-lock show-tooltip" title="Private Frage"></i><% } %>
                <a href="<%= Model.AnswerQuestionLink(Url) %>"><%=Model.QuestionShort%></a>
            </div>   
            <div>
                <% Html.RenderPartial("Category", Model.Question); %>
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
        <div class="MainContentLower">
        </div>
    </div>

    <div class="column-Additional" data-questionId="<%= Model.QuestionId %>">
        <div class="StatsGroup NumberTimesStats">
            
            <div class="timesAdded StatsRow">
                <span class="show-tooltip totalPinsTooltip" data-original-title="Ist bei <%= Model.TotalRelevancePersonalEntries%> Personen im Wunschwissen">
                    <i class="fa fa-heart greyed" style="display: inline;"></i>
                    <span class="totalPins NumberTimes"><%= Model.TotalRelevancePersonalEntries %>x</span>                        
                </span>
                    
                &nbsp;
                    
                <span class="show-tooltip" data-original-title="Frage wurde <%= Model.Views %>x gesehen">
                    <i class="fa fa-eye"></i>                
                    <span class="NumberTimes"><%= Model.Views %>x</span>
                </span>
            </div>
            <% Html.RenderPartial("HistoryAndProbability", Model.HistoryAndProbability); %>
        </div>
        <div class="StatsGroup QuestionAuthor">
            <a href="<%= Model.UserLink(Url)  %>" class="show-tooltip" rel="popover" data-creater-id="<%= Model.CreatorId %>" 
                data-original-title="Erstellt von <%=Model.CreatorName %> am <%= Model.DateCreated.ToString("D") %>">
                <%=Model.CreatorName %>
            </a>
            
            &nbsp;
            &nbsp;
            <% if (Model.IsCreator){%>
                <div style="position: relative; top: -1px; display: inline-block">
                    <a data-toggle="modal" data-questionId="<%= Model.QuestionId %>" href="#modalDeleteQuestion"><i class="fa fa-trash-o"></i></a>&nbsp;
                    <a href="<%= Links.EditQuestion(Url, Model.QuestionText, Model.QuestionId) %>"><i class="fa fa-pencil"></i></a>
                </div>
            <% } %>
        </div>
    </div>
</div>