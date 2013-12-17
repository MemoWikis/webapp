<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div class="rowBase question-row" style="position: relative;" data-questionId="<%= Model.QuestionId %>" data-userIsOwner="<%= Model.IsOwner? "true" : "false" %>">
    <div class="column-1"  >
        <div image-container="true">
            <img src="<%= Model.ImageUrl%>" width="105" style="padding-bottom: 5px;">
            <label class="checkbox selectQuestion" style="font-size: 12px; position: absolute; left:5px; bottom: -5px; width: 105px; color: black; background-color: white; opacity:0.7; display: none;">
                <input type="checkbox"> auswählen
            </label>
        </div>
    </div>

    <div class="column-2" style="height: 95px; position: relative;">

        <div class="pull-right" style="margin-top: 1px; margin-right: 4px; border-radius: 6px; border: 1px solid beige; background-color: beige; padding:4px;">
            <span class="show-tooltip" title="Insgesamt <%=Model.AnswersAllCount%>x beantwortet."><%=Model.AnswersAllCount%>x </span>
            <span class="pieTotals" data-percentage="<%= Model.AnswersAllPercentageTrue %>-<%= Model.AnswersAllPercentageFalse %>"></span>
            <span class="show-tooltip" title="Von Dir <%=Model.AnswerMeCount%>x beantwortet.">(ich <%= Model.AnswerMeCount%>x </span>
            <span class="pieTotals" data-percentage="<%= Model.AnswerMePercentageTrue %>-<%= Model.AnswerMePercentageFalse %>"></span>)
        </div>
        
        <div style="font-weight:normal; font-size:large;">
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

    <div class="column-3" style="line-height: 15px; font-size: 90%;" data-questionId="<%= Model.QuestionId %>">

        <div style="padding-bottom:2px; padding-top:5px; width: 150px; <% if(Model.RelevancePersonal == -1){ %>display:none<% } %>" class="sliderContainer">
            <div class="slider ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all" style="width: 90px; margin-left:5px; float: left;" data-questionId="<%= Model.QuestionId %>"> 
                <div class="ui-slider-range ui-widget-header ui-slider-range-min"></div>
                <a class="ui-slider-handle ui-state-default ui-corner-all" href="#"></a>
            </div>
            <div style="float:left; margin-top: -2px" class="sliderAnotation">
                <a href="#"><span class="sliderValue"><%= Model.RelevancePersonal %></span></a> <a href="#" class="removeRelevance"><i class="fa fa-minus"></i></a>
            </div>
        </div>
        
        <a href="#" class="addRelevance" style="<% if(Model.RelevancePersonal != -1){ %>display:none;<% } %>" ><i class="fa fa-plus-circle "></i> merken</a>
        
        <div style="clear:both;"></div>

        <%if(Model.TotalRelevancePersonalEntries != "0"){ %>
            <div style="margin-top: 2px;">
                <span class="totalRelevanceEntries"><%= Model.TotalRelevancePersonalEntries %></span> x 
                <a href="">gemerkt (&#216;   <span class="totalRelevanceAvg"><%= Model.TotalRelevancePersonalAvg %></span></a> <span class="piePersonalRelevanceTotal" data-avg="<%= Model.TotalRelevancePersonalAvg  %>"></span> )
            </div>
        <%} %>
        <%if(Model.TotalQualityEntries != "0"){ %>
            <div>
                <%= Model.TotalQualityEntries%> x <a href="">Qualität (&#216; <%= Model.TotalQualityAvg%>)</a>
            </div>
        <%} %>
        
        <div style="position: absolute; bottom: 3px;">
            von <a href="<%= Model.UserLink(Url)  %>" class="userPopover" rel="popover" data-creater-id="<%= Model.CreatorId %>" data-original-title="<%=Model.CreatorName %>">
                    <%=Model.CreatorName %>
                </a>

            <% if (Model.IsOwner){%>
                <div style="position: relative; top: -1px; display: inline-block">
                    <a data-toggle="modal" data-questionId="<%= Model.QuestionId %>" href="#modalDelete"><img src="/Images/delete.png"/> </a>
                    <a href="<%= Url.Action(Links.EditQuestion, Links.EditQuestionController, new {id = Model.QuestionId}) %>">
                        <img src="/Images/edit.png"/> 
                    </a>
                </div>
            <% } %>
        </div>

    </div>
</div>