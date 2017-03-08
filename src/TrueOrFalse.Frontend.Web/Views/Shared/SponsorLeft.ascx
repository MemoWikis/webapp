<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SponsorModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% if (!Model.IsAdFree){ %>
    <div class="SponsorLeftWrapper" style="margin-top: 50px; text-align: center;">
        <div style="margin-bottom: 20px;">
            <a href="<%= Model.Sponsor.SponsorUrl %>" target="_blank"><img src="<%= Model.Sponsor.ImageUrl %>" style="max-width: 50%; max-height: 100px;"/></a>
        </div>
        <%= Model.Sponsor.PresentationText %>
        <a href="<%= Model.Sponsor.SponsorUrl%>"><%= Model.Sponsor.LinkText %></a>
    </div>
<% } %>
