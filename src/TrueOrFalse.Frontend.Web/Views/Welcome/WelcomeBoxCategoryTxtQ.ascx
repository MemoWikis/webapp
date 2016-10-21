<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<WelcomeBoxCategoryTxtQModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div class="Card CardBig Category">
    <header style="">
        <h6 style="margin-bottom: 5px; margin-top: 0px; color: #a3a3a3;">Kategorie mit 
            <a href="<%: Links.QuestionWithCategoryFilter(Url, Model.CategoryName, Model.CategoryId) %>">
                <%= Model.QuestionCount %> Fragen
            </a>
        </h6>
        <h4><%: Model.CategoryName %></h4>
    </header>
    <div class="CardContent">
        <div class="row">
            <div class="col-xs-4">
                <div class="ImageContainer">
                    <%= Model.ImageFrontendData.RenderHtmlImageBasis(180, false, ImageType.Category) %>
                </div>
            </div>
            <div class="col-xs-8 xxs-stack">
                <p><%: Model.CategoryDescription %></p>
            </div>
            <div class="col-xs-8 xxs-stack pull-right">

                <div class="LabelList">
                    <% foreach (var question in Model.Questions){ %>
                    <div class="LabelItem LabelItem-Question" style="padding-bottom: 10px">
                        <%= question.Text %>
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
                    <a href="<%= Links.AnswerQuestion(Url, question.Text, question.Id, paramElementOnPage:1, categoryFilter:Model.CategoryName) %>" class="btn btn-xs btn-primary" role="button">Beantworten</a>
                </p>
            </div>
        <% } %>
    </div>--%>

    <div class="BottomBar">
            <%--<a href="<%= Links.AnswerQuestion(Url, Model.FirstQText, Model.FirstQId, Model.SetId) %>" class="btn btn-primary btn-sm" role="button">Alle beantworten</a>--%>
            <%--<div class="dropdown">
                <% var buttonId = Guid.NewGuid(); %>
                <a href="#" id="<%=buttonId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonOnHover ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                    <i class="fa fa-ellipsis-v"></i>
                </a>
                <ul class="dropdown-menu" aria-labelledby="<%=buttonId %>">
                    <li><a href="#"> Action 1</a></li>
                    <li><a href="#"> Action 2</a></li>
                </ul>
            </div>--%>
            <a href="<%= Links.TestSessionStartForCategory(Model.CategoryId) %>" class="btn btn-link btn-sm ButtonOnHover" role="button" rel="nofollow">
                &nbsp;JETZT TESTEN
            </a>
        </div>

</div>