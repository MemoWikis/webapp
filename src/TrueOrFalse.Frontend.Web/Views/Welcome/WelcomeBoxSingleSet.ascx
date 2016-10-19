<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<WelcomeBoxSingleSetModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div class="ThumbnailColumn">
    <div class="thumbnail">
        <div class="ImageContainer">
            <%= Model.ImageFrontendData.RenderHtmlImageBasis(200, true, ImageType.QuestionSet, linkToItem: Links.SetDetail(Url,Model.SetName,Model.SetId)) %>
        </div>

        <div>
            <!-- % Html.RenderPartial("Category", Model.Question); % -->
        </div>

        <div class="caption">
            <h6 style="margin-bottom: 5px; color: #a3a3a3;">
                <span class="Pin" data-set-id="<%= Model.SetId %>" style="">
                    <a href="#" class="noTextdecoration">
                        <i class="fa fa-heart show-tooltip iAdded <%= Model.IsInWishknowledge ? "" : "hide2" %>" style="color: #b13a48;" title="Aus deinem Wunschwissen entfernen"></i>
                        <i class="fa fa-heart-o show-tooltip iAddedNot <%= Model.IsInWishknowledge ? "hide2" : "" %>" style="color:#b13a48;" title="Zu deinem Wunschwissen hinzuzufügen"></i>
                        <i class="fa fa-spinner fa-spin hide2 iAddSpinner" style="color:#b13a48;"></i>
                    </a>
                </span>&nbsp;
                Fragesatz mit <a href="<%= Links.SetDetail(Url,Model.SetName,Model.SetId) %>"><%= Model.QCount %> Fragen</a>
            </h6>
            <h4 style="margin-top: 5px;"><%: Model.SetName %></h4>
            <p><%: Model.SetText %></p>
            <p style="text-align: center;">
                <a href="<%= Links.TestSessionStartForSet(Model.SetId) %>" class="btn btn-primary btn-sm" role="button" rel="nofollow"><i class="fa fa-play-circle AnswerResultIcon">&nbsp;</i>&nbsp;Jetzt testen</a>
                <%--<a href="<%= Links.AnswerQuestion(Url, Model.FirstQText, Model.FirstQId, Model.SetId) %>" class="btn btn-primary btn-sm" role="button">Alle beantworten</a>--%>
            </p>
        </div>
    </div>
</div>
