<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SidebarModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="SidebarCards" style="display: block;">
   <%if(Model.AutorCardLinkText != null){ %>
    <div id="AutorCard">
        <div class="ImageContainer" style="width: 100%;">
           <div class="autor-card-image" <%if(Model.AutorImageUrl != null){ %> style="background:url(<%= Model.AutorImageUrl%>) center;" <%}else { %> style="background:url(Images/no-profile-picture-128.png) center;" <%} %>></div>
        </div>
        <div class="card-title">
            <span>Autorenschaft</span>
        </div>
        <div class="card-link">
           <a  href="<%= Links.UserDetail(Model.Creator) %>">
               <%= Model.AutorCardLinkText %> 
           </a>
        </div>
        <div class="autor-card-footer-bar">               
            <div class="show-tooltip" <% if(!Model.IsCurrentUser){%>title="<%if(Model.DoIFollow){ %>Du folgst <%= Model.AutorCardLinkText%> und nimmst an seinen Aktivitäten teil.<%}else{ %>Folge <%= Model.AutorCardLinkText%>, um an ihren/seinen Aktivitäten teilzuhaben.<%} %>" <%} %>>
              <i class="fa fa-heart"></i>
              <span class="footer-bar-text"><%= Model.Reputation.ForUsersFollowingMe %></span> 
            </div>
            <div class="show-tooltip" title='"<%= Model.Reputation.TotalReputation%>" ist eine stolze Zahl! Reputationspunkte zeigen, wieviel  <%= Model.AutorCardLinkText%> für memucho getan hat.'>
                <i style="color:#b13a48;" class="fa fa-heart"></i>
               <span class="footer-bar-text"> <%=Model.Reputation.TotalReputation %></span>
            </div>
            <div class="show-tooltip" title="<%= Model.AutorCardLinkText%> hat sein Wunschwissen veröffentlicht und 530 Fragen gesammelt. Alles klar soweit?">
                <i class="fa fa-pencil"></i>
               <span class="footer-bar-text"><%= Model.AmountWishCountQuestions %></span>
            </div>
        </div>
    </div>
   <%} %>
    <div id="MultipleAutorCard">
        <div class="card-title">
            <span>Beitragende</span>
        </div>
        <div class="autor-container">
           <% var End = 3;
              if (Model.MultipleCreatorName.Count() < 3) {
                 End = Model.MultipleCreatorName.Count();
              }
              var i = 0;
              for (i = 0; i < End; i++)  { %>
               <div class="single-autor-container">
                  <img class="multiple-autor-card-image ItemImage JS-InitImage" alt="" src="<%= Model.MultipleImageUrl[i] %>" data-append-image-link-to="ImageContainer" />
                   <a style="font-size: 14px;" href="<%= Links.UserDetail(Model.MultipleCreator[i])%>" class="card-link"><%= Model.MultipleCreatorName[i] %></a>
               </div>
           <%} %>
        </div>
        <%if(i > 2){ %>
           <div id="AllAutorsContainer" class="card-link">
               <span style="align-self:center;">Alle Beitragenden <i id="ExtendAngle" style="color:#979797;" class="fa fa-angle-right"></i></span> 
           </div>
           <div id="AllAutorsList" style="display:none; margin-bottom:19px" class="card-link">
               <% for (i = 3; i < Model.MultipleCreatorName.Count(); i++)  {
                       if(i == Model.MultipleCreatorName.Count() - 1) { %>
                           <a href="<%= Links.UserDetail(Model.MultipleCreator[i])%>" style="font-size: 14px;" class="card-link"> <%= Model.MultipleCreatorName[i] %></a>
                       <%} else if( i == 3) { %>
                           <a href="<%= Links.UserDetail(Model.MultipleCreator[i])%>" style="font-size: 14px;" class="card-link"><%= Model.MultipleCreatorName[i] %>,</a>
                       <%} else { %>
                           <a href="<%= Links.UserDetail(Model.MultipleCreator[i])%>" style="font-size: 14px;" class="card-link"> <%= Model.MultipleCreatorName[i] %>,</a>
                       <%} %>
                <%} %>
           </div>
        <%} %>
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
