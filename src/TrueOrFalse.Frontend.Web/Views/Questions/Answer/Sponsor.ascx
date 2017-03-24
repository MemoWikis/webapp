<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>

<% if (Model.Creator.IsMemuchoUser && Model.SponsorModel != null && !Model.SponsorModel.IsAdFree){ %>
    <div class="SponsorWrapper">
        <span class="SponsorText">Freier Bildungsinhalt – mit Unterstützung von </span>
        <a href="<%= Model.SponsorModel.Sponsor.SponsorUrl %>" class="SponsorLink" rel="nofollow"><%= Model.SponsorModel.Sponsor.LinkText %></a> <i class="fa fa-info-circle show-tooltip" data-placement="bottom" 
            title="Unser Bildungssponsor unterstützt die Erstellung von freien Lerninhalten durch memucho. Eine inhaltliche Einflussnahme gibt es dabei nicht." style="font-size: 120%"></i>
    </div>
<% } %>