<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Infrastructure" %>

<%= Styles.Render("~/bundles/css") %>
<%= Scripts.Render("~/bundles/shared") %>

<% if (!OverwrittenConfig.DevelopOffline()){ %>
    <link href='http://fonts.googleapis.com/css?family=Emilys+Candy' rel='stylesheet' type='text/css'>
    <link href='http://fonts.googleapis.com/css?family=Lato' rel='stylesheet' type='text/css'>
<% } %>

<link rel="shortcut icon" href="/favicon.ico" type="image/x-icon">

<meta name="viewport" content="width=device-width, initial-scale=1">

<script type="text/javascript">
    $(function () {
        $('a.button').hover(
		    function () { $(this).addClass('ui-state-hover'); },
		    function () { $(this).removeClass('ui-state-hover'); }
		);

        /* links with class submit should submit a forms */
        $("a.submit").click(function (event) {
            $(this).closest('form').submit();
            event.preventDefault();
        });

        $(".alert-message").alert();
        $('.show-tooltip').tooltip();
        $('.show-popover').popover();

        $('#showUserOptions').click(function () {
            alert("hello");
            $('#userOptions').dropdown();
        });
        
    });
</script>