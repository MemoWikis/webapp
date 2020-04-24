<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Control Language="C#"  Inherits="System.Web.Mvc.ViewUserControl<AnswerBodyModel>" %>

<div id="QuestionDetailsApp">
    <question-details-component model-question-id="<%= Model.QuestionId %>" is-in-learning-tab="<%= Model.IsInLearningTab %>"/>
</div>
<div id="QuestionDetailsFooter">
    <div class="questionDetailsFooterPartialLeft">

        <div id="LicenseQuestion">
            <% if (Model.LicenseQuestion.IsDefault()) { %>
                <a class="TextLinkWithIcon" rel="license" href="http://creativecommons.org/licenses/by/4.0/" data-toggle="popover" data-trigger="focus" title="Infos zur Lizenz <%= LicenseQuestionRepo.GetDefaultLicense().NameShort %>" data-placement="auto top"
                   data-content="Autor: <a href='<%= Links.UserDetail(Model.Creator) %>' <%= Model.IsInWidget ? "target='_blank'" : "" %>><%= Model.Creator.Name %></a><%= Model.IsInWidget ? " (Nutzer auf <a href='/' target='_blank'>memucho.de</a>)" : " " %><br/><%= LicenseQuestionRepo.GetDefaultLicense().DisplayTextFull %>">
                    <div> <img src="/Images/Licenses/cc-by 88x31.png" width="60" style="margin-top: 4px; opacity: 0.6; padding-bottom: 2px;" />&nbsp;</div>
                    <div  class="TextDiv"> <span class="TextSpan"><%= LicenseQuestionRepo.GetDefaultLicense().NameShort %></span></div>
                </a><%--target blank to open outside the iframe of widget--%>

            <% } else { %>
                <a class="TextLinkWithIcon" href="#" data-toggle="popover" data-trigger="focus" title="Infos zur Lizenz" data-placement="auto top" data-content="<%= Model.LicenseQuestion.DisplayTextFull %>">
                    <div class="TextDiv"><span class="TextSpan"><%= Model.LicenseQuestion.DisplayTextShort %></span>&nbsp;&nbsp;<i class="fa fa-info-circle">&nbsp;</i></div>
                </a>
            <% } %>
        </div>
        <div class="created"> Erstellt von: <a href="<%= Links.UserDetail(Model.Creator) %>"><%= Model.Creator.Name %></a> vor <%= Model.CreationDateNiceText %></div>
    </div>
    
    <div class="questionDetailsFooterPartialRight">
        <div class="wishknowledgeCount"><i class="fas fa-heart"></i><span id="<%= "WishknowledgeCounter-" + Model.QuestionId %>" data-relevance="<%= Model.IsInWishknowledge %>"><%= Model.Question.TotalRelevancePersonalEntries %></span></div>
        <div class="viewCount"><i class="fas fa-eye"></i><span><%= Model.Question.TotalViews %></span></div>
        <div class="commentCount"><a href="<%= Links.GetUrl(Model.Question) + "#QuestionComments"%>"><i class="fas fa-comment"></i><span><%= Model.CommentCount %></span></a></div>
    </div>


</div>

<%= Scripts.Render("~/bundles/js/QuestionDetailsApp") %>

