<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<TopicNavigationModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% if (Model.CategoryList.Any()) { %>
    <h2><%: Model.Title %></h2>
    <p><%: Model.Text %></p>
<% } %>

<input type="hidden" id="test" value=>
<div class="container">
    <div class="row">
        <div id="knowledgeAsABox" class="col-sm"></div>
        
        <% for (var i = 0; i < Model.getQuestionKnowledge(504).Count; i++)
           {
              %> <span style ='float:left; height: 20px; width: 20px; background-color: green; border: black 1px solid '></span> <%

           } %>
        
        <p><% Model.getQuestionKnowledge(504).Count.ToString(); %></p>
        
    </div>
</div>

