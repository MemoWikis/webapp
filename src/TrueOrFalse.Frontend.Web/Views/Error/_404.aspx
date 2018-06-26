<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="System.Web.Mvc.ViewPage<BaseModel>" 
        Title="Error"%>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/Views/Error/Error.css") %>
</asp:Content>

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
    <img src="/Images/Error/memo-404_german_600.png" class="ErrorImage" />

    <p>
        Fehler 404. Wenn du meinst, dass hier eigentlich etwas sein müsste, dann <a href="#" onclick="_urq.push(['Feedback_Open']);">melde es uns bitte hier</a> oder per E-Mail an <span class="mailme">team at memucho dot de</span>
    </p>
    <p>
         <a href="/"><i class="fa fa-chevron-left">&nbsp;</i>Zur Startseite</a>
    </p>
</asp:Content>
