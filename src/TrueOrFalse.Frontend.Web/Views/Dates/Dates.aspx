<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<DatesModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">    
    <h2 style="margin-top: 0; margin-bottom: 20px;">
        <span class="ColoredUnderline Date" style="padding-right: 3px;">Termine</span>
    </h2>
        
    <% if(!Model.IsLoggedIn){ %>

        <div class="bs-callout bs-callout-danger">
            <h4>Anmelden oder registrieren</h4>
            <p>Um Termine zu erstellen, musst du dich <a href="/Anmelden">anmelden</a> oder dich <a href="/Registrieren">registrieren</a>.</p>
        </div>

    <% }else{ %>
    
    <% } %>
</asp:Content>