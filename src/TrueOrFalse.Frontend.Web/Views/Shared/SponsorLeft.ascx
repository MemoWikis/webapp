<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SponsorModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% if (!Model.IsAdFree){ %>
    <div class="SponsorLeftWrapper" style="margin-top: 50px; text-align: center;">
        <div style="margin-bottom: 20px;">
            <a href="<%= Model.SponsorUrl %>" target="_blank"><img src="<%= Model.ImageUrl %>" style="max-width: 50%; max-height: 100px;"/></a>
        </div>
        <%= Model.PresentationText %>
        <a href="<%= Model.SponsorUrl %>">Tutory</a>
    </div>
<% } %>
