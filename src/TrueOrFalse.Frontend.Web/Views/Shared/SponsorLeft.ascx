<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SponsorModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% if (!Model.IsAdFree){ %>
    <div class="SponsorLeftWrapper">
        <div class="SponsorLeftLogo">
            <a href="<%= Model.Sponsor.SponsorUrl %>" target="_blank" rel="nofollow"><img src="<%= Model.Sponsor.ImageUrl %>"/></a>
        </div>
        <%= Model.Sponsor.TextBeforeLink %> <a href="<%= Model.Sponsor.SponsorUrl%>" rel="nofollow"><%= Model.Sponsor.LinkText %></a> <%= Model.Sponsor.TextAfterLink %>
    </div>
<% } %>
