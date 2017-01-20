<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<WelcomeBoxSingleSetModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<%--<div class="CardColumn" style="display: none;">
    <div class="thumbnail">
        <div class="ImageContainer">
            <%= Model.ImageFrontendData.RenderHtmlImageBasis(200, true, ImageType.QuestionSet, linkToItem: Links.TestSessionStartForSet(Model.SetName, Model.SetId), noFollow: true) %>
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
                <a href="<%= Links.TestSessionStartForSet(Model.SetName, Model.SetId) %>" class="btn btn-primary btn-sm" role="button" rel="nofollow"><i class="fa fa-play-circle AnswerResultIcon">&nbsp;</i>&nbsp;Jetzt testen</a>
            </p>
        </div>
    </div>
</div>--%>

<div class="CardColumn">
    <div class="Card SingleItem Set ">
        <div class="ImageContainer">
            <%= Model.ImageFrontendData.RenderHtmlImageBasis(300, true, ImageType.QuestionSet, linkToItem: Links.TestSessionStartForSet(Model.SetName, Model.SetId), noFollow: true) %>
        </div>

        <div>
            <!-- % Html.RenderPartial("Category", Model.Question); % -->
        </div>
        <div class="ContentContainer">
            <div class="CardContent">
                <h6 class="ItemInfo">
                    <span class="Pin" data-set-id="<%= Model.SetId %>" style="">
                        <a href="#" class="noTextdecoration">
                            <i class="fa fa-heart show-tooltip iAdded <%= Model.IsInWishknowledge ? "" : "hide2" %>" style="color: #b13a48;" title="Aus deinem Wunschwissen entfernen"></i>
                            <i class="fa fa-heart-o show-tooltip iAddedNot <%= Model.IsInWishknowledge ? "hide2" : "" %>" style="color:#b13a48;" title="Zu deinem Wunschwissen hinzuzufügen"></i>
                            <i class="fa fa-spinner fa-spin hide2 iAddSpinner" style="color:#b13a48;"></i>
                        </a>
                    </span>&nbsp;
                    Fragesatz mit <a href="<%= Links.SetDetail(Url,Model.SetName,Model.SetId) %>"><%= Model.QCount %> Fragen</a>
                </h6>
                <h4 class="ItemTitle"><%: Model.SetName %></h4>
                <div class="ItemText"><%: Model.SetText %></div>
            </div>
            <div class="BottomBar">
                <div class="dropdown">
                    <% var buttonId = Guid.NewGuid(); %>
                    <a href="#" id="<%=buttonId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonOnHover ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                        <i class="fa fa-ellipsis-v"></i>
                    </a>
                    <ul class="dropdown-menu" aria-labelledby="<%=buttonId %>">
                        <li><a href="<%= Links.StartLearningSesssionForSet(Model.SetId) %>" data-allowed="logged-in" data-allowed-type="learning-session" rel="nofollow">Jetzt üben</a></li>
                        <li><a href="<%= Links.GameCreateFromSet(Model.SetId) %>"> Spiel starten</a></li>
                        <li><a href="<%= Links.DateCreateForSet(Model.SetId) %>"> Termin anlegen</a></li>
                        <li><a href="<%= Links.SetDetail(Model.SetName, Model.SetId) %>"> Fragesatz-Detailseite</a></li>
                    </ul>
                </div>
                <a href="<%= Links.TestSessionStartForSet(Model.SetName, Model.SetId) %>" class="btn btn-link btn-sm ButtonOnHover" role="button" rel="nofollow">
                    &nbsp;JETZT TESTEN
                </a>
            </div>
        </div>
    </div>
</div>
