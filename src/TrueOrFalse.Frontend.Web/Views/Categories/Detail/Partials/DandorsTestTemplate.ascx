<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<TopicNavigationModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% if (Model.CategoryList.Any()) { %>
    <h2><%: Model.Title %></h2>
    <p><%: Model.Text %></p>
<% } %>


<div class="container">
    <div class="row">
        <div id="knowledgeAsABox" class="col-sm"></div>
        
    </div>
</div>

