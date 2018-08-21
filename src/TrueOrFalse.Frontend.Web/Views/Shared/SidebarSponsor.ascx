<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SponsorModel>" %>

<% if (!Model.IsAdFree){ %>
    <div class="EduPartnerLSidebarWrapper">
        <div class="EduPartnerSidebarLogo">
            <a href="<%= Model.Sponsor.SponsorUrl %>" target="_blank" rel="nofollow"><img src="<%= Model.Sponsor.ImageUrl %>" style="<%= Model.Sponsor.ImageStyleOverwrite %>" /></a>
        </div>
        <%= Model.Sponsor.TextBeforeLink %> <a href="<%= Model.Sponsor.SponsorUrl%>" rel="nofollow"><%= Model.Sponsor.LinkText %></a> <%= Model.Sponsor.TextAfterLink %>
        <i class="fa fa-info-circle"></i>
    </div>
<% } %>
