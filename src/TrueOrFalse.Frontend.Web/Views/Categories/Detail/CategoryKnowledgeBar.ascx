<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryKnowledgeBarModel>" %>

<div class="category-knowledge-bar">
    <div class="knowledge-summary-section solid-knowledge" style="width: <%= Model.CategoryKnowledgeSummary.SolidPercentage %>%;"></div>
    <div class="knowledge-summary-section needs-learning" style="width: <%= Model.CategoryKnowledgeSummary.NeedsLearningPercentage %>%;"></div>
    <div class="knowledge-summary-section needs-consolidation" style="width: <%= Model.CategoryKnowledgeSummary.NeedsConsolidationPercentage %>%;"></div>
    <div class="knowledge-summary-section not-learned" style="width: <%= Model.CategoryKnowledgeSummary.NotLearnedPercentage %>%;"></div>
    <div class="knowledge-summary-section not-in-wish-knowledge" style="width: <%= Model.CategoryKnowledgeSummary.NotInWishknowledgePercentage %>%;"></div>
</div>