<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SidebarModel>" %><%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="SidebarCards" style="display: block;">

<%if(Model.Authors.Count == 1){

    var author = Model.Authors.First();

%>
    <div id="AutorCard" style="padding-top:0.1px;">
        <div class="ImageContainer" style="width: 100%; padding-left:0.5px; border-right: #979797 solid 0.5px;">
            <div class="card-image-large" style="background:url(<%= author.ImageUrl %>) center;"></div>
        </div>
        <div class="card-title">
            <span>Autorenschaft</span>
        </div>
        <% if(Model.Creator != null){  %>
            <div class="card-link">
                <a  href="<%= Links.UserDetail(author.User) %>">
                    <%= author.Name %> 
                </a>
            </div>
        <% } %>
        <div class="autor-card-footer-bar">               
            <div class="show-tooltip" <% if(!Model.IsCurrentUser){%>title="<%if(Model.DoIFollow){ %>Du folgst <%= author.Name %> und nimmst an ihren/seinen Aktivitäten teil.<%}else{ %>Folge <%= author.Name %>, um an ihren/seinen Aktivitäten teilzuhaben.<%} %>" <%} %>>
                <i class="fa fa-user"></i>
                <span class="footer-bar-text"><%= Model.Reputation.ForUsersFollowingMe %></span> 
            </div>
            <div class="show-tooltip" title='"<%= Model.Reputation.TotalReputation%>" ist eine stolze Zahl! Reputationspunkte zeigen, wieviel  <%= Model.AutorCardLinkText%> für memucho getan hat.'>
                <i style="color:#b13a48;" class="fa fa-heart"></i>
                <span class="footer-bar-text"> <%=Model.Reputation.TotalReputation %></span>
            </div>
            <div class="show-tooltip"  <% if(!Model.IsCurrentUser){%>title="<%= Model.AutorCardLinkText%> hat ihr/sein Wunschwissen<% if(Model.Creator.ShowWishKnowledge){ %> veröffentlicht und <%= Model.AmountWishCountQuestions %> Fragen gesammelt. Alles klar soweit?<%}else{ %> nicht veröffentlicht.<%} %>"<%} %>>
                <span class="fa fa-pencil" ></span>
                <span class="footer-bar-text"><%= Model.AmountWishCountQuestions %></span>
            </div>
        </div>
    </div>
    <%} 

if (Model.Authors.Count != 1){%>
    <div id="MultipleAutorCard">
        <div class="card-title">
            <span>Beitragende</span>
        </div>
        <div class="autor-container">
        <% 
            foreach (var author in Model.Authors.Take(3))  { %>
                <div class="single-autor-container">
                    <div class="multiple-autor-card-image">
                    <img class="ItemImage JS-InitImage" alt="" src="<%= author.ImageUrl %>" data-append-image-link-to="ImageContainer" />
                    </div>
                    <a style="font-size: 14px;" href="<%= Links.UserDetail(author.User)%>" class="card-link"><%= author.Name %></a>
                </div>
        <%  } %>
        </div>
        
        <%if(Model.Authors.Count > 3){ %>
            <div id="AllAutorsContainer" class="card-link cursor-hand">
                <span style="align-self:center;">weitere Beitragende <i id="ExtendAngle" style="color:#979797;" class="fa fa-angle-right"></i></span> 
            </div>
            <div id="AllAutorsList" style="display:none; margin-bottom:19px" class="card-link">
                <% foreach(var author in Model.Authors.Skip(3))  {%>
                    <a href="<%= Links.UserDetail(author.User)%>" style="display:unset; font-size: 14px;" class="card-link"> <%= author.Name %></a>
                <%} %>
            </div>
        <% } %>
    </div>
<%} %>


<%if (Model.CategorySuggestionName != null){ %>
   <div id="CategorySuggestionCard" style="padding-top:0.2px;">
      <div class="ImageContainer" style="width: 100%;">
          <div class="card-image-large" style="background:url(<%= Model.CategorySuggestionImageUrl%>) center;"></div>
       </div>
       <div class="card-title">
           <span>Themen-Vorschlag</span>
       </div>
       <div class="card-link" style="margin-bottom:25px;">
          <a class="show-tooltip"  title="Zur Themenseite  <%= Model.CategorySuggestionName %>" href="<%= Model.CategorySuggestionUrl %>">
              <%= Model.CategorySuggestionName %> 
          </a>
       </div>
   </div>
<% } %>

<div id="EduPartnerCard">
 <% if (Model.SponsorModel != null && !Model.SponsorModel.IsAdFree) { %>
    <% Html.RenderPartial("SidebarSponsor", Model.SponsorModel); %>
 <% } %>
</div>

<div id="CreateQuestionCard">
    <div class="ImageContainer" style="width:100%;">
        <div class="card-image-large" style="background:url(/Images/no-question-533.png) center; "></div>
    </div>
    <div class="card-title">
        <span>Frage erstellen</span>
    </div>
    <div class="card-link" style="margin-bottom:33px;">
       <a href="<%= Url.Action("Create", "EditQuestion") %>">Was willst du wissen?</a>                 
    </div>
</div>

 <div id="CreateCategoryCard">
    <div class="card-title">
        <span>Thema erstellen</span>
    </div>
    <p style="margin-top: 21px; border-bottom:solid 1px #d6d6d6; padding-bottom: 23px;">Lass memucho wachsen, durch eine neue Themenseite.</p>
    <i class="fa fa-circle"></i>
    <i class="fa fa-plus-square"></i>
    <div class="card-link">
       <a href="<%= Url.Action("Create", "EditCategory") %>">Zum Erstell-Tool</a>                 
    </div>
 </div>
</div>
