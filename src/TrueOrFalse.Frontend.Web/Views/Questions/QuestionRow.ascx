﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div class="rowBase question-row" style="position: relative;" data-questionId="<%= Model.QuestionId %>" data-userIsOwner="<%= Model.IsOwner? "true" : "false" %>">
    <div class="column-Image">
        <div class="image-container">
            <img src="<%= Model.ImageUrl%>">
            <label class="checkbox selectQuestion">
                <input type="checkbox"> auswählen
            </label>
        </div>
    </div>

    <div class="column-MainContent">

        <div class="ShortStats">
            <span class="show-tooltip" title="Insgesamt <%=Model.AnswersAllCount%>x beantwortet."><%=Model.AnswersAllCount%>x </span>
            <span class="pieTotals" data-percentage="<%= Model.AnswersAllPercentageTrue %>-<%= Model.AnswersAllPercentageFalse %>"></span>
            <span class="show-tooltip" title="Von Dir <%=Model.AnswerMeCount%>x beantwortet.">(ich <%= Model.AnswerMeCount%>x </span>
            <span class="pieTotals" data-percentage="<%= Model.AnswerMePercentageTrue %>-<%= Model.AnswerMePercentageFalse %>"></span>)
        </div>
        
        <div class="QuestionText">
            <% if(Model.IsPrivate){ %> <i class="fa fa-lock show-tooltip" title="Private Frage"></i><% } %>
            <a href="<%= Model.AnswerQuestionLink(Url) %>"><%=Model.QuestionShort%></a>
        </div>   
        <div>
            <% foreach (var category in Model.Categories){ %>
                <a href="<%= Links.CategoryDetail(Url, category) %>"><span class="label label-category"><%= category.Name %></span></a>
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

    <div class="column-Additional col-xs-10 col-sm-3 col-lg-2" data-questionId="<%= Model.QuestionId %>" style="height: 100%;" >
        <div class="StatsGroup Slider">
            
        </div>
        <div class="StatsGroup NumberTimesStats">
            <%if(Model.TotalRelevancePersonalEntries != "0"){ %>
                <div class="timesAdded" style="margin-top: 2px;">
                    <span class="show-tooltip" data-original-title="Ist bei <%= Model.TotalRelevancePersonalEntries%> Personen im Wunschwissen">
                    <i class="fa fa-heart"  style="color:silver; display: inline;" ></i>
                    <span class="totalRelevanceEntries NumberTimes"><%= Model.TotalRelevancePersonalEntries %>x</span>                        
                    </span>

                </div>
            <%} %>
            <div class="timesSeen">
                <span class="show-tooltip" data-original-title="Die Frage wurde <%= Model.Views %>x mal gesehen.">
                    <i class="fa fa-eye" style="color:darkslategray;"></i>                
                    <span class="NumberTimes"><%= Model.Views %>x</span>
                     
                </span>

            </div>
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