<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div id="TestSessionHeader" data-div-type="testSessionHeader">
    <link href="/Views/Questions/Answer/LearningSession/LearningSessionResult.css" rel="stylesheet" />
    <% if(!Model.IsInWidget) { %>
        <div class="SessionHeading">
                    
            <% Html.RenderPartial("~/Views/Questions/Answer/Sponsor.ascx", Model); %>

            <div class="SessionTitle">
                <%
                    if (Model.TestSession.IsSetSession) { %>
                    <div class="CollectionType">
                        Lernset
                    </div>
                    <div class="LabelWrapper">
                        <a class="LabelLink" href="<%= Model.TestSession.SetLink %>">
                            <span class="label label-set show-tooltip" data-original-title="Zum Lernset <%= Model.TestSession.SetName %> mit <%= Model.TestSession.SetQuestionCount %> Fragen"><%: Model.TestSession.SetName %></span>
                        </a>
                    </div>
                <% } %>

                <% if(Model.TestSession.IsSetsSession) { %>
                    <%= Model.TestSession.SetListTitle %> (<span style="white-space: nowrap"><%= Model.TestSession.SetsToTestIds.Count %> Lernsets</span>)
                <% } %>

                <% if(Model.TestSession.IsCategorySession) { %>
                    <div class="CollectionType TypeCategory">
                        Thema 
                    </div>
                
                    <% Html.RenderPartial("CategoryLabel", Model.TestSession.CategoryToTest); %>
                <% } %>
            </div>
        </div>
    <% } %>
    <div  id="QuestionCountCompletSideBar"class="SessionBar">
        <div class="QuestionCount" style="float: right;">Frage <%= Model.TestSessionCurrentStep %> von <%= Model.TestSessionNumberOfSteps %></div>
        <div class="SessionType">
            <% if (Model.IsInWidget) { %>
                <span>
                    Testen
                </span>
            <% } else { %>
        
                <span class="show-tooltip"
                      data-original-title="<%= @"<div style='text-align: left;'>In diesem Modus
                <ul>
                    <li>werden die Fragen zufällig ausgewählt</li>
                    <li>hast du jeweils nur einen Antwortversuch</li>
                </ul>
            </div>" %>"
                      data-html="true" style="float: left;">
                    Testen
                    <span class="fa-stack fa-1x" style="font-size: 10px; top: -1px;">
                        <i class="fa fa-circle fa-stack-2x" style="color: #e1efb3;"></i>
                        <i class="fa fa-info fa-stack-1x" style=""></i>
                    </span>
                </span>
            <% } %>
        </div>
        <div class="ProgressBarContainer">
            <div id="progressPercentageDone" class="ProgressBarSegment ProgressBarDone" style="width: <%= Model.TestSessionCurrentStepPercentage + "%" %>;">
                <div class="ProgressBarSegment ProgressBarLegend">
                    <span id="spanPercentageDone"><%= Model.TestSessionCurrentStepPercentage %>%</span>
                </div>
            </div>
            <% if (Model.TestSessionCurrentStepPercentage<100) {%>
                <div class="ProgressBarSegment ProgressBarLeft" style="width: 100%;"></div>
            <% } %>
            
        </div>
    </div>
</div>

