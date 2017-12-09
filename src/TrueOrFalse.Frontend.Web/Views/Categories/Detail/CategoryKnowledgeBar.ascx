<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryKnowledgeBarModel>" %>

<div class="category-knowledge-bar" data-category-id="<%= Model.Category.Id%>" style="position: relative;">

    <% if(Model.CategoryKnowledgeSummary.NeedsLearningPercentage > 0) { %>
        <div class="needs-learning show-tooltip"
            data-html="true"
            title="Solltest du lernen: <br/> <%= Model.CategoryKnowledgeSummary.NeedsLearning %> Fragen (<%= Model.CategoryKnowledgeSummary.NeedsLearningPercentage %>%)"
            style="width: <%= Model.CategoryKnowledgeSummary.NeedsLearningPercentage %>%;"></div>
    <% } %>

    <% if(Model.CategoryKnowledgeSummary.NeedsConsolidationPercentage > 0) { %>
        <div class="needs-consolidation show-tooltip"
            data-html="true"
            title="Solltest du festigen: <br/> <%= Model.CategoryKnowledgeSummary.NeedsConsolidation %> Fragen (<%= Model.CategoryKnowledgeSummary.NeedsConsolidationPercentage %>%)"
            style="width: <%= Model.CategoryKnowledgeSummary.NeedsConsolidationPercentage %>%;"></div>
    <% } %>
    
    <% if(Model.CategoryKnowledgeSummary.SolidPercentage > 0) { %>
        <div class="solid-knowledge show-tooltip" 
            data-html="true"
            title="Sicheres Wissen: <br/> <%= Model.CategoryKnowledgeSummary.Solid %> Fragen (<%= Model.CategoryKnowledgeSummary.SolidPercentage %>%)" 
            style="width: <%= Model.CategoryKnowledgeSummary.SolidPercentage %>%;"></div>
    <% } %>

    <% if(Model.CategoryKnowledgeSummary.NotLearnedPercentage > 0) { %>
        <div class="not-learned show-tooltip"
            data-html="true"
            title="Noch nicht gelernt: <br/> <%= Model.CategoryKnowledgeSummary.NotLearned %> Fragen (<%= Model.CategoryKnowledgeSummary.NotLearnedPercentage %>%)"
            style="width: <%= Model.CategoryKnowledgeSummary.NotLearnedPercentage %>%;"></div>
    <% } %>

    <% if(Model.CategoryKnowledgeSummary.NotInWishknowledgePercentage > 0) { %>
        <div class="not-in-wish-knowledge show-tooltip"
            data-html="true"
            title="Noch nicht im Wunschwissen: <br/> <%= Model.CategoryKnowledgeSummary.NotInWishknowledge %> Fragen (<%= Model.CategoryKnowledgeSummary.NotInWishknowledgePercentage %>%)"
            style="width: <%= Model.CategoryKnowledgeSummary.NotInWishknowledgePercentage %>%;"></div>
    <% } %>
    <% if (Model.CategoryKnowledgeSummary.NotInWishknowledgePercentage == 100)
       { %>
           <div class="ConditionalLegend" style="display: none;">
               Dein Wissensstand
           </div>
       <% } %>
</div>