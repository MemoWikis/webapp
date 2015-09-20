﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UserKnowledgeModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% if(!Model.User.ShowWishKnowledge && !Model.IsCurrentUser){ %>

    <div class="bs-callout bs-callout-info" style="margin-top: 15px;">
        <h4>Nicht öffentlich</h4>
        <p>
            <%= Model.User.Name %> hat sein Wunschwissen nicht veröffentlicht.
        </p>
                        
        <p>
            <a href="#" class="btn btn-default btn-sm" onclick="alert('Diese Funktion ist noch nicht umgesetzt.')">Bitte zeige mir dein Wunschwissen</a>    
        </p>
    </div>                    

<% }else{ %>
<div class="row">
    <div class="col-lg-10 col-xs-9 xxs-stack">                
        <div style="clear: both; padding-top: 14px; margin-bottom: 3px; border-bottom: 1px solid #ffd700;">Fragesätze (<%= Model.WishSets.Count %>):</div>
        <% if (Model.WishSets.Count > 0){ %>
            <% foreach(var set in Model.WishSets){ %>
                <div><a href="<%: Links.SetDetail(Url, set) %>"><%: set.Name %></a></div>
            <% } %>
        <% } else { %>
            <div style="padding-top: 10px; padding-bottom: 10px;">--
                <%= Model.IsCurrentUser ?  
                    "Du hast keine Fragesätze zu deinem Wunschwissen hinzugefügt" : 
                        Model.User.Name + " hat keine Fragesätze zum Wunschwissen hinzugefügt." %> --
            </div>
        <% } %>

        <div style="clear: both; padding-top: 14px; margin-bottom: 3px; border-bottom: 1px solid #afd534;">Fragen (<%= Model.WishQuestions.Count %>):</div>
        <% if (Model.WishQuestions.Count > 0){ %>
            <% foreach(var question in Model.WishQuestions){ %>
                <div>
                    <% if(question.IsPrivate()){ %> <i class="fa fa-lock show-tooltip" title="Private Frage"></i><% } %>
                    <a href="<%: Links.AnswerQuestion(Url, question) %>"><%: question.Text %></a>
                </div>
            <% } %>
        <% } else { %>
            <div style="padding-top: 10px; padding-bottom: 10px;">--
                <%= Model.IsCurrentUser ?  
                    "Du hast keine Fragen zu deinem Wunschwissen hinzugefügt" :
                    Model.User.Name + " hat keine Fragen zum Wunschwissen hinzugefügt."  %> --
            </div>
        <% } %>
    </div>
    
    <div class="col-lg-2 col-xs-3 xxs-stack">
        <% if(Model.User.ShowWishKnowledge || Model.IsCurrentUser){ %>
        <h4 style="margin-top: 20px;">Kategorien mit Wunschwissen</h4>
            <% foreach (var item in Model.WishQuestionsCategories.OrderByDescending(x => x.Questions.Count)){ %>
                <a href="<%= Links.CategoryDetail(item.Category) %>">
                    <span class="label label-category" style="margin-top: 7px;"><%: item.Category.Name %></span>
                </a> 
                <% if(Model.IsCurrentUser) { %>
                    <a href="<%= Links.QuestionWish_WithCategoryFilter(item.Category) %>" class="show-tooltip" title="<%: item.Questions.Count %> Fragen im Wunschwissen">
                <% } %>
                    <span><%: item.Questions.Count %>x</span> 
                <% if(Model.IsCurrentUser) { %>
                    </a>
                <% } %>
                <br />
            <% } %>
        <% } %>           
    </div>

</div>
<% } %>