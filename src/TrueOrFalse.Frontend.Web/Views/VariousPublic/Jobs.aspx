<%@ Page Title="Jobs bei memucho" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">

<script type="text/javascript" >

    /* Source http://www.html-advisor.com/javascript/hide-email-with-javascript-jquery/ */
    $(function () {
        var spt = $('span.mailme');
        var at = / at /;
        var dot = / dot /g;
        var addr = $(spt).text().replace(at, "@").replace(dot, ".");
        $(spt).after('<a href="mailto:' + addr + '" title="Schreibe eine E-Mail">' + addr + '</a>').hover(function () { window.status = "Schreibe eine E-Mail!"; }, function () { window.status = ""; });
        $(spt).remove();
    });
</script>    
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


<div class="row">
    <div class="col-xs-12">
        <h1 class="PageHeader" style="margin-bottom: 15px; margin-top: 0px;"><span class="ColoredUnderline GeneralMemucho">Jobs bei memucho</span></h1>
    </div>
</div>
<div class="row">
    <div class="col-xs-12 col-sm-8">

    </div>
    <div class="col-xs-12 col-sm-4" style="text-align: center;">

    </div>
</div>

</asp:Content>
