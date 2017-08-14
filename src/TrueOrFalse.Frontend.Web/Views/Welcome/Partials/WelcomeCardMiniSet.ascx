<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<WelcomeCardMiniSetModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="CardColumn">
    <div class="Card CardMini SingleItem Set">
        <div class="ImageContainer">
            <%= Model.ImageFrontendData.RenderHtmlImageBasis(300, true, ImageType.QuestionSet, linkToItem: Links.SetDetail(Model.SetName, Model.SetId), noFollow: true) %>
        </div>

        <div class="ContentContainer">
            <div class="CardContent">
<%--                <h6 class="ItemInfo">
                    <span class="Pin" data-set-id="<%= Model.SetId %>" style="">
                        <a href="#" class="noTextdecoration">
                            <%= Html.Partial("AddToWishknowledge", new AddToWishknowledge(Model.IsInWishknowledge)) %>
                        </a>
                    </span>&nbsp;
                    <a href="<%= Links.SetDetail(Model.SetName, Model.SetId) %>">Lernset mit <%= Model.QuestionCount %> Fragen</a>
                </h6>--%>
                <div class="LinkArea">
                    <h4 class="ItemTitle"><%: Model.SetName %></h4>
                    <a class="Link" href="<%= Links.SetDetail(Model.SetName, Model.SetId) %>"></a>
                </div>
            </div>
            <div class="BottomBar">
                <div class="dropdown">
                    <% var buttonId = Guid.NewGuid(); %>
                    <a href="#" id="<%=buttonId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonOnHover ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                        <i class="fa fa-ellipsis-v"></i>
                    </a>
                    <ul class="dropdown-menu" aria-labelledby="<%=buttonId %>">
                        <li><a href="<%= Links.StartLearningSesssionForSet(Model.SetId) %>" data-allowed="logged-in" data-allowed-type="learning-session" rel="nofollow">Jetzt lernen</a></li>
                        <li><a href="<%= Links.GameCreateFromSet(Model.SetId) %>"> Spiel starten</a></li>
                        <li><a href="<%= Links.DateCreateForSet(Model.SetId) %>"> Termin anlegen</a></li>
                        <li><a href="<%= Links.SetDetail(Model.SetName, Model.SetId) %>"> Zur Themenseite</a></li>
                    </ul>
                </div>
                <a href="<%= Links.TestSessionStartForSet(Model.SetName, Model.SetId) %>" class="btn btn-link btn-sm ButtonOnHover" role="button" rel="nofollow">
                    <i class="fa fa-play-circle AnswerResultIcon">&nbsp;&nbsp;</i>WISSEN TESTEN
                </a>
            </div>
        </div>
    </div>
</div>