<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<WelcomeCardMiniCategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="CardColumn">
    <div class="Card SingleItem Category">
        <div class="ImageContainer">
            <%= Model.ImageFrontendData.RenderHtmlImageBasis(300, true, ImageType.Category, linkToItem: Links.CategoryDetail(Model.CategoryName, Model.CategoryId), noFollow: true) %>
        </div>

        <div class="ContentContainer">
            <div class="CardContent">
<%--                <h6 class="ItemInfo">
                    <span class="Pin" data-category-id="<%= Model.CategoryId %>" style="">
                        <a href="#" class="noTextdecoration">
                            <%= Html.Partial("AddToWishknowledge", new AddToWishknowledge(Model.IsInWishknowledge)) %>
                        </a>
                    </span>&nbsp;
                    <a href="<%= Links.CategoryDetail(Model.CategoryName, Model.CategoryId) %>">Thema mit <%= Model.QuestionCount %> Fragen</a>
                </h6>--%>
                <div class="LinkArea">
                    <h4 class="ItemTitle"><%: Model.CategoryName %></h4>
                    <a class="Link" href="<%= Links.CategoryDetail(Model.CategoryName, Model.CategoryId) %>"></a>
                </div>
            </div>
            <%--<div class="BottomBar">
                <div class="dropdown">
                    <% var buttonId = Guid.NewGuid(); %>
                    <a href="#" id="<%=buttonId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonOnHover ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                        <i class="fa fa-ellipsis-v"></i>
                    </a>
                    <ul class="dropdown-menu" aria-labelledby="<%=buttonId %>">
                        <li><a href="<%= Links.StartCategoryLearningSession(Model.CategoryId) %>" data-allowed="logged-in" data-allowed-type="learning-session" rel="nofollow">Jetzt lernen</a></li>
                        <li><a href="<%= Links.GameCreateFromCategory(Model.CategoryId) %>"> Spiel starten</a></li>
                        <li><a href="<%= Links.DateCreateForCategory(Model.CategoryId) %>"> Termin anlegen</a></li>
                        <li><a href="<%= Links.CategoryDetail(Model.CategoryName, Model.CategoryId) %>"> Zur Themenseite</a></li>
                    </ul>
                </div>
                <a href="<%= Links.TestSessionStartForCategory(Model.CategoryName, Model.CategoryId) %>" class="btn btn-link btn-sm ButtonOnHover" role="button" rel="nofollow">
                    <i class="fa fa-play-circle AnswerResultIcon">&nbsp;&nbsp;</i>WISSEN TESTEN
                </a>
            </div>--%>
        </div>
    </div>
</div>