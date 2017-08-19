<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<WelcomeBoxCategoryTxtQModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div class="Card CardBig Category">
    <header style="">
        <h6 class="ItemInfo" style="margin-bottom: 5px; margin-top: 0px; color: #a3a3a3;"> 
            <a href="<%: Links.CategoryDetail(Model.Category) %>">
                Thema mit <%= Model.QuestionCount %> Fragen
            </a>
        </h6>
        <h4><a class="PlainTextLook" href="<%= Links.CategoryDetail(Model.Category) %>"><%: Model.CategoryName %></a></h4>
    </header>
    <div class="CardContent">
        <div class="row">
            <div class="col-xs-5 col-sm-4">
                <div class="ImageContainer">
                    <%= Model.ImageFrontendData.RenderHtmlImageBasis(350, false, ImageType.Category, linkToItem: Links.CategoryDetail(Model.Category), noFollow: true) %>
                </div>
            </div>
            <div class="col-xs-7 col-sm-8">
                <p><a class="PlainTextLook" href="<%= Links.CategoryDetail(Model.Category) %>"><%: Model.CategoryDescription %></a></p>
            </div>
            <div class="col-xs-12 col-sm-8">

                <div class="LabelList">
                    <% foreach (var question in Model.Questions){ %>
                    <div class="LabelItem LabelItem-Question" style="padding-bottom: 5px">
                        <%: question.Text %>
                    </div>
            <% } %>
                </div>
            </div>
        </div>
    </div>
   <%-- <div class="pull-left">
        <div class="ImageContainer">
        </div>        
    </div>
    
    <div class="row" style="clear: left;">
        <% foreach (var question in Model.Questions){ %>
            <div class="col-xs-4" style="padding-top: 10px">
                <div class="caption"><p><%= question.Text %></p></div>
            </div>
        <% } %>
    </div>--%>
<%--    <div class="row" style="clear: left;">
        <% foreach (var question in Model.Questions){ %>
            <div class="col-xs-4">
                <p style="margin-top: 5px; text-align: center;">
                    <a href="<%= Links.AnswerQuestion(question.Text, question.Id, paramElementOnPage:1, categoryFilter:Model.CategoryName) %>" class="btn btn-xs btn-primary" role="button">Beantworten</a>
                </p>
            </div>
        <% } %>
    </div>--%>

    <div class="BottomBar">
<%--            <a href="<%= Links.AnswerQuestion(Model.FirstQText, Model.FirstQId, Model.SetId) %>" class="btn btn-primary btn-sm" role="button">Alle beantworten</a>--%>
            <div class="dropdown">
                <% var buttonId = Guid.NewGuid(); %>
                <a href="" id="<%=buttonId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonOnHover ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                    <i class="fa fa-ellipsis-v"></i>
                </a>
                <ul class="dropdown-menu" aria-labelledby="<%=buttonId %>">
                    <li><a href="<%= Model.CategoryDetailLink(Url) %>"> Detailseite zum Thema</a></li>
                </ul>
            </div>
            <a href="<%= Links.TestSessionStartForCategory(Model.CategoryName, Model.CategoryId) %>" class="btn btn-link btn-sm ButtonOnHover" role="button" rel="nofollow">
                <i class="fa fa-play-circle AnswerResultIcon">&nbsp;&nbsp;</i>WISSEN TESTEN
            </a>
        </div>

</div>