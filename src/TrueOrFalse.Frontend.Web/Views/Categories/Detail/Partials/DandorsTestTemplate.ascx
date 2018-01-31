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
           var t = objectGetQuestionKnowledge;%>
        <p> hallo <% objectGetQuestionKnowledge.NumberKnowledgeQuestions.ToString(); %></p>
        <% for (var i = 0; i < objectGetQuestionKnowledge.NumberKnowledgeQuestions; i++)
           {
              %> <span style ='float:left; height: 20px; width: 20px; background-color: green; border: black 1px solid '></span> <%

           } %>
        
   
        
    </div>
</div>

