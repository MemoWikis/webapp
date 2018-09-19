<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<CategoryHistoryModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="Microsoft.Owin.Security.DataHandler.Encoder" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/CategoryHistory") %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="row">
        <div class="col-12">
            <h1>Bearbeitungshistorie '<%= Model.CategoryName %>' <i class="fa fa-code-fork"></i></h1>
        </div>
    </div>
    <% foreach (var day in Model.Days) { %>
    
        <div class="row">
            <div class="col-md-12">
                <h3><%= day.Date %></h3>
            </div>
        </div>
        
        <% foreach (var item in day.Items){ %>
    
            <div class="row change-detail-model">
                <div class="col-xs-3">
                    <a href="<%= Links.UserDetail(item.Author) %>"><img src="<%= item.AuthorImageUrl %>" height="20"/></a>
                    <b><a href="<%= Links.UserDetail(item.Author) %>"><%= item.AuthorName %></a></b>
                </div>
                <div class="col-xs-6 show-tooltip"  data-toggle="tooltip" data-placement="left" title="<%= item.DateTime %>">
                    Änderung vor <%= item.ElapsedTime %> (um <%= item.Time %>)
                </div>
                <div class="col-xs-3 pull-right">    
                    <a>
                        <i class="fa fa-comment-o"></i> #
                    </a>&nbsp;
                    
                    <a class="btn btn-sm btn-default btn-primary" href="<%= Links.CategoryHistoryDetail(Model.CategoryId, item.CategoryChangeId) %>">
                        <i class="fa fa-eye"></i> Anzeigen
                    </a>
                </div>
            </div>       

        <% } %>
            

    <% } %>
</asp:Content>
