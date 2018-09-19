<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<CategoryHistoryDetailModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/CategoryHistoryDetail") %>
    <%= Scripts.Render("~/bundles/js/CategoryHistoryDetail") %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="row">
        <div class="col-12">
            <h1>Änderungen für '<%= Model.CategoryName %>'</h1>
        </div>
    </div>
    
    <div class="change-detail-model">
        <div class="row">
            <div class="col-xs-3">            
                <a href="<%= Links.UserDetail(Model.CategoryChange.Author) %>"><img src="<%= Model.AuthorImageUrl %>" height="20"/></a>
                <b><a href="<%= Links.UserDetail(Model.CategoryChange.Author) %>"><%= Model.AuthorName %></a></b><br/>
                vom <%= Model.CategoryChange.DateCreated %>
            </div>
            
            <div class="col-xs-9">
                <a class="btn btn-sm btn-default">
                    <i class="fa fa-undo"></i> Wiederherstellen
                </a>
            </div>
        </div>
        
        <br/>
    
        <div class="row">
            <div class="col-xs-6">
                <textarea rows="10" cols="80">
                </textarea>
            </div>
        </div>
    
    </div>

    <br/>

    <div class="row">
        <div class="col-12">
            <a href="<%= Links.CategoryHistory(Model.CategoryId) %>">
                <i class="fa fa-chevron-left"></i> Zurück zur Bearbeitungshistorie
            </a>
        </div>
    </div>

</asp:Content>
