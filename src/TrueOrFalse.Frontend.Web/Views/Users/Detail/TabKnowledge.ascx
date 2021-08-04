<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TabKnowledgeModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% if(!Model.User.ShowWishKnowledge && !Model.IsCurrentUser){ %>

    <div class="bs-callout bs-callout-info" style="margin-top: 15px;">
        <h4>Nicht öffentlich</h4>
        <p>
            <%= Model.User.Name %> hat ihr/sein Wunschwissen nicht veröffentlicht.
        </p>
                        
        <p>
            <a href="#" class="btn btn-default btn-sm featureNotImplemented">Bitte zeige mir dein Wunschwissen</a>    
        </p>
    </div>                    

<% }else{ %>
<div class="row">
    <div class="col-lg-12 xxs-stack">
        <div style="clear: both; padding-top: 14px; margin-bottom: 3px; border-bottom: 1px solid #afd534;">Fragen (<%= Model.WishQuestions.Count %>):</div>
        <% if (Model.WishQuestions.Count > 0){ %>
            <% foreach(var question in Model.WishQuestions.Take(100)){ %>
                <div>
                    <% if(question.IsPrivate()){ %> <i class="fa fa-lock show-tooltip" title="Private Frage"></i><% } %>
                    <a href="<%: Links.AnswerQuestion(question) %>"><%: question.Text %></a>
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
    
    <div class="col-lg-12 xxs-stack">
        <% if(Model.User.ShowWishKnowledge || Model.IsCurrentUser){ %>
        <h4 style="margin-top: 20px;">Themen mit Wunschwissen</h4>
            <% foreach (var item in Model.WishQuestionsCategories.OrderByDescending(x => x.Questions.Count).Take(42)){ %>
                <% if(Model.IsCurrentUser) { %>
                    <a href="<%= Links.CategoryDetail(item.CategoryCacheItem) %>" class="show-tooltip" title="<%: item.Questions.Count %> Fragen im Wunschwissen">
                <% } %>
                <%= item.CategoryCacheItem.Name %> <span>mit <%: item.Questions.Count %>x Frage<%: item.Questions.Count > 1 ? "n" : "" %></span> 
                <% if(Model.IsCurrentUser) { %>
                    </a>
                <% } %>
                <br />
            <% } %>
        <% } %>           
    </div>

</div>
<% } %>