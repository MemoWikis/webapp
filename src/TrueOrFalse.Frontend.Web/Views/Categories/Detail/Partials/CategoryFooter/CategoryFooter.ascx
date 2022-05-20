<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="CategoryFooter">
    
    <% if (Model.CountAggregatedQuestions > 0)
       { %>
        <div class="footerToolbar">
            <div class="">
                <%= Model.CountAggregatedQuestions %> Frage<%= Model.CountAggregatedQuestions > 1 ? "n" : "" %> im Thema
            </div>

            <div class="StartLearningSession">
                <a href="<%=Links.LearningSessionFooter(Model.Id, Model.Category.Name) %>" id="LearningFooterBtn" data-tab-id="LearningTab" class="btn btn-lg btn-primary footerBtn memo-button" >Jetzt Lernen</a> 
            </div>
        </div>
        
    <% } %>

    <%Html.RenderPartial("~/Views/Categories/Detail/Partials/CategoryNetwork/CategoryNetwork.ascx", Model); %>
</div>

