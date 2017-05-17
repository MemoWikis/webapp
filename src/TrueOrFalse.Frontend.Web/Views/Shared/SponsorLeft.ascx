<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SponsorModel>" %>

<% if (!Model.IsAdFree){ %>
    <div class="EduPartnerLeftWrapper">
        <div class="EduPartnerAdLabel">ANZEIGE</div>
        <div class="EduPartnerLeftLogo">
            <a href="<%= Model.Sponsor.SponsorUrl %>" target="_blank" rel="nofollow"><img src="<%= Model.Sponsor.ImageUrl %>" style="<%= Model.Sponsor.ImageStyleOverwrite %>" /></a>
        </div>
        <%= Model.Sponsor.TextBeforeLink %> <a href="<%= Model.Sponsor.SponsorUrl%>" rel="nofollow"><%= Model.Sponsor.LinkText %></a> <%= Model.Sponsor.TextAfterLink %>
    </div>
<% } %>
