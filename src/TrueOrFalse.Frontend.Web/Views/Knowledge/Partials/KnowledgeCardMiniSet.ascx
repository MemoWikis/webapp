<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<KnowledgeCardMiniSetModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="row">
    <div class="col-xs-3">
        <div class="ImageContainer">
            <%= Model.GetSetImage(Model.Set).RenderHtmlImageBasis(128, true, ImageType.Category, linkToItem: Links.SetDetail(Model.Set)) %>
        </div>
    </div>
    <div class="col-xs-9">            
        <a class="topic-name" href="<%= Links.GetUrl(Model.Set) %>">
            <div class="topic-name">
                <%: Model.Set.Name %>
            </div>
        </a>
        <div class="set-question-count">
            Lernset mit <%= Model.Set.QuestionCount() /*includes private questions! excluding them would also exclude private questions visible to user*/ %>
            Frage<%= StringUtils.PluralSuffix(Model.Set.QuestionCount(),"n") %>
        </div>
        <div class="KnowledgeBarWrapper">
            <% Html.RenderPartial("~/Views/Sets/Detail/SetKnowledgeBar.ascx", new SetKnowledgeBarModel(Model.Set)); %>
            <div class="KnowledgeBarLegend">Dein Wissensstand</div>
        </div>
                                        
    </div>
</div>
