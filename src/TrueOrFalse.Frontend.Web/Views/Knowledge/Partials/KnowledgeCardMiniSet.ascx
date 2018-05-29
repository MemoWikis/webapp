<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<KnowledgeCardMiniSetModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="row">
    <div class="col-xs-2">
        <div class="ImageContainer">
            <%= Model.GetSetImage(Model.Set).RenderHtmlImageBasis(128, true, ImageType.Category, linkToItem: Links.SetDetail(Model.Set)) %>
        </div>
    </div>
    <div class="col-xs-4">
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
    <div class="col-xs-2">
       <% Html.RenderPartial("~/Views/Shared/AddToWishknowledge.ascx",new AddToWishknowledge(Model.isInWishKnowledge)); %>
    </div>
    <div class="col-xs-4" style="">
        <a href="#" class="btn btn-link link-to-learnset" data-allowed="logged-in" data-allowed-type="learning-session" rel="nofollow" data-setid="<%=Model.Set.Id%>">
            <i class="fa fa-lg fa-line-chart">&nbsp;</i> Gleich richtig lernen
        </a>
        <div class="dropdown">
            <% var buttonId = Guid.NewGuid(); %>
            <a href="#" id="<%=buttonId %>" class="dropdown-toggle btn btn-link ButtonOnHover ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                <i class="fa fa-ellipsis-v"></i>
            </a>
            <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="<%=buttonId %>">
                <li><a href="#"><i class="fa fa-search-plus">&nbsp;</i> Detailseite des Lernsets</a></li>
                <li><a href="#"><i class="fa fa-gamepad">&nbsp;</i> Spiel starten</a></li>
                <li><a href="#"><i class="fa fa-calendar">&nbsp;</i> Prüfungstermin anlegen</a></li>
            </ul>
        </div>
    </div>
</div>
