﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<link href="/Views/Questions/Answer/LearningSession/LearningSessionResult.css" rel="stylesheet" />

<div class="SessionHeading">
    <% if (Model.Creator.IsMemuchoUser && Model.SponsorModel != null && !Model.SponsorModel.IsAdFree){ %>
        <div class="SponsorWrapper">
            <span class="SponsorText">Mit Unterstützung von </span><a href="<%= Model.SponsorModel.Sponsor.SponsorUrl %>" class="SponsorLink"><%= Model.SponsorModel.Sponsor.LinkText %></a>
        </div>
    <% } %>
    <div class="SessionTitle">
        <% if(Model.TestSession.IsSetSession) { %>
            <div class="CollectionType">
                Fragesatz
            </div>
            <div class="LabelWrapper">
                <a class="LabelLink" href="<%= Model.TestSession.SetLink %>">
                    <span class="label label-set show-tooltip" data-original-title="Zum Fragesatz <%= Model.TestSession.SetName %> mit <%= Model.TestSession.SetQuestionCount %> Fragen"><%: Model.TestSession.SetName %></span>
                </a>
            </div>
        <% } %>

        <% if(Model.TestSession.IsSetsSession) { %>
            <%= Model.TestSession.SetListTitle %> (<span style="white-space: nowrap"><%= Model.TestSession.SetsToTestIds.Count %> Fragesätze</span>)
        <% } %>

        <% if(Model.TestSession.IsCategorySession) { %>
            <div class="CollectionType">
                Thema 
            </div>
            <div class="LabelWrapper">
                <a class="LabelLink" href="<%= Links.CategoryDetail(Model.TestSession.CategoryToTest.Name, Model.TestSession.CategoryToTest.Id) %>">
                    <span class="label label-category"><%: Model.TestSession.CategoryToTest.Name %></span>
                </a>
            </div>
        <% } %>
    </div>
</div>
<div class="SessionBar">
    <div class="QuestionCount" style="float: right;">Abfrage <%= Model.TestSessionCurrentStep %> von <%= Model.TestSessionNumberOfSteps %></div>
    <div class="SessionType">
        <span class="show-tooltip"
            data-original-title="<%= @"<div style='text-align: left;'>In diesem Modus
                <ul>
                    <li>werden die Fragen zufällig ausgewählt</li>
                    <li>hast du jeweils nur einen Antwortversuch</li>
                </ul>
            </div>"%>"
            data-html="true" style="float: left;">
            Testen
            <span class="fa-stack fa-1x" style="font-size: 10px; top: -1px;">
                <i class="fa fa-circle fa-stack-2x" style="color: #e1efb3;"></i>
                <i class="fa fa-info fa-stack-1x" style=""></i>
            </span>
        </span>
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