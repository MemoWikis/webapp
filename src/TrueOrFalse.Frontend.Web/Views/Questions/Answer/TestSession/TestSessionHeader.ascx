<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div class="" style="margin-bottom: 20px;">
    <h2 style="margin-top: 0; position: relative;">
        Du bist in einer TestSession
    </h2>

    <div class="progressBarContainer">
        <% if (Model.CurrentLearningStepPercentage>0) {%>
            <div class="progressBar progressBarDone" style="width: /*<%= Model.CurrentLearningStepPercentage %>*/%;">
                &nbsp;
            </div>
        <% } %>                
        <% if (Model.CurrentLearningStepPercentage<100) {%>
            <div class="progressBar progressBarLeft" style="width: /*<%= 100-Model.CurrentLearningStepPercentage %>*/%;">
                &nbsp; 
            </div>
        <% } %>   
        <div class="progressBar progressBarLegend">
            <%--<%= Model.CurrentLearningStepPercentage %>--%>%
        </div>
    </div>

</div>