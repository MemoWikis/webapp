<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>



<input type="hidden" id="hddIsLearningSession" value="True" 
       data-learning-session-id="-1"
       data-current-step-guid=""
       data-current-step-idx=""
       data-is-last-step=""
       data-skip-step-index="" />

<input type="hidden" id="hddQuestionId" value="1"/>
<input type="hidden" id="hddCategoryId" value="<%= Model.Category.Id %>"/>
<input type="hidden" id="hddLearningSessionStarted" value="False"/>

<div class="SessionHeading">
    
    <% if(Model.Category.CountQuestionsAggregated > 0)
       {
           var dummyQuestion = Sl.QuestionRepo.GetById(Model.Category.GetAggregatedQuestionIdsFromMemoryCache().FirstOrDefault());
           var temporaryAnswerQuestionModel = new AnswerQuestionModel(dummyQuestion);

            Html.RenderPartial("~/Views/Questions/Answer/Sponsor.ascx", temporaryAnswerQuestionModel); %>

    <div class="SessionTitle">
            <div class="CollectionType">Thema</div>
            <div class="LabelWrapper">
                <a class="LabelLink" href="<%= Links.CategoryDetail(Model.Category.Name, Model.Category.Id) %>">
                    <span class="label label-category"><%: Model.Category.Name %></span>
                </a>
            </div>
        <% } %>
    </div>
</div>
<div id="AnswerBody"></div>

<%--<% if (Model.IsLearningSession) { %>
    <% Html.RenderPartial("~/Views/Questions/Answer/LearningSession/LearningSessionHeader.ascx", Model); %>
<% }else if (Model.IsTestSession) { %>
    <% Html.RenderPartial("~/Views/Questions/Answer/TestSession/TestSessionHeader.ascx", Model); %>
<% }else { %>
    <div class="AnswerQuestionHeader">
        <% Html.RenderPartial("~/Views/Questions/Answer/Sponsor.ascx", Model); %>
        <% Html.RenderPartial("~/Views/Questions/Answer/AnswerQuestionPager.ascx", Model); %>
    </div>
<% } %>


<% Html.RenderPartial("~/Views/Questions/Answer/AnswerBodyControl/AnswerBody.ascx",
       new AnswerBodyModel(Model)); %>

<div class="row">
    <% if (!Model.IsLoggedIn && !Model.IsTestSession && !Model.IsLearningSession && Model.SetMinis.Any()) {
           var primarySet = Sl.R<SetRepo>().GetById(Model.SetMinis.First().Id); %>
        <div class="col-sm-6 xxs-stack">
            <div class="well CardContent" style="margin-left: 0; margin-right: 0; padding-top: 10px; min-height: 175px;">
                <h6 class="ItemInfo">
                    <span class="Pin" data-set-id="<%= primarySet.Id %>" style="">
                        <%= Html.Partial("AddToWishknowledge", new AddToWishknowledge(Model.IsInWishknowledge)) %>
                    </span>&nbsp;
                    Lernset mit <a href="<%= Links.SetDetail(Url,primarySet.Name,primarySet.Id) %>"><%= primarySet.Questions().Count %> Fragen</a>
                </h6>
                <h4 class="ItemTitle"><%: primarySet.Name %></h4>
                <div class="ItemText"><%: primarySet.Text %></div>
                <div style="margin-top: 8px; text-align: right;">
                    <a href="<%= Links.TestSessionStartForSet(primarySet.Name, primarySet.Id) %>" class="btn btn-primary btn-sm" role="button" rel="nofollow">
                        <i class="fa fa-play-circle AnswerResultIcon">&nbsp;&nbsp;</i>WISSEN TESTEN
                    </a>
                </div>
            </div>
        </div>
                
    <% } %>
    <div class="col-sm-6 xxs-stack">
        <% Html.RenderPartial("~/Views/Questions/Answer/AnswerQuestionDetails.ascx", Model); %>
    </div>
</div>--%>
