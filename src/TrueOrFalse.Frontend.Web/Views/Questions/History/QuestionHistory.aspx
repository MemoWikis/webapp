<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="ViewPage<QuestionHistoryModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="Microsoft.Owin.Security.DataHandler.Encoder" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/QuestionHistory") %>
    <% 
        Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem {Text = Model.QuestionName, Url = Model.QuestionUrl, ToolTipText = Model.QuestionName});
        Model.TopNavMenu.IsCategoryBreadCrumb = false;
    %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="row">
        <div class="col-12">
            <h1><i class="fa fa-code-fork"></i>&nbsp; Bearbeitungshistorie '<%= Model.QuestionName %>'</h1>
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
                <div class="col-xs-3 show-tooltip"  data-toggle="tooltip" data-placement="left" title="<%= item.DateTime %>">
                    vor <%= item.ElapsedTime %> um <%= item.Time %>
                </div>
                <div class="col-xs-6 pull-right">    
                    <a>
                        <i class="fa fa-comment-o"></i> #
                    </a>&nbsp;
                    
                    <%--TODO FK <a class="btn btn-sm btn-default btn-primary" href="<%= Links.QuestionDetail(Model.QuestionName, Model.QuestionId, item.RevisionId) %>">
                        <i class="fa fa-desktop"></i> Revision anzeigen
                    </a>&nbsp;--%>
                    
                    <a class="btn btn-sm btn-default btn-primary" href="<%= Links.QuestionHistoryDetail(Model.QuestionId, item.RevisionId) %>">
                        <i class="fa fa-code-fork"></i> Änderungen anzeigen
                    </a>
                </div>
            </div>       

        <% } %>
            

    <% } %>
</asp:Content>
