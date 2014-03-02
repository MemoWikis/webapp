<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<MessageModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/message") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="col-md-6">
        
        <div class="row">
            <h2 style="margin-top: 0px;">Nachrichten</h2>    
        </div>

        <% foreach(var msg in Model.Rows){ %>
    
            <div class="row msgRow rowBase ">
                <div class="header col-lg-12">
                    <h4><%: msg.Subject %></h4>
                </div>

                <div class="col-lg-12 body">
                    <%= msg.Body %>
                </div>

                <div class="col-lg-7 footer">
                    vor 12 min
                </div>
                <div class="col-lg-5  footer">
                    <a href="#" class="pull-right">als gelesen makieren</a>
                </div>
            </div>

        <% } %>        

    </div>
    


</asp:Content>