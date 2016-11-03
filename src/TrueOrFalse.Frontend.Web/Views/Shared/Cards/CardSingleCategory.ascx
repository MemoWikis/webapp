<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CardSingleCategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div class="ThumbnailColumn" style="display: none;">
    <div class="thumbnail">
        <div class="ImageContainer">
            <%= Model.ImageFrontendData.RenderHtmlImageBasis(200, true, ImageType.Category, linkToItem: Links.TestSessionStartForCategory(Model.CategoryName, Model.CategoryId), noFollow: true) %>
        </div>

        <div class="caption">
            <h6 style="margin-bottom: 5px; color: #a3a3a3;">
                Kategorie mit <a href="<%= Links.CategoryDetail(Model.CategoryName, Model.CategoryId) %>"><%= Model.QCount %> Fragen</a>
            </h6>
            <h4 style="margin-top: 5px;"><%: Model.CategoryName %></h4>
            <p><%: Model.CategoryText %></p>
            <p style="text-align: center;">
                <a href="<%= Links.TestSessionStartForCategory(Model.CategoryName, Model.CategoryId) %>" class="btn btn-primary btn-sm" role="button" rel="nofollow"><i class="fa fa-play-circle AnswerResultIcon">&nbsp;</i>&nbsp;Jetzt testen</a>
            </p>
        </div>
    </div>
</div>

<div class="ThumbnailColumn">
    <div class="Card SingleItem Category">
        <div class="ImageContainer ImageFull">
            <%= Model.ImageFrontendData.RenderHtmlImageBasis(300, true, ImageType.Category, linkToItem: Links.TestSessionStartForCategory(Model.CategoryName, Model.CategoryId), noFollow: true) %>
        </div>

        <div class="CardContent">
            <h6 class="ItemInfo">
                Kategorie mit <a href="<%= Links.CategoryDetail(Model.CategoryName,Model.CategoryId) %>"><%= Model.QCount %> Fragen</a>
            </h6>
            <h4 class="ItemTitle"><%: Model.CategoryName %></h4>
            <div class="ItemText"><%: Model.CategoryText %></div>
        </div>
        <div class="BottomBar">
            <div class="dropdown">
                <% var buttonId = Guid.NewGuid(); %>
                <a href="#" id="<%=buttonId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonOnHover ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                    <i class="fa fa-ellipsis-v"></i>
                </a>
                <ul class="dropdown-menu" aria-labelledby="<%=buttonId %>">
                    <li><a href="<%= Links.CategoryDetail(Model.CategoryName, Model.CategoryId) %>"> Detailseite Kategorie</a></li>
                </ul>
            </div>
            <a href="<%= Links.TestSessionStartForCategory(Model.CategoryName, Model.CategoryId) %>" class="btn btn-link btn-sm ButtonOnHover" role="button" rel="nofollow">
                &nbsp;JETZT TESTEN
            </a>
        </div>
    </div>
</div>
