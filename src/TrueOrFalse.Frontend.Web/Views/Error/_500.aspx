<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<BaseModel>" 
        Title="Error"
%>

<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1 style="margin-top: 0px;">Autsch! Ein Fehler ist aufgetreten.</h1>
    
    <p>
        Wir kümmern uns um das Problem. 
        <ul>
            <li>Wir wurden per Email informiert.</li>
            <li>Bei dringenden Fragen kannst du Robert unter 0178 18 668 48 erreichen.</li>
            <li>Oder schicke eine Email an <a href="mailto:an@robert-m.de">an@robert-m.de</a>.</li>
        </ul>
    </p>
    
    <img src="/Images/500.jpg" width="200" style="padding: 20px 0 100px 0" />

</asp:Content>
