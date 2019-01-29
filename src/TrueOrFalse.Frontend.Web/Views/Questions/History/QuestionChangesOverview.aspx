<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="ViewPage<QuestionChangesOverviewModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/QuestionHistory") %>
    <% 
        Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem
        {
            Text = "Bearbeitungshistorie aller Fragen",
            Url = Links.QuestionChangesOverview(1),
            ToolTipText = "Bearbeitungshistorie aller Fragen"
        });
        Model.TopNavMenu.IsCategoryBreadCrumb = false;
    %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="row">
        <div class="col-12">
            <h1><i class="fa fa-list"></i>&nbsp; Bearbeitungshistorie aller Fragen</h1>
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
                <div class="col-xs-6">
                    <a href="<%= Links.AnswerQuestion(item.QuestionName, item.QuestionId) %>">
                        <b><%= item.QuestionName %></b>
                    </a>
                </div>
                <div class="col-xs-3 show-tooltip"  data-toggle="tooltip" data-placement="left" title="<%= item.DateTime %>">
                    <a href="<%= Links.UserDetail(item.Author) %>"><img src="<%= item.AuthorImageUrl %>" height="20"/></a>
                    <b><a href="<%= Links.UserDetail(item.Author) %>"><%= item.AuthorName %></a></b>
                    um <%= item.Time %>
                </div>
                <div class="col-xs-3 pull-right">    
                    <%--<a>
                        <i class="fa fa-comment-o"></i> #
                    </a>&nbsp;--%>
                    
                    <a class="btn btn-sm btn-default btn-primary" href="<%= Links.QuestionHistoryDetail(item.QuestionId, item.RevisionId) %>">
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
            <a type="button" class="btn btn-default" href="<%= Links.QuestionChangesOverview(Model.PageToShow-1) %>" <%= Model.PageToShow == 1 ? "disabled" : "" %>>
                Neuere Revisionen

            </a>
            <a type="button" class="btn btn-default" href="<%= Links.QuestionChangesOverview(Model.PageToShow+1) %>">
                Ältere Revisionen
            </a>
        </div>
    </div>
</asp:Content>
