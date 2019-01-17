<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="ViewPage<CategoryChangesOverviewModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/CategoryHistory") %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="row">
        <div class="col-12">
            <h1><i class="fa fa-code-fork"></i>&nbsp; Bearbeitungshistorie aller Themen</h1>
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
                    <b><%= item.CategoryName %></b>
                </div>
                <div class="col-xs-3 show-tooltip"  data-toggle="tooltip" data-placement="left" title="<%= item.DateTime %>">
                    <a href="<%= Links.UserDetail(item.Author) %>"><img src="<%= item.AuthorImageUrl %>" height="20"/></a>
                    <b><a href="<%= Links.UserDetail(item.Author) %>"><%= item.AuthorName %></a></b>
                    um <%= item.Time %>
                </div>
                <div class="col-xs-6 pull-right">    
                    <a>
                        <i class="fa fa-comment-o"></i> #
                    </a>&nbsp;
                    
                    <a class="btn btn-sm btn-default btn-primary" href="<%= Links.CategoryDetail(item.CategoryName, item.CategoryId, item.CategoryChangeId) %>">
                        <i class="fa fa-desktop"></i> Revision anzeigen
                    </a>&nbsp;
                    
                    <a class="btn btn-sm btn-default btn-primary" href="<%= Links.CategoryHistoryDetail(item.CategoryId, item.CategoryChangeId) %>">
                        <i class="fa fa-code-fork"></i> Änderungen anzeigen
                    </a>
                </div>
            </div>       

        <% } %>
            
    <% } %>
    
    <br />
    <br />
    <div class="col-md-12 text-center">
        <div class="btn-group" role="group">
            <a type="button" class="btn btn-default" href="<%= Links.CategoryChangesOverview(Model.PageToShow-1) %>" <%= Model.PageToShow == 1 ? "disabled" : "" %>>
                Neuere Revisionen

            </a>
            <a type="button" class="btn btn-default" href="<%= Links.CategoryChangesOverview(Model.PageToShow+1) %>">
                Ältere Revisionen
            </a>
        </div>
    </div>
</asp:Content>
