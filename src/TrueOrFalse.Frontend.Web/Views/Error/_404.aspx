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

    <h1 style="margin-top: 0px;">Seite nicht gefunden.</h1>
    <p>
        Leider konnten wir die angefragte Seite nicht finden (Fehler 404).<br/>
        Wenn du meinst, dass sei ein Fehler, dann <a href="#" onclick="_urq.push(['Feedback_Open']);">melde ihn uns bitte hier</a> oder per E-Mail an <span class="mailme">team at memucho dot de</span>
    </p>
    <p>
         <a href="/"><i class="fa fa-chevron-left">&nbsp;</i>Zur Startseite</a>
    </p>
    <img src="/Images/Logo/memucho_MEMO_angry_blau_w220.png" width="220" height="231" style="margin: 20px 0 50px 20px" />
</asp:Content>
