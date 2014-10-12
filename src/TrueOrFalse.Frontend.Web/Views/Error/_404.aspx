<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<BaseModel>" 
        Title="Error"
%>

<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1 style="margin-top: 0px;">404! Seite nicht gefunden.</h1>
    <p>
        Wir haben die angefragte Seite leider nicht gefunden. (<a href="/">-> zur Startseite</a>)
    </p>
    <img src="/Images/404.jpg" style="padding: 20px 0 100px 0" />
</asp:Content>
