<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<TopicNavigationModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% if (Model.CategoryList.Any()) { %>
    <h2><%: Model.Title %></h2>
    <p><%: Model.Text %></p>
<% } %>


<div class="container">
    <div class="row">
        <div id="knowledgeAsABox" class="col-sm"></div>
        
        <% var objectGetQuestionKnowledge = Model.BuildObjectGetQuestionKnowledge();
           
           %>
        
        
        <% for (var i = 0; i < objectGetQuestionKnowledge.KnowledgeStatus.Count; i++)
           {
               var t = TopicNavigationModel.returnKnowledgeStatus(objectGetQuestionKnowledge.KnowledgeStatus, i);
        %> <span class= "<% =t %> square-wish-knowledge" ></span> <%

           } %>
        
   
        
    </div>
</div>

