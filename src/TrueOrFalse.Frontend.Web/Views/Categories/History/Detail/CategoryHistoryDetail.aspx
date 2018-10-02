<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<CategoryHistoryDetailModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/CategoryHistoryDetail") %>
    <%= Scripts.Render("~/bundles/js/CategoryHistoryDetail") %>
    <%= Scripts.Render("~/bundles/js/diff2html") %>
    <%= Styles.Render("~/bundles/diff2html") %>
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
                <a href="<%= Links.UserDetail(Model.CurrentChange.Author) %>"><img src="<%= Model.AuthorImageUrl %>" height="20"/></a>
                <b><a href="<%= Links.UserDetail(Model.CurrentChange.Author) %>"><%= Model.AuthorName %></a></b><br/>
                vom <%= Model.CurrentChange.DateCreated %>
            </div>
            
            <div class="col-xs-9">
                <nav class="navbar-right">
                    <a class="btn btn-primary navbar-btn" href="<%= Links.CategoryHistory(Model.CategoryId) %>">
                        <i class="fa fa-chevron-left"></i> Zurück zur Bearbeitungshistorie
                    </a>
                    <a class="btn btn-default navbar-btn" href="<%= Links.CategoryRestore(Model.CategoryId, Model.CurrentChange.Id) %>">
                        <i class="fa fa-undo"></i> Wiederherstellen
                    </a>
                </nav>
            </div>
        </div>
        
        <div class="row">
            <div class="col-12">
                <% if (Model.PrevChange == null) {  %>
                    <br />
                    <div id="initialRevision" class="alert alert-info" role="alert">
                        Dies ist die <b>initiale Revision</b>, weswegen hier keine Änderungen angezeigt werden können.
                    </div>
                <% } else { %>
                    <br/>
                    <input type="hidden" id="currentRevData" value="<%= Server.HtmlEncode(Model.CurrentData) %>"/>
                    <input type="hidden" id="previousRevData" value="<%= Server.HtmlEncode(Model.PrevData) %>"/>
                    <input type="hidden" id="currentRevDateCreated" value="<%= Model.CurrentChange.DateCreated %>" />
                    <input type="hidden" id="previousRevDateCreated" value="<%= Model.PrevChange.DateCreated %>" />
                    <div id="outputdiv"></div>
                    <div id="nochangesdiv" style="display: none;">
                        <h4><i class="fa fa-check"></i> Zwischen den beiden Revisionen gibt es keine inhaltlichen Unterschiede.</h4>
                    </div>
                <% } %>
            </div>
        </div>
    </div>

</asp:Content>
