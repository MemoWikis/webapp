<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

    
<div class="row">
    <div class="col-xs-12 xxs-stack"> 
        <div class="flex-box">
           <span class=" category-set">
               <span id="Category">
            <% if (Model.Categories.Count > 0)
               { %>
               
                    <span>Thema:&nbsp;</span> <% Html.RenderPartial("CategoriesOfQuestion", Model.Question); %>
                
            <% } %>
        
            <% if (Model.SetMinis.Count > 0)
               { %>
                <% foreach (var setMini in Model.SetMinis)
                   { %>
                    <a class="margin-left" href="<%= Links.SetDetail(Url, setMini) %>"><span class="label label-set"><%: setMini.Name %></span></a>           
                <% } %>
                <% if (Model.SetCount > 5)
                   { %>        
                    <a class="margin-left" href="#" popover-all-sets-for="<%= Model.QuestionId %>">+  <%= Model.SetCount - 5 %> weitere </a>
                <% } %>

            <% } %> 
               </span>
           </span>
            <span class="right">
            <% if(Model.HistoryAndProbability.QuestionValuation.IsInWishKnowledge()) { 
                   var status = Model.HistoryAndProbability.QuestionValuation.KnowledgeStatus; %>
                    <span style="background-color: <%= status.GetColor() %>;  font-size: 13px;  padding: 2px 4px; -ms-border-radius: 5px; border-radius: 5px; width: 100%;">
                        <%= status.GetText() %>
                    </span>
            <% } %>
                <span class="show-tooltip" title="Die Frage wurde <%= Model.TotalViews %>x mal gesehen.">
                    <span><i class="fa fa-eye greyed"></i> <%= Model.TotalViews %>x </span>
                </span>
                <span class="show-tooltip margin-left" title="Die Frage wurde <%= Model.TotalRelevancePersonalEntries %>x zum Wunschwissen hinzugefügt.">
                    <i class="fa fa-heart greyed"></i> 
                    <span id="sideWishKnowledgeCount"><%= Model.TotalRelevancePersonalEntries %>x</span>
                </span> 
            </span>
        </div>
    </div>
</div>
