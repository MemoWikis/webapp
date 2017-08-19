<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<WelcomeBoxSetImgQModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="Card CardBig Set">
    <header>
        <h6 class="ItemInfo" style="margin-bottom: 5px; margin-top: 0px; color: #a3a3a3;">
            <span class="Pin" data-set-id="<%= Model.SetId %>">
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
            
            <div class="col-xs-12 xxs-stack" style="margin-bottom: 10px">
                <p><a class="PlainTextLook" href="<%: Links.SetDetail(Url, Model.Set) %>"><%: Model.SetText %></a></p>
            </div>

            <% foreach (var question in Model.Questions){ %>
            <div class="col-xs-12 col-sm-4">
                <div class="row" style="padding-bottom: 10px;">
                    <div class="col-xs-3 col-sm-12" style="padding-bottom: 20px;">
                        <div class="ImageContainer ImageLicenseOnImage-sm-up">
                            <%= Model.QuestionImageFrontendDatas
                            .First(x => x.Item1 == question.Id).Item2
                            .RenderHtmlImageBasis(200, true, ImageType.Question, linkToItem: Links.AnswerQuestion(question, Model.Set), noFollow: true) %>
                        </div>
                    </div>
                    <div class="col-xs-9 col-sm-12">
                        <div class="LabelItem LabelItem-Question" style="padding-bottom: 10px">
                            <a class="PlainTextLook" href="<%= Links.AnswerQuestion(question, Model.Set)%>"><%: question.Text %></a>
                        </div>
                    </div>
                </div>
            </div>
            <% } %>
        </div>
    </div>
    <div class="BottomBar">
            <%--<a href="<%= Links.AnswerQuestion(Model.FirstQText, Model.FirstQId, Model.SetId) %>" class="btn btn-primary btn-sm" role="button">Alle beantworten</a>--%>
            <div class="dropdown">
                <% var buttonId = Guid.NewGuid(); %>
                <a href="#" id="<%=buttonId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonOnHover ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                    <i class="fa fa-ellipsis-v"></i>
                </a>
                <ul class="dropdown-menu" aria-labelledby="<%=buttonId %>">
                    <li><a href="<%= Links.StartLearningSesssionForSet(Model.SetId) %>" rel="nofollow" data-allowed="logged-in">Jetzt lernen</a></li>
                    <li><a href="<%= Links.GameCreateFromSet(Model.SetId) %>"> Spiel starten</a></li>
                    <li><a href="<%= Links.DateCreateForSet(Model.SetId) %>"> Termin anlegen</a></li>
                    <li><a href="<%= Model.SetDetailLink %>"> Lernset-Detailseite</a></li>
                </ul>
            </div>
            <a href="<%= Links.TestSessionStartForSet(Model.SetName, Model.SetId) %>" class="btn btn-link btn-sm ButtonOnHover" role="button" rel="nofollow">
                <i class="fa fa-play-circle AnswerResultIcon">&nbsp;&nbsp;</i>WISSEN TESTEN
            </a>
        </div>
</div>


