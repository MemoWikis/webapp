<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
    Inherits="System.Web.Mvc.ViewPage<CategoryModel>" Title="Kategorie" %>

<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/category") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
            
    <div class="col-md-6  category category" style="margin-bottom: 30px;">
        <h2><%= Model.Name %></h2>
    </div>
    <div class="col-md-3">
        <div class="pull-right">
            <div>
                <a href="<%= Url.Action(Links.Categories, Links.CategoriesController) %>" style="font-size: 12px; margin: 0px;"><i class="fa fa-list"></i>&nbsp;zur Übersicht</a><br/>
                <% if(Model.IsOwner){ %>
                    <a href="<%= Links.CategoryEdit(Url, Model.Id) %>" style="font-size: 12px; margin: 0px;"><i class="fa fa-pencil"></i>&nbsp;bearbeiten</a> 
                <% } %>
            </div>
        </div>
    </div>

    <div class="col-md-6">
        <%= Model.Description %>
    </div>
       
    <div class="col-md-6">                    
        <h4 style="margin-top: 0px;">Fragen (<%=Model.CountQuestions %>)</h4>                    
        <% foreach(var question in Model.TopQuestions){ %>
            <div>
                - <a href="<%= Links.AnswerQuestion(Url, question) %>"><%= question.GetShortTitle(80) %></a>
            </div>
        <% } %>
        <a href="<%= Links.QuestionWithCategoryFilter(Url, Model.Category) %>" class="btn btn-info btn-sm" style="margin-top: 10px; margin-bottom: 10px;">
            Alle <%: Model.CountQuestions %> Fragen dieser Kategorie zeigen
        </a>
                
        <h4>Fragesätze (<%=Model.CountSets %>)</h4>
        <% foreach(var set in Model.TopSets){ %>
            <div>
                - <a href="<%= Links.SetDetail(Url, set) %>"><%= set.Name %></a>
            </div>
        <% } %>

        <h4>Ersteller (<%=Model.CountCreators %>)</h4>
    </div>         
    
    <div class="col-md-3">
        <div>
            <img src="<%= Model.ImageUrl %>" class="img-responsive" style="border-radius:5px;" />
        </div>
    </div>

</asp:Content>