<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div class="row question-row" data-questionId="<%= Model.QuestionId %>" data-userIsOwner="<%= Model.IsOwner? "true" : "false" %>">
    <div class="column-1" style="line-height: 15px; font-size: 90%;" data-questionId="<%= Model.QuestionId %>" >
        
        <div style="padding-bottom:2px; padding-top:5px; width: 150px; <% if(Model.RelevancePersonal == -1){ %>display:none<% } %>" class="sliderContainer">
            <div class="slider ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all" style="width: 90px; margin-left:5px; float: left;" data-questionId="<%= Model.QuestionId %>"> 
                <div class="ui-slider-range ui-widget-header ui-slider-range-min"></div>
                <a class="ui-slider-handle ui-state-default ui-corner-all" href="#"></a>
            </div>
            <div style="float:left; margin-top: -2px" class="sliderAnotation">
                <a href="#"><span class="sliderValue"><%= Model.RelevancePersonal %></span></a> <a href="#" class="removeRelevance"><i class="icon-minus"></i></a>
            </div>
        </div>
        
        <a href="#" class="addRelevance" style="<% if(Model.RelevancePersonal != -1){ %>display:none;<% } %>" ><i class="icon-plus-sign "></i> merken</a>
        
        <div style="clear:both;"></div>

        <%if(Model.TotalRelevancePersonalEntries != "0"){ %>
            <div style="margin-top: 2px;">
                <span class="totalRelevanceEntries"><%= Model.TotalRelevancePersonalEntries %></span> x 
                <a href="">Merken (&#216;   <span class="totalRelevanceAvg"><%= Model.TotalRelevancePersonalAvg %></span></a> <span class="piePersonalRelevanceTotal" data-avg="<%= Model.TotalRelevancePersonalAvg  %>"></span> )
            </div>
        <%} %>
        <%if(Model.TotalQualityEntries != "0"){ %>
            <div>
                <%= Model.TotalQualityEntries%> x <a href="">Qualität (&#216; <%= Model.TotalQualityAvg%>)</a>
            </div>        
        <%} %>
        <div>
            <%= Model.Views %>
            <a href="">x gesehen</a>
        </div>  
        <div>
            <label class="checkbox selectQuestion" style="font-size: 12px">
                <input type="checkbox"> <a>auswählen</a>
            </label>
        </div>      
    </div>

    <div class="column-2" style="height: 87px; position: relative;">
        <div style="font-weight:normal; font-size:large;">
            <a href="<%= Model.AnswerQuestionLink(Url) %>"><%=Model.QuestionShort%></a>
        </div>   
        <div>
            <% foreach (var category in Model.Categories){ %>
                <a><span class="label label-category"><%= category.Name %></span></a>    
            <% } %>
        </div>
        
        <div style="overflow: no-content; height: 20px; width: 130px; position: absolute; bottom:2px;">
            <% if (Model.IsOwner){%>
                <a data-toggle="modal" data-questionId="<%= Model.QuestionId %>" href="#modalDelete"><img src="/Images/delete.png"/> </a>

                <a href="<%= Url.Action(Links.EditQuestion, Links.EditQuestionController, new {id = Model.QuestionId}) %>">
                    <img src="/Images/edit.png"/> 
                </a>
            <% } %>
        </div>
        <div style="text-align: right; width: 150px; position: absolute; bottom:0px; right: 10px;">
            von <a href="<%= Model.UserProfileLink(Url)  %>" class="userPopover" rel="popover" data-creater-id="<%= Model.CreatorId %>" data-original-title="<%=Model.CreatorName %>">
                    <%=Model.CreatorName %>
                </a>
        </div>
    </div>

    <div class="column-3">
       <div class="row header">
           <div class="column answersTotal">Antworten</div>
           <div class="column percentageBar"><span style="color: green" >&nbsp;richt</span>/<span style="color: red">falsch</span></div>
       </div>

       <div class="row">
           <div class="column answersTotal">Alle: <%=Model.AnswersAllCount%></div>
           <div class="column percentageBar">
               <span class="pieTotals" data-percentage="<%= Model.AnswersAllPercentageTrue %>-<%= Model.AnswersAllPercentageFalse %>"></span>
               <span class="tristateHistory" data-history=""></span>
           </div>
       </div>

       <div class="row">
           <div class="column answersTotal">Ich: <%= Model.AnswerMeCount%></div>
           <div class="column percentageBar">
               <span class="pieTotals" data-percentage="<%= Model.AnswerMePercentageTrue %>-<%= Model.AnswerMePercentageFalse %>"></span>
               <span class="tristateHistory" data-history=""></span>
           </div>
       </div>
        <div class="row row-viewHistory">
            Gesehen:<span class="viewsHistory">8,4,0,0,0,0,1,4,4,10,10,10,10,0,0,0,4,6,5,9,10</span>
        </div>
    </div>
</div>