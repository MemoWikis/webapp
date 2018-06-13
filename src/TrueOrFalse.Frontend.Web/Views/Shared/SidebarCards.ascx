<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SidebarModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="SidebarCards" style="display: block;">
   <%if(Model.CardFooterText != null){ %>
    <div id="AutorCard">
        <div class="card-headline">
            <span>Themen-Autoren</span>
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
               <!-- <img class="multiple-autor-card-image ItemImage JS-InitImage" alt="" src="<%//= Model.MultipleImageUrl[0] %>" data-append-image-link-to="ImageContainer" />
                <img class="multiple-autor-card-image ItemImage JS-InitImage" alt="" src="<%//= Model.MultipleImageUrl[1] %>" data-append-image-link-to="ImageContainer" />
           <%//= Model.MultipleCreatorName[0] %>
                </div>
            <div style="display: flex; justify-content: center;">
                <img class="multiple-autor-card-image ItemImage JS-InitImage" alt="" src="<%//= Model.MultipleImageUrl[2] %>" data-append-image-link-to="ImageContainer" />
                <img class="multiple-autor-card-image ItemImage JS-InitImage" alt="" src="<%//= Model.MultipleImageUrl[3] %>" data-append-image-link-to="ImageContainer" />
            </div> -->
        </div>
        <div class="card-footer-text">
            Anzeigen                  
        </div>
    </div>
    <div id="CreateCategoryCard">
        <div class="card-headline">
            <span>Thema erstellen</span>
        </div>
        <p style="margin-top: 21px; border-bottom:solid 1px #d6d6d6; padding-bottom: 23px;">Lass memucho wachsen, durch eine neue Themenseite.</p>
        <i class="fa fa-circle"></i>
        <i class="fa fa-plus-square"></i>
        <div class="card-footer-text">
           <a href="<%= Url.Action("Create", "EditCategory") %>">Zum Erstell-Tool</a>                 
        </div>
    </div>
</div>
