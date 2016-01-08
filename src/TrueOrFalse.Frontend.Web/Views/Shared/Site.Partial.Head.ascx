<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.Web.Optimization" %>
<meta http-equiv="X-UA-Compatible" content="IE=edge" />
<%= Styles.Render("~/bundles/css") %>
<%= Scripts.Render("~/bundles/shared") %>

<% if (!Settings.DevelopOffline()){ %>
    <link href='//fonts.googleapis.com/css?family=Emilys+Candy' rel='stylesheet' type='text/css'>
    <link href='//fonts.googleapis.com/css?family=Lato' rel='stylesheet' type='text/css'>
    <link href='//fonts.googleapis.com/css?family=Roboto:500' rel='stylesheet' type='text/css'>
    <link href='//fonts.googleapis.com/css?family=Roboto+Slab' rel='stylesheet' type='text/css'>
    <link href="//fonts.googleapis.com/css?family=Open+Sans:400italic,700italic,400,600,700,800&subset=latin,cyrillic-ext,greek-ext,greek,vietnamese,latin-ext,cyrillic" rel="stylesheet" type="text/css" />
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