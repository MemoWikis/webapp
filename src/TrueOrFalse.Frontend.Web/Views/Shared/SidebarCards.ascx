<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SidebarModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="SidebarCards"> 

    <%if (Model.Authors.Count == 1){
            var author = Model.Authors.First();
    %>
        <input id="isFollow" type="hidden" value="<%=Model.DoIFollow %>"/>
        <input id="author" type="hidden" value="<%= author.User.Id%>" name="<%= author.User.Name %>" data-question-created="<%=Model.Reputation.ForQuestionsCreated %>" data-sets-created="<%= author.User.Name %>" data-question-created="<%=Model.Reputation.ForSetsCreated %>" />

    <div id="AutorCard">
        <div class="column-left">
            <div class="ImageContainer">
                <div class="card-image-large" style="background: url(<%= author.ImageUrl %>) center;"></div>
            </div>
        </div>
        <div class="column-right">
            <div class="card-title">
                <span>Erstellt von:</span>
            </div>
            <div id="card-link" class="card-link">
                <a href="<%= Links.UserDetail(author.User) %>">
                    <%= author.Name %> 
                </a>
                <i id="followIcon"class="fas follower"></i>
            </div>
            <div class="author-reputation">
                <span>Reputation:</span>
                <br />
                <span><%= author.Reputation %> Punkte (Rang <%= author.ReputationPos %>)</span>
            </div>
        </div>
        <div class="autor-card-footer-bar">
            <div class="show-tooltip" title='<%=Model.Reputation.User.Name%> hat <%= Model.Reputation.ForQuestionsCreated%> Fragen / <%=Model.Reputation.ForSetsCreated %> Lernsets erstellt.'>
                <i class="fa fa fa-question-circle"></i>
                <span class="footer-bar-text"><%=Model.Reputation.ForQuestionsCreated %></span>
            </div>
            <div class="show-tooltip" title="<% if(Model.Reputation.User.ShowWishKnowledge) {%> <%=author.Name %> hat sein/ihr Wunschwissen veröffentlicht und <%=Model.AmountWishCountQuestions %> Fragen gesammelt <% }
                                                else {%>  <%=author.Name %> hat sein Wunschwissen leider nicht veröffentlicht. <%}%> " >
                <span class="fa fa-heart"></span>
                <span class="footer-bar-text"><%= Model.AmountWishCountQuestions %></span>
            </div>
            <div id="follow-tooltip"class="show-tooltip " <% if (!Model.IsCurrentUser)
                {%>title="<%if (Model.DoIFollow)
                { %>Du folgst <%= author.Name %> und nimmst an ihren/seinen Aktivitäten teil.<%}
                else
                { %>Folge <%= author.Name %>, um an ihren/seinen Aktivitäten teilzuhaben.<%} %>"
                <%} %>>
                 <div id="follower" class="fas follower"></div>  
                 <span class="footer-bar-text"><%= Model.Reputation.ForUsersFollowingMe %></span>
            </div>
        </div>
    </div>
    <%} if (Model.Authors.Count != 1) {%>

    <div id="MultipleAutorCard">
        <div class="card-title">
            <span>Beitragende</span>
        </div>
        <div class="autor-container">
            <% 
                foreach (var author in Model.Authors.Take(3))
                { %>
            <div class="single-autor-container">
                <div class="multiple-autor-card-image">
                    <img class="ItemImage JS-InitImage" alt="" src="<%= author.ImageUrl %>" data-append-image-link-to="ImageContainer" />
                </div>
                <a href="<%= Links.UserDetail(author.User)%>" class="card-link"><%= author.Name %></a>
            </div>
            <%  } %>
        </div>

        <%if (Model.Authors.Count > 3)
            { %>
        <div id="AllAutorsContainer" class="card-link cursor-hand">
            <span style="align-self: center;">weitere Beitragende <i id="ExtendAngle" style="color: #979797;" class="fa fa-angle-right"></i></span>
        </div>
        <div id="AllAutorsList" style="display: none; margin-bottom: 19px" class="card-link">
            <% foreach (var author in Model.Authors.Skip(3))
                {%>
            <a href="<%= Links.UserDetail(author.User)%>" style="display: unset; font-size: 14px;" class="card-link"><%= author.Name %></a>
            <%} %>
        </div>
        <% } %>
    </div>
    <%} %>


    <%if (Model.SuggestionCategory != null){ %>
    <div id="CategorySuggestionCard">
        <div class="ImageContainer">
            <div class="card-image-large" style="background: url(<%= Model.CategorySuggestionImageUrl%>) center;"></div>
        </div>
        <div class="card-title">
            <span>Themen-Vorschlag</span>
        </div>
        <div class="card-link" style="margin-bottom: 25px;">
            <a class="show-tooltip" title="Zur Themenseite  <%= Model.SuggestionCategory.Name %>" href="<%= Model.CategorySuggestionUrl %>">
                <%= Model.SuggestionCategory.Name %> 
            </a>
        </div>
        <%if (Model.SuggestionCategory.GetAggregatedSetsFromMemoryCache().Count != 0)
            { %>
        <div class="category-suggestion-footer">
            <div class="set-question-count">
                <%: Model.SuggestionCategory.GetAggregatedSetsFromMemoryCache().Count  %> Lernset
                <% if (Model.SuggestionCategory.GetAggregatedSetsFromMemoryCache().Count != 1)                                                                                                     { %>s mit&nbsp;<% }
                    else{ %> mit&nbsp;<% } %>
                <%: Model.SuggestionCategory.GetAggregatedQuestionsFromMemoryCache().Count %> Frage
                <% if (Model.SuggestionCategory.GetAggregatedQuestionsFromMemoryCache().Count != 1){ %>n<% } %>
            </div>
            <div class="KnowledgeBarWrapper">
                <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(Model.SuggestionCategory)); %>
            </div>
        </div>
        <%} %>
    </div>
    <% } %>
    
    <% if(Settings.ShowAdvertisment){ %>
        <div id="EduPartnerCard">
            <% if (Model.SponsorModel != null && !Model.SponsorModel.IsAdFree) { %>
                <% Html.RenderPartial("SidebarSponsor", Model.SponsorModel); %>
            <% } %>
        </div>
    <% } %>
     <% Html.RenderPartial("~/Views/Shared/SidebarCards/CreateQuestion.ascx"); %>
</div>
