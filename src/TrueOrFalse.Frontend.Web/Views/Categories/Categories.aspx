<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<CategoriesModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div>
        <h2 style="float: left;">Kategorien</h2>
        <div style="float: right;">
            <%= Buttons.Link("Kategorie erstellen", Links.CreateCategory, Links.EditCategoryController, ButtonIcon.Add)%>
        </div>
    </div>

    <div style="clear: both;">
        <% foreach (var row in Model.CategoryRows){
               Html.RenderPartial("CategoryRow", row);
        } %>
    </div> 

</asp:Content>
