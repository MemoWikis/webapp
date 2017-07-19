<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryKnowledgeBarModel>" %>

<div class="category-knowledge-bar">
    <div class="solid-knowledge show-tooltip" 
        data-html="true"
        title="Sicheres Wissen: <br/> <%= Model.CategoryKnowledgeSummary.Solid %> Fragen (<%= Model.CategoryKnowledgeSummary.SolidPercentage %>%)" 
        style="width: <%= Model.CategoryKnowledgeSummary.SolidPercentage %>%;"></div>

    <div class="needs-learning"
        data-html="true"
        title="Solltest du festige: <br/> <%= Model.CategoryKnowledgeSummary.NeedsLearning %> Fragen (<%= Model.CategoryKnowledgeSummary.NeedsLearningPercentage %>%)"
        style="width: <%= Model.CategoryKnowledgeSummary.NeedsLearningPercentage %>%;"></div>

    <div class="needs-consolidation"
        data-html="true"
        title="Solltest du lernen: <br/> <%= Model.CategoryKnowledgeSummary.NeedsConsolidation %> Fragen (<%= Model.CategoryKnowledgeSummary.NeedsConsolidationPercentage %>%)"
        style="width: <%= Model.CategoryKnowledgeSummary.NeedsConsolidationPercentage %>%;"></div>

    <div class="not-learned"
        data-html="true"
        title="Noch nicht gelernt: <br/> <%= Model.CategoryKnowledgeSummary.NotLearned %> Fragen (<%= Model.CategoryKnowledgeSummary.NotLearnedPercentage %>%)"
        style="width: <%= Model.CategoryKnowledgeSummary.NotLearnedPercentage %>%;"></div>

    <div class="not-in-wish-knowledge show-tooltip"
        data-html="true"
        title="Noch nicht im Wunschwissen: <br/> <%= Model.CategoryKnowledgeSummary.NotInWishknowledge %> Fragen (<%= Model.CategoryKnowledgeSummary.NotInWishknowledgePercentage %>%)"
        style="width: <%= Model.CategoryKnowledgeSummary.NotInWishknowledgePercentage %>%;"></div>
</div>