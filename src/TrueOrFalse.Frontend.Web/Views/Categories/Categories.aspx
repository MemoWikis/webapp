<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<CategoriesModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Scripts.Render("~/Views/Categories/Categories.js") %>
    <%= Styles.Render("~/Views/Categories/Categories.css") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="span10">
        <div class="box">
        <div>
            <h1 style="float: left; width: 200px; padding-bottom: 10px;" >Kategorien</h1>
            <div style="float: right; padding-top: 5px; padding-bottom: 15px;">
                <a href="<%= Url.Action("Create", "EditCategory") %>" class="btn" style="display: inline;"><i class="icon-plus-sign"></i> Kategorie erstellen</a>
            </div>
        </div>

        <div class="box-content" style="clear: both;">
            <% foreach (var row in Model.CategoryRows){
                Html.RenderPartial("CategoryRow", row);
            } %>
        </div>
        </div>
    </div>

</asp:Content>
