<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<BaseModel>" 
        Title="Error"
%>

<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript" >

    /* Source http://www.html-advisor.com/javascript/hide-email-with-javascript-jquery/ */
    $(function () {
        var spt = $('span.mailme');
        var at = / at /;
        var dot = / dot /g;
        var addr = $(spt).text().replace(at, "@").replace(dot, ".");
        $(spt).after('<a href="mailto:' + addr + '" title="Send an email">' + addr + '</a>').hover(function () { window.status = "Send a letter!"; }, function () { window.status = ""; });
        $(spt).remove();
    });
</script>    
        
    <h1 style="margin-top: 0px;">Autsch! Ein Fehler ist aufgetreten.</h1>
    
    <p>
        Wir kümmern uns um das Problem. 
        <ul>
            <li>Wir wurden per Email informiert.</li>
            <li>Bei dringenden Fragen kannst du Christof unter 01577-6825707 erreichen.</li>
            <li>Oder schicke eine Email an <span class="mailme">team at memucho dot de</span>.</li>
        </ul>
    </p>
    
    <img src="/Images/Logo/memucho_MEMO_angry_blau_w220.png" width="220" height="231" style="margin: 20px 0 50px 20px" />

</asp:Content>
