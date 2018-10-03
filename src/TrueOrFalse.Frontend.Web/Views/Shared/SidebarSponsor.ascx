<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SponsorModel>" %>

<% if (!Model.IsAdFree){ %>
    <div class="EduPartnerLSidebarWrapper">
        <div class="EduPartnerSidebarLogo">
            <a href="<%= Model.Sponsor.SponsorUrl %>" target="_blank" rel="nofollow"><img src="<%= Model.Sponsor.ImageUrl %>" style="<%= Model.Sponsor.ImageStyleOverwrite %>" /></a>
        </div>
        <%= Model.Sponsor.TextBeforeLink %> <a href="<%= Model.Sponsor.SponsorUrl%>" rel="nofollow"><%= Model.Sponsor.LinkText %></a> <%= Model.Sponsor.TextAfterLink %>
        <div style="width:100%; display:flex; justify-content:flex-end;">
            <i class="fa fa-info-circle show-tooltip" style="width:15px;" title="Unser Bildungssponsor unterstützt die Erstellung von freien Lerninhalten durch memucho. Eine inhaltliche Einflussnahme gibt es dabei nicht."></i>
       </div>
    </div>
<% } %>
