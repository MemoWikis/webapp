<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
    Inherits="System.Web.Mvc.ViewPage<CategoryModel>" Title="Kategorie" %>

<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/category") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
            
    <div class="col-md-6 category">
        <h2 class="pull-left"><%= Model.Name %></h2>
        <div class="pull-right">
            <div>
                <a href="<%= Url.Action(Links.Categories, Links.CategoriesController) %>" style="font-size: 12px; margin: 0px;"><i class="fa fa-list"></i>&nbsp;zur Übersicht</a><br/>
                <% if(Model.IsOwner){ %>
                    <a href="<%= Links.CategoryEdit(Url, Model.Id) %>" style="font-size: 12px; margin: 0px;"><i class="fa fa-pencil"></i>&nbsp;bearbeiten</a> 
                <% } %>
            </div>
        </div>
        <div style="clear: both;">
            <%= Model.Description %>
        </div>
       
        <div class="row">
            <div class="col-md-12">                    
                <h4>Fragen (<%=Model.CountQuestions %>)</h4>                    
                    <% foreach(var question in Model.TopQuestions){ %>
                        <div>
                            - <a href="<%= Links.AnswerQuestion(Url, question) %>"><%= question.GetShortTitle(80) %></a>
                        </div>
                    <% } %>
                <h4>Fragesätze (<%=Model.CountSets %>)</h4>
                <% foreach(var set in Model.TopSets){ %>
                    <div>
                        - <a href="<%= Links.SetDetail(Url, set) %>"><%= set.Name %></a>
                    </div>
                <% } %>

                <h4 >Ersteller (<%=Model.CountCreators %>)</h4>
            </div>         
        </div>

    </div>
    <div class="col-md-3">
        <div>
            <div class="box">
                <img src="<%= Model.ImageUrl %>"/>
            </div>
        </div>
    </div>
</asp:Content>