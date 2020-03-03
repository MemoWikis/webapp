<%@ Control Language="C#" AutoEventWireup="true" 
Inherits="System.Web.Mvc.ViewUserControl<AnalyticsFooterModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% if(!Model.IsQuestionssite) {%>
    <div style="border-bottom: solid 1px #d6d6d6;"></div>
<%} %>
<div class="footerContainer-Analytics row" <%=!Model.IsQuestionssite ? "style='padding: 30px 0; display: flex;'" : "style='padding-top: 50px; padding-bottom: 80px; display: flex;'" %> > 
    <div class="analyticsImageContainer col-sm-4">
        <img src="/Images/Various/knowledgeNetworkSample.png">
    </div>

    <div class="analyticsTextContainer col-sm-8">
        <h1>Wissensnetz</h1>

        <% if (Model.AllCategoriesParents.Count > 0){ %>
            <p>Übergeordnete Themen: <%= Model.AllCategoriesParents.Count %><span> <%= Model.ParentList %></span></p>
        <% } %>
        <% if (Model.CategoriesDescendantsCount > 0){ %>
            <p>Untergeordnete Themen: <%= Model.CategoriesDescendantsCount %></p>
        <% } %>
            
        <div class="OpenAnalyticsTab">
            <a href="<%= Links.AnalyticsFooter(Model.CategoryId, Model.Category.Name) %>" id="AnalyticsFooterBtn" data-tab-id="AnalyticsTab" class="btn btn-lg btn-primary footerBtn">Wissensnetz ansehen</a>   
        </div>
    </div>
</div>