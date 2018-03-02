<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<TopicNavigationModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% if (Model.CategoryList.Any()) { %>
    <h2><%: Model.Title %></h2>
    <p><%: Model.Text %></p>
<% } %>


<div class="row">
    <div id="knowledgeAsABox" class="col-sm"></div>
        
    <% var objectGetQuestionKnowledge = Model.BuildObjectGetQuestionKnowledge(); %>

    <% for (var i = 0; i < objectGetQuestionKnowledge.KnowledgeStatus.Count; i++)
        {
            var knowledgeStatus = TopicNavigationModel.ReturnKnowledgeStatus(objectGetQuestionKnowledge.KnowledgeStatus, i);
            var questionText = objectGetQuestionKnowledge.AggregatedWishKnowledge[i].Text;
            var id = i;
    %> 
        <span class="<% =knowledgeStatus %> square-wish-knowledge" id="question<% =id %>" data-toggle="tooltip" data-placement="top" title="<% =questionText %>" ></span>
        <p style="display: none; width: 300px; position: relative; z-index: 20; border: 1px black solid;" ></p>
        
    <% } %>
       
</div>
