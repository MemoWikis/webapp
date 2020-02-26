<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<WelcomeCardMiniCategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

    <div class="Card SingleItem Category">
        <div class="ImageContainer">
            <%= Model.ImageFrontendData.RenderHtmlImageBasis(300, true, ImageType.Category, linkToItem: Links.CategoryDetail(Model.CategoryName, Model.CategoryId), noFollow: true) %>
        </div>

        <div class="ContentContainer">
            <div class="CardContent">
                <div class="LinkArea">
                    <h4 class="ItemTitle"><%: Model.CategoryName %></h4>
                    <a class="Link" href="<%= Links.CategoryDetail(Model.CategoryName, Model.CategoryId) %>"></a>
                </div>
            </div>
        </div>
    </div>
