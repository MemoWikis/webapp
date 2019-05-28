<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<WelcomeBoxSetTxtQModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="Card CardBig Set">
    <header style="">
        <h6 class="ItemInfo" style="margin-bottom: 5px; margin-top: 0px; color: #a3a3a3;">
            <span class="Pin" data-set-id="<%= Model.SetId %>" style="">
                <a href="#" class="noTextdecoration">
                    <%= Html.Partial("AddToWishknowledge", new AddToWishknowledge(Model.IsInWishknowledge)) %>
                </a>
            </span>&nbsp;
            <a href="<%: Links.SetDetail(Url, Model.Set) %>">Lernset mit <%= Model.QuestionCount %> Fragen</a>
        </h6>
        <h4>
            <a class="PlainTextLook" href="<%: Links.SetDetail(Url, Model.Set) %>"><%: Model.SetName %></a>
        </h4>
    </header>
    <div class="CardContent">
        <div class="row">
            <div class="col-xs-4">
                <div class="ImageContainer">
                     <%= Model.ImageFrontendData.RenderHtmlImageBasis(180, false, ImageType.QuestionSet, linkToItem: Links.SetDetail(Url, Model.Set), noFollow: true) %>
                </div>
            </div>
            <div class="col-xs-8 xxs-stack">
                <p><a class="PlainTextLook" href="<%= Links.SetDetail(Url, Model.Set)%>"><%: Model.SetDescription %></a></p>
            </div>
            <div class="col-xs-8 xxs-stack pull-right">

                <div class="LabelList">
                    <% foreach (var question in Model.Questions){ %>
                    <div class="LabelItem LabelItem-Question" style="padding-bottom: 10px">
                        <a class="PlainTextLook" href="<%= Links.AnswerQuestion(question, Model.Set) %>">
                            <%= question.Text %>
                         </a>
                    </div>
            <% } %>
                </div>
            </div>
        </div>
    </div>
    <div class="BottomBar">
            <div class="dropdown">
                <% var buttonId = Guid.NewGuid(); %>
                <a href="#" id="<%=buttonId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonOnHover ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                    <i class="fa fa-ellipsis-v"></i>
                </a>
                <ul class="dropdown-menu" aria-labelledby="<%=buttonId %>">
                    <li><a href="<%= Links.StartLearningSessionForSet(Model.SetId) %>" rel="nofollow" data-allowed="logged-in">Jetzt lernen</a></li>
                    <li><a href="<%= Model.SetDetailLink %>"> Lernset-Detailseite</a></li>
                </ul>
            </div>
            <a href="<%= Links.TestSessionStartForSet(Model.SetName, Model.SetId) %>" class="btn btn-link btn-sm ButtonOnHover" role="button" rel="nofollow">
                <i class="fa fa-play-circle AnswerResultIcon">&nbsp;&nbsp;</i>WISSEN TESTEN
            </a>
        </div>
</div>

