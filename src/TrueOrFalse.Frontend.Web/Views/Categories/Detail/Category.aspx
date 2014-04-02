<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
    Inherits="System.Web.Mvc.ViewPage<CategoryModel>" Title="Kategorie" %>

<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/category") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-9">
        <div class="row">
            <div class="col-xs-12">
                <div class="row">
                    <div class="col-xs-9 xxs-stack category" style="margin-bottom: 10px;">
                        <h2 style="margin-top: 0px;"><%= Model.Name %></h2>
                    </div>
                    <div class="col-xs-3 xxs-stack">
                        <div class="navLinks">
                            <a href="<%= Url.Action(Links.Categories, Links.CategoriesController) %>" style="font-size: 12px;"><i class="fa fa-list"></i>&nbsp;zur Übersicht</a>
                            <% if(Model.IsOwnerOrAdmin){ %>
                                <a href="<%= Links.CategoryEdit(Url, Model.Id) %>" style="font-size: 12px;"><i class="fa fa-pencil"></i>&nbsp;bearbeiten</a> 
                            <% } %>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-lg-10 col-xs-9 xxs-stack">
                <%= Model.Description %> asdfasdfasdfasdf
            </div>
       
            <div class="col-lg-10 col-xs-9 xxs-stack">
        
                <% if(Model.CategoriesParent.Count > 0){ %>
                    <h4 style="margin-top: 0px;"><i class="fa fa-arrow-up"></i> Elternkategorien</h4>
                    <div style="margin-bottom: 12px;">
                        <% foreach(var category in Model.CategoriesParent){ %>
                            <a href="<%= Links.CategoryDetail(Url, category) %>"><span class="label label-category"><%= category.Name %></span></a>
                        <% } %>
                    </div>
                <% } %>
        
                <% if(Model.CategoriesChildren.Count > 0){ %>
                    <h4 style="margin-top: 0px;"><i class="fa fa-arrow-down"></i> Kindkategorien</h4>
                    <div style="margin-bottom: 12px;">
                        <% foreach(var category in Model.CategoriesChildren){ %>
                            <a href="<%= Links.CategoryDetail(Url, category) %>"><span class="label label-category"><%= category.Name %></span></a>
                        <% } %>
                    </div>
                <% } %>

                <% if(Model.CountQuestions > 0){ %>
                    <h4 style="margin-top: 0px;">Fragen (<%=Model.CountQuestions %>)</h4>                    
                    <% foreach(var question in Model.TopQuestions){ %>
                        <div style="white-space: nowrap; overflow: hidden; text-overflow:ellipsis;">
                            - <a href="<%= Links.AnswerQuestion(Url, question) %>"><%= question.GetShortTitle(150) %></a>
                        </div>
                    <% } %>
                    <a href="<%= Links.QuestionWithCategoryFilter(Url, Model.Category) %>" class="btn btn-info btn-sm" style="margin-top: 10px; margin-bottom: 10px;">
                        Alle <%: Model.CountQuestions %> Fragen dieser Kategorie zeigen
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
                    <img src="<%= Model.ImageUrl %>" class="img-responsive" style="border-radius:5px;" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>