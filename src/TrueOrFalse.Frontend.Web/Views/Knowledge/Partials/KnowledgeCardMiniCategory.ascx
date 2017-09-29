<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<KnowledgeCardMiniCategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="row">
    <div class="col-xs-3">
        <div class="ImageContainer">
            <%= Model.GetCategoryImage(Model.Category).RenderHtmlImageBasis(128, true, ImageType.Category, linkToItem: Links.CategoryDetail(Model.Category.Name, Model.Category.Id)) %>
        </div>
    </div>
    <div class="col-xs-9">
        <a class="topic-name" href="<%= Links.GetUrl(Model.Category) %>">
            <div class="topic-name">
                <%: Model.Category.Name %>
            </div>
        </a>
        <div class="set-question-count">
            <%: Model.GetTotalSetCount(Model.Category) %> Lernset<%= StringUtils.PluralSuffix(Model.GetTotalSetCount(Model.Category),"s") %>
            <%: Model.GetTotalQuestionCount(Model.Category) %> Frage<%= StringUtils.PluralSuffix(Model.GetTotalQuestionCount(Model.Category),"n") %>
        </div>
        <div class="KnowledgeBarWrapper">
            <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(Model.Category)); %>
            <div class="KnowledgeBarLegend">Dein Wissensstand</div>
        </div>
        <%--                                        <div class="showSubTopics">
                                        <button data-toggle="collapse" data-target="#agg<%= category.Id %>"><i class="fa fa-caret-down">&nbsp;</i>Zeige aggreg. Unterthemen</button>
                                        <div id="agg<%= category.Id %>" class="collapse">
                                            <% foreach (var aggregatedCategory in category.AggregatedCategories(false))
                                                {
                                                    Response.Write(aggregatedCategory.Name + " (" + aggregatedCategory.Id + "); ");
                                                } %>
                                        </div>
                                    </div>--%>
    </div>
</div>
