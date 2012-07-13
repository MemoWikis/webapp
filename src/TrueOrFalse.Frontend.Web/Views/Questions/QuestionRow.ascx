<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div class="row question-row">
    <div class="column-1" >
        <%--<div>Gemerkt: <img src="/Images/star.png"/> </div>--%>
        <%--<img src="/Images/group_40x40.png" style="width: 21px; display: inline; line-height: 30px; margin-top:-7px;"/>--%>
        
        
        <div style="padding-bottom:2px; padding-top:5px; height:  width: 160px;">
            <div id="slider" class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all" style="width: 100px; margin-left:5px; float: left; "> 
                <div class="ui-slider-range ui-widget-header ui-slider-range-min"></div>
                <a class="ui-slider-handle ui-state-default ui-corner-all" href="#"></a>
            </div>
            &nbsp;<a href="#">10</a>
        </div>
        
        <div><a href="">Gemerkt: 
            <%if(Model.TotalRelevancePersonalEntries != "0"){ %>
                <%= Model.TotalRelevancePersonalEntries %> (&#216;   <%= Model.TotalRelevancePersonalAvg %>)
            <%}else{ %> ------------- <% } %></a>
        </div>
        <div><a href="">Qualtität: 
            <%if(Model.TotalQualityEntries != "0"){ %>
                <%= Model.TotalQualityEntries%> (&#216;   <%= Model.TotalQualityAvg%>)
            <%}else{ %> ------------ <% } %></a>
        </div>        
    </div>

    <div class="column-2">
        <div style="height: 20px;">
            <% if (Model.IsOwner){%>
            <a data-toggle="modal" data-questionId="<%= Model.QuestionId %>" href="#modalDelete"><img src="/Images/delete.png"/> </a>

            <a href="<%= Url.Action(Links.EditQuestion, Links.EditQuestionController, new {id = Model.QuestionId}) %>">
                <img src="/Images/edit.png"/> 
            </a>
            <% } %>
            
            von <a href="<%= Model.AnswerQuestionLink(Url)  %>"><%=Model.CreatorName %></a>
        </div>
        <div style="height: 100%; font-weight:normal; font-size:large;">
            <a href="<%= Model.AnswerQuestionLink(Url) %>"><%=Model.QuestionShort%></a>
        </div>   
    </div>

    <div class="column-3">
       <div class="row header">
           <div class="column answersTotal">Antworten</div>
           <div class="column percentageBar"><span style="color: green" >&nbsp;richt</span>/<span style="color: red">falsch</span></div>
       </div>

       <div class="row">
           <div class="column answersTotal">Alle: <%=Model.AnswerCountTotal%></div>
           <div class="column percentageBar">
               <span class="pieTotals" data-percentage="<%= Model.AnswerPercentageTrue %>-<%= Model.AnswerPercentageFalse %>"></span>
               <span class="tristateHistory" data-history=""></span>
           </div>
       </div>

       <div class="row">
           <div class="column answersTotal">Ich: <%= Model.AnswerCountMe%></div>
           <div class="column percentageBar">
               <span class="pieTotals" data-percentage="10-15"></span>
               <span class="tristateHistory" data-history=""></span>
           </div>
       </div>

    </div>
</div>