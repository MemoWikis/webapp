<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SidebarModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="SidebarCards" style="display: block;">
   <%if(Model.CardFooterText != null){ %>
    <div id="AutorCard">
        <div class="card-headline">
            <span>Themen-Autor</span>
        </div>
        <div class="ImageContainer" style="margin-top: 12.64px; width: 100%;">
            <img class="autor-card-image ItemImage JS-InitImage" alt="" src="<%= Model.AutorImageUrl%>"  data-append-image-link-to="ImageContainer" />
        </div>
        <div class="card-footer-text">
       <%= Model.CardFooterText%> 
        </div>
    </div>
   <%} %>
    <div id="MultipleAutorCard">
        <div class="card-headline">
            <span>Beitragende</span>
        </div>
        <div class="ImageContainer" style="margin-top: 12.64px; width: 100%;">
            <div style="display: flex; justify-content: center;">
                <img class="multiple-autor-card-image ItemImage JS-InitImage" alt="" src="/Images/no-profile-picture-85.png" data-append-image-link-to="ImageContainer" />
                <img class="multiple-autor-card-image ItemImage JS-InitImage" alt="" src="/Images/no-profile-picture-85.png" data-append-image-link-to="ImageContainer" />
            </div>
            <div style="display: flex; justify-content: center;">
                <img class="multiple-autor-card-image ItemImage JS-InitImage" alt="" src="/Images/no-profile-picture-85.png" data-append-image-link-to="ImageContainer" />
                <img class="multiple-autor-card-image ItemImage JS-InitImage" alt="" src="/Images/no-profile-picture-85.png" data-append-image-link-to="ImageContainer" />
            </div>
        </div>
        <div class="card-footer-text">
            Anzeigen                  
        </div>
    </div>
</div>
