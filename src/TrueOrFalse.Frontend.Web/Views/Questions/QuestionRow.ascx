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
    </div>

    <div class="column-Additional" data-questionId="<%= Model.QuestionId %>">
        
        <div class="row">
            <div class="col-sm-12 col-xs-6 col-AnswerChance">Antwortchance:</div>
            <div class="col-sm-12 col-xs-3 col-AnswerProbability"><%= Model.HistoryAndProbability.CorrectnessProbability.CPPersonal %>%</div>
            <div class="col-sm-12 col-xs-3 col-Wishknowledge">
                <div class="Pin" data-question-id="<%= Model.QuestionId %>">
                    <a href="#" class="noTextdecoration" style="font-size: 22px; height: 10px;">
                        <%= Html.Partial("AddToWishknowledgeButton", new AddToWishknowledge(Model.IsInWishknowledge)) %>
                    </a>
                </div>                
            </div>
        </div>

    </div>
</div>     