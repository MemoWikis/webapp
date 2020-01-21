<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<SingleCategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
                
    <div class="CardColumn">
        <div class="Card SingleItem Category ">
            <div class="ImageContainer">
                <%= Model.ImageFrontendData.RenderHtmlImageBasis(300, true, ImageType.QuestionSet, linkToItem: Links.CategoryDetail(Model.CategoryName,Model.CategoryId), noFollow: true) %>
            </div>
            <div class="ContentContainer">
                <div class="CardContent">
                    <h6 class="ItemInfo">
                        <span class="Pin" data-set-id="<%= Model.CategoryId %>" style="">
                            <a href="#" class="noTextdecoration">
                                <%= Html.Partial("AddToWishknowledge", new AddToWishknowledge(Model.IsInWishknowledge, false, false, false)) %>
                            </a>
                        </span>&nbsp;
                        <a href="<%= Links.CategoryDetail(Model.Category) %>">Thema mit <%= Model.QCount %> Frage<%= StringUtils.PluralSuffix(Model.QCount, "n") %></a>
                    </h6>
                    <div class="LinkArea">
                        <h4><%: Model.CategoryName %></h4>
                        <div class="ItemText"><%: Model.CategoryText %></div>
                        <a class="Link" href="<%= Links.CategoryDetail(Model.Category) %>"></a>
                    </div>
                </div>
                <div class="BottomBar">
                    <div class="dropdown">
                        <% var buttonId = Guid.NewGuid(); %>
                        <a href="#'" id="<%=buttonId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonOnHover ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                            <i class="fa fa-ellipsis-v"></i>
                        </a>
                        <ul class="dropdown-menu" aria-labelledby="<%=buttonId %>">
                            <li><a href="<%= Links.StartLearningSessionForSet(Model.CategoryId) %>" data-allowed="logged-in" data-allowed-type="learning-session" rel="nofollow">Jetzt lernen</a></li>
                            <li><a href="<%= Links.SetDetail(Model.CategoryName, Model.CategoryId) %>"> Kategorie Detailseite</a></li>
                        </ul>
                    </div>
                    <a href="<%= Links.TestSessionStartForSet(Model.CategoryName, Model.CategoryId) %>" class="btn btn-link btn-sm ButtonOnHover" role="button" rel="nofollow">
                        <i class="fa fa-play-circle AnswerResultIcon">&nbsp;&nbsp;</i>WISSEN TESTEN
                    </a>
                </div>
            </div>
        </div>
    </div>
