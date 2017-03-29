<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>

<% if (Model.Creator.IsMemuchoUser && Model.SponsorModel != null && !Model.SponsorModel.IsAdFree){ %>
    <div class="EduPartnerWrapper">
        <div class="TextBlock">
            <span class="EduPartnerText">Freier Bildungsinhalt – mit Unterstützung von </span>
            <span style="display: inline-block; white-space: nowrap;" class="show-tooltip" data-placement="bottom" title="Unser Bildungssponsor unterstützt die Erstellung von freien Lerninhalten durch memucho. Eine inhaltliche Einflussnahme gibt es dabei nicht.">
                <a href="<%= Model.SponsorModel.Sponsor.SponsorUrl %>" rel="nofollow" class="EduPartnerLink" target="_blank">
                    <%= Model.SponsorModel.Sponsor.LinkText %>
                </a>
                <i class="fa fa-info-circle" style="font-size: 120%"></i>
            </span>
        </div>
    </div>
<% } %>