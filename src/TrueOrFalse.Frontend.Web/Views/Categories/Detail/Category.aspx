<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
    Inherits="System.Web.Mvc.ViewPage<CategoryModel>"%>

<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/category") %>
    <title>Kategorie - <%=Model.Name %> </title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="xxs-stack col-xs-12">
            <div class="row">
                <div class="col-xs-9 xxs-stack category">
                    <h2 style="margin-top: 0; margin-bottom: 10px;"><%= Model.Name %></h2>
                    <% if(!String.IsNullOrEmpty(Model.Description)){ %>
                        <div style="margin-bottom: 12px;">
                            <%= Model.Description %>
                        </div>
                    <% } %>
                </div>
                <div class="col-xs-3 xxs-stack">
                    <div class="navLinks">
                        <a href="<%= Url.Action(Links.Categories, Links.CategoriesController) %>" style="font-size: 12px;"><i class="fa fa-list"></i>&nbsp;zur Übersicht</a>
                        <% if(Model.IsOwnerOrAdmin){ %>
                            <a href="<%= Links.CategoryEdit(Url, Model.Id) %>" style="font-size: 12px;"><i class="fa fa-pencil"></i>&nbsp;bearbeiten</a> 
                        <% } %>
                        <a href="<%= Links.CreateQuestion(Url, Model.Id) %>" style="font-size: 12px;"><i class="fa fa-plus-circle"></i>&nbsp;Frage hinzufügen</a>
                    </div>
                </div>
            </div>
        </div>
            
        <div class="col-lg-10 col-xs-9 xxs-stack">
        
            <% if(Model.CategoriesParent.Count > 0){ %>
                <h4 style="margin-top: 0;"><i class="fa fa-arrow-up"></i> Elternkategorien</h4>
                <div style="margin-bottom: 12px;">
                    <% foreach(var category in Model.CategoriesParent){ %>
                        <a href="<%= Links.CategoryDetail(Url, category) %>"><span class="label label-category"><%= category.Name %></span></a>
                    <% } %>
                </div>
            <% } %>
        
            <% if(Model.CategoriesChildren.Count > 0){ %>
                <h4 style="margin-top: 0;">
                    <i class="fa fa-arrow-down"></i> Kindkategorien
                    <i class="fa fa-plus-circle show-tooltip cat-color add-new" 
                       style="font-size: 14px; color: #99ccff; cursor: pointer"
                       onclick="window.location = '/Kategorien/Erstelle?parent=<%= Model.Category.Name %>'; return false; " 
                       data-original-title="Neue Kindkategorie erstellen"></i>
                </h4>
                <div style="margin-bottom: 12px;">
                    <% foreach(var category in Model.CategoriesChildren){ %>
                        <a href="<%= Links.CategoryDetail(Url, category) %>"><span class="label label-category"><%= category.Name %></span></a>
                    <% } %>
                </div>
            <% } %>

            <% if(Model.CountQuestions > 0){ %>
                <h4 style="margin-top: 0;">Fragen (<%=Model.CountQuestions %>)</h4>                    
                <%
                    var index = 0;
                    foreach(var question in Model.TopQuestions){ 
                        index++;
                %>
                    <div style="white-space: nowrap; overflow: hidden; -moz-text-overflow:ellipsis; text-overflow:ellipsis;">
                        - <a href="<%= Links.AnswerQuestion(Url, question, paramElementOnPage: index, categoryFilter:Model.Name) %>"><%= question.GetShortTitle(150) %></a>
                    </div>
                <% } %>
                <a href="<%= Links.QuestionWithCategoryFilter(Url, Model.Category) %>" class="" style="display:block; margin-top: 10px; margin-bottom: 10px; font-style: italic">
                    <i class="fa fa-forward" style="color: #afd534;"></i> Alle <%: Model.CountQuestions %> Fragen dieser Kategorie zeigen
                </a>
            <% } %>
            
            <% if(Model.CountSets > 0){ %>    
                <h4>Fragesätze (<%=Model.CountSets %>)</h4>
                <% foreach(var set in Model.TopSets){ %>
                    <div>
                        - <a href="<%= Links.SetDetail(Url, set) %>"><%= set.Name %></a>
                    </div>
                <% } %>
            <% } %>
        
            <% if (Model.CountCreators > 0){ %>
                <h4>Ersteller (<%=Model.CountCreators %>)</h4>
            <% } %>
        </div>         
        
        <div class="col-lg-2 col-xs-3 xxs-stack">
            <div>
                <img src="<%= Model.ImageUrl %>" class="img-responsive" style="-ms-border-radius:5px; border-radius:5px;" />
            </div>
                
            <% if (!String.IsNullOrEmpty(Model.WikiUrl)){ %>
                <div style="text-overflow: ellipsis; overflow: hidden;  white-space: nowrap; ">
                    <a href="<%= Model.WikiUrl %>" style="margin-left: -3px;" class="show-tooltip" title="<div style='white-space: normal; word-wrap: break-word; text-align:left; '>Link&nbsp;auf&nbsp;Wikipedia:&nbsp;<%= Model.WikiUrl %></div>" data-placement="left" data-html="true">
                        <img src="/Images/wiki-24.png" style="margin-top: -1px;" /><%= Model.WikiUrl %>
                    </a>
                </div>
            <% } %>
        </div>
    </div>
</asp:Content>