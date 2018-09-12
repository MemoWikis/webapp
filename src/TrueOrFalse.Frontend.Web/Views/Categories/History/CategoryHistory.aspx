<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<CategoryHistoryModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="row">
        <div class="col-12">
            <%= Model.CategoryName %>
        </div>
    </div>
    <% foreach (var day in Model.Days) { %>
    
        <div class="row">
            <div class="col-2">
                <%= day.Date %>
            </div>
        </div>
        
        <% foreach (var item in day.Items){ %>

            <div class="row">
                <div class="col-2">
                    <%= item.AuthorName %>
                </div>
            </div>            

        <% } %>
            

    <% } %>
</asp:Content>
