<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<SetCardMiniListModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div class="setCardMiniList">
    <div class="row">

        <% foreach (var set in Model.Sets)
            { %>
        <div class="col-xs-6 xxs-stack setCardMini">
            <div class="row">
                <div class="col-xs-3">
                    <div class="ImageContainer">
                        <%= Model.GetSetImage(set).RenderHtmlImageBasis(128, true, ImageType.Category, linkToItem: Links.SetDetail(set)) %>
                    </div>
                </div>
                <div class="col-xs-9">
                    <a class="topic-name" href="<%= Links.GetUrl(set) %>">
                        <div class="topic-name">
                            <%: set.Name %>
                        </div>
                    </a>
                    <div class="set-question-count">
                        Lernset mit <%= set.QuestionCount() /*includes private questions! excluding them would also exclude private questions visible to user*/ %>
                        Frage<%= StringUtils.PluralSuffix(set.QuestionCount(),"n") %>
                    </div>
                    <div class="KnowledgeBarWrapper">
                        <% Html.RenderPartial("~/Views/Sets/Detail/SetKnowledgeBar.ascx", new SetKnowledgeBarModel(set)); %>
                        <div class="KnowledgeBarLegend">Dein Wissensstand</div>
                    </div>

                </div>
            </div>
        </div>
        <% } %>
    </div>
</div>
