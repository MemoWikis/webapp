<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SetQuestionRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="rowBase question-row" style="padding-top: 10px; padding-bottom: 10px;">
    <div class="col-lg-1 col-xs-2 col-0" style="padding-right: 0px;">
        <div class="ImageContainer ShortLicenseLinkText">
        <%= GetQuestionImageFrontendData.Run(Model.Question).RenderHtmlImageBasis(128, true, ImageType.Question, linkToItem: Links.AnswerQuestion(Url, Model.Question, Model.Set)) %>
        </div>                       
    </div>
    <div class="col-lg-9 col-sm-7 col-xs-10 col-1">
        <div class="Pin" data-question-id="<%= Model.Question.Id %>" style="float: right; display: inline-block">
            <a href="#" class="noTextdecoration" style="font-size: 16px; height: 10px; position: relative; top: 3px; padding-right: 7px; padding-left: 7px;">
                <%= Html.Partial("AddToWishknowledge", new AddToWishknowledge(Model.IsInWishknowledge)) %>
            </a>
        </div>                        
        <a href="<%= Links.AnswerQuestion(Url, Model.Question, Model.Set) %>" style="font-weight:normal; font-size:17px;">
            <%=Model.Question.Text %>
        </a>
        <% if (Model.UserIsInstallationAdmin) { %>
            <p style="margin-top: 8px; padding-left: 15px;">
                <i class="fa fa-user-secret show-tooltip" title="Die Kategorien werden an dieser Stelle nur Admin-Nutzern angezeigt."></i>
                <% foreach (var category in Model.Question.Categories){ %>
                    <% Html.RenderPartial("CategoryLabel", category); %>
                <% } %>
            </p>
        <% } %>
    </div>
    <div class="Stats col-lg-2 col-sm-3 col-xs-10 col-2" style="padding-top: 5px; line-height: 15px;">
        <% Html.RenderPartial("HistoryAndProbability", Model.HistoryAndProbability); %>
    </div>
</div>