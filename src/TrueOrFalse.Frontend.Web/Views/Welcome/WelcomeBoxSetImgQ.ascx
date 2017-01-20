<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<WelcomeBoxSetImgQModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="Card CardBig Set">
    <header style="">
        <h6 style="margin-bottom: 5px; margin-top: 0px; color: #a3a3a3;">
            <span class="Pin" data-set-id="<%= Model.SetId %>" style="">
                <a href="#" class="noTextdecoration">
                    <i class="fa fa-heart show-tooltip iAdded <%= Model.IsInWishknowledge ? "" : "hide2" %>" style="color: #b13a48;" title="Aus deinem Wunschwissen entfernen"></i>
                    <i class="fa fa-heart-o show-tooltip iAddedNot <%= Model.IsInWishknowledge ? "hide2" : "" %>" style="color:#b13a48;" title="Zu deinem Wunschwissen hinzuzufügen"></i>
                    <i class="fa fa-spinner fa-spin hide2 iAddSpinner" style="color:#b13a48;"></i>
                </a>
            </span>&nbsp;
            Fragesatz mit <a href="<%: Links.SetDetail(Url, Model.Set) %>"><%= Model.QuestionCount %> Fragen</a>
        </h6>
        <h4><%: Model.SetName %></h4>
    </header>
    <div class="CardContent">
        <div class="row">
            
            <div class="col-xs-12 xxs-stack" style="margin-bottom: 10px">
                <p><%: Model.SetText %></p>
            </div>

        <% foreach (var question in Model.Questions){ %>
        <div class="col-xs-12 col-sm-4">
            <div class="row" style="padding-bottom: 10px;">
                    
                <div class="col-xs-3 col-sm-12" style="padding-bottom: 20px;">
                    <div class="ImageContainer ImageLicenseOnImage-sm-up">
                        <%= Model.QuestionImageFrontendDatas
                        .First(x => x.Item1 == question.Id).Item2.RenderHtmlImageBasis(200, true, ImageType.Question, linkToItem: Links.TestSessionStartForSet(Model.SetName, Model.SetId), noFollow: true) %>
                    </div>
                </div>
                <div class="col-xs-9 col-sm-12">
                    <div class="LabelItem LabelItem-Question" style="padding-bottom: 10px">
                        <%= question.Text %>
                    </div>
                </div>
            </div>
        </div>
    <% } %>
        </div>
    </div>
    <div class="BottomBar">
            <%--<a href="<%= Links.AnswerQuestion(Url, Model.FirstQText, Model.FirstQId, Model.SetId) %>" class="btn btn-primary btn-sm" role="button">Alle beantworten</a>--%>
            <div class="dropdown">
                <% var buttonId = Guid.NewGuid(); %>
                <a href="#" id="<%=buttonId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonOnHover ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                    <i class="fa fa-ellipsis-v"></i>
                </a>
                <ul class="dropdown-menu" aria-labelledby="<%=buttonId %>">
                    <li><a href="<%= Links.StartLearningSesssionForSet(Model.SetId) %>" rel="nofollow" data-allowed="logged-in">Jetzt üben</a></li>
                    <li><a href="<%= Links.GameCreateFromSet(Model.SetId) %>"> Spiel starten</a></li>
                    <li><a href="<%= Links.DateCreateForSet(Model.SetId) %>"> Termin anlegen</a></li>
                    <li><a href="<%= Model.SetDetailLink %>"> Fragesatz-Detailseite</a></li>
                </ul>
            </div>
            <a href="<%= Links.TestSessionStartForSet(Model.SetName, Model.SetId) %>" class="btn btn-link btn-sm ButtonOnHover" role="button" rel="nofollow">
                &nbsp;JETZT TESTEN
            </a>
        </div>
</div>


